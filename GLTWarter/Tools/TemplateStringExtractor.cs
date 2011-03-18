using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;
using System.Reflection;

namespace GLTWarter.Tools
{
    class TemplateStringExtractorWorkItem
    {
        public DataTemplate Template;
        public object Context;
        public HostVisual Host;
        public string Result;
    }

    internal class TemplateStringExtractor
    {
        bool done;
        Queue<TemplateStringExtractorWorkItem> workQueue = new Queue<TemplateStringExtractorWorkItem>();
        Queue<TemplateStringExtractorWorkItem> resultQueue = new Queue<TemplateStringExtractorWorkItem>();

        public Queue<TemplateStringExtractorWorkItem> ResultQueue
        {
            get { return resultQueue; }
        }

        public bool IsDone
        {
            get { return done; }
        }

        public void QueueWorkItem(TemplateStringExtractorWorkItem workItem)
        {
            lock (workQueue)
            {
                workQueue.Enqueue(workItem);
                Monitor.Pulse(workQueue);
            }
        }

        public void SignalAllWorkItemsQueued()
        {
            this.QueueWorkItem(null);
        }

        public void Run(object context)
        {
            done = false;
            TemplateStringExtractorWorkItem wi;
            while (true)
            {
                lock (workQueue)
                {
                    if (workQueue.Count == 0) Monitor.Wait(workQueue);
                    wi = workQueue.Dequeue();
                }
                if (wi == null) break;

                wi.Result = Process(wi.Host, wi.Template, wi.Context);
                lock (resultQueue)
                {
                    resultQueue.Enqueue(wi);
                    Monitor.Pulse(resultQueue);
                }
            }
            done = true;
            lock (resultQueue)
            {
                Monitor.Pulse(resultQueue);
            }
        }

        public void RunOne(object context)
        {
            TemplateStringExtractorWorkItem wi = context as TemplateStringExtractorWorkItem;
            wi.Result = Process(wi.Host, wi.Template, wi.Context);
            lock (lockOne)
            {
                Monitor.Pulse(lockOne);
            }
        }

        object lockOne = new object();
        public string ProcessOne(DataTemplate template, object context)
        {
            TemplateStringExtractorWorkItem wi = new TemplateStringExtractorWorkItem();
            wi.Host = new HostVisual();
            wi.Template = template;
            wi.Context = context;

            Thread proc = new Thread(this.RunOne);
            proc.SetApartmentState(ApartmentState.STA);
            proc.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            proc.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            lock (lockOne)
            {
                proc.Start(wi);
                Monitor.Wait(lockOne);
            }
            return wi.Result;
        }

        string Process(HostVisual host, DataTemplate template, object context)
        {
            string result = null;
            
            DispatcherFrame frame = new DispatcherFrame();

            FrameworkElement fe = null;

            RoutedEventHandler handlerLoad = null;
            handlerLoad = new RoutedEventHandler(delegate(object sender, RoutedEventArgs e)
            {
                fe.Loaded -= handlerLoad;
                result = ExtractStringFromTextBlock(sender as FrameworkElement);
                frame.Continue = false;
            });
            
            if (template.Dispatcher == null)
            {
                VisualTargetPresentationSource visualTargetPS = new VisualTargetPresentationSource(host);
                fe = template.LoadContent() as FrameworkElement;
                fe.DataContext = context;
                fe.Loaded += handlerLoad;
                visualTargetPS.RootVisual = fe;
            }
            else
            {
                template.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        fe = template.LoadContent() as FrameworkElement;
                        fe.DataContext = context;
                        fe.Loaded += handlerLoad;
                    }
                );
                host.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        VisualTargetPresentationSource visualTargetPS = new VisualTargetPresentationSource(host);
                        visualTargetPS.RootVisual = fe;
                    }
                );
            }
            Dispatcher.PushFrame(frame);

            return result;
        }

        static string ExtractFirstBinding(DependencyObject root)
        {
            string ret = "";
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                DependencyObject vc = VisualTreeHelper.GetChild(root, i);
                if (vc is TextBlock)
                {
                    ret += ExtractStringFromInlines((vc as TextBlock).Inlines) + "\n";
                }
                else
                {
                    ret += ExtractStringFromTextBlock(vc);
                }
            }
            return ret.TrimEnd('\n');
        }


        public static string ExtractStringFromTextBlock(DependencyObject root)
        {
            string ret = "";
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                DependencyObject vc = VisualTreeHelper.GetChild(root, i);
                if (vc as FrameworkElement != null && (vc as FrameworkElement).Visibility != Visibility.Visible) continue;
                if (vc is TextBlock)
                {
                    ret += ExtractStringFromInlines((vc as TextBlock).Inlines) + "\n";
                }
                else
                {
                    ret += ExtractStringFromTextBlock(vc) + "\n";
                }
            }
            if (count == 0 && root is TextBlock && (root as FrameworkElement).Visibility == Visibility.Visible)
            {
                ret += ExtractStringFromInlines((root as TextBlock).Inlines) + "\n";
            }
            return ret.TrimEnd('\n');
        }

        static string ExtractStringFromInlines(InlineCollection inlines)
        {
            string ret = string.Empty;
            for (Inline il = inlines.FirstInline; il != null; il = il.NextInline)
            {
                if (il is Span)
                {
                    ret += ExtractStringFromInlines((il as Span).Inlines);
                }
                else if (il is LineBreak)
                {
                    ret += "\n";
                }
                else if (il is InlineUIContainer)
                {
                    if ((il as InlineUIContainer).Child as TextBlock != null &&
                        (il as InlineUIContainer).Child.Visibility == Visibility.Visible)
                    {
                        ret += ExtractStringFromInlines(((il as InlineUIContainer).Child as TextBlock).Inlines);
                    }
                }
                else if (il is Run)
                {
                    ret += (il as Run).Text;
                }
            }
            return ret;
        }

        class VisualTargetPresentationSource : PresentationSource
        {
            public VisualTargetPresentationSource(HostVisual hostVisual)
            {
                _visualTarget = new VisualTarget(hostVisual);
            }

            public override Visual RootVisual
            {
                get
                {
                    return _visualTarget.RootVisual;
                }
                set
                {
                    Visual oldRoot = _visualTarget.RootVisual;

                    // Set the root visual of the VisualTarget.  This visual will
                    // now be used to visually compose the scene.
                    _visualTarget.RootVisual = value;

                    // Tell the PresentationSource that the root visual has
                    // changed.  This kicks off a bunch of stuff like the
                    // Loaded event.
                    RootChanged(oldRoot, value);

                    // Kickoff layout...
                    UIElement rootElement = value as UIElement;
                    if (rootElement != null)
                    {
                        rootElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                        rootElement.Arrange(new Rect(rootElement.DesiredSize));
                    }
                }
            }

            protected override CompositionTarget GetCompositionTargetCore()
            {
                return _visualTarget;
            }

            public override bool IsDisposed
            {
                get
                {
                    // We don't support disposing this object.
                    return false;
                }
            }

            private VisualTarget _visualTarget;

        }
    }
}
