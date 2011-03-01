using System;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Xceed.Wpf.DataGrid;
using System.Windows.Input;

namespace GLTWarter
{
    public static class Utils
    {
        static Dictionary<WeakReference, EventHandler> dictContainerFromItemInListHandlers = new Dictionary<WeakReference, EventHandler>();

        static void ReplaceContainerFromItemInListHandlers(ItemContainerGenerator generator, EventHandler handler)
        {
            Dictionary<WeakReference, EventHandler> dictNewList = new Dictionary<WeakReference, EventHandler>();
            foreach (WeakReference wr in dictContainerFromItemInListHandlers.Keys)
            {
                if (wr.IsAlive)
                {
                    if (wr.Target == generator)
                    {
                        generator.StatusChanged -= dictContainerFromItemInListHandlers[wr];
                    }
                    else
                    {
                        dictNewList[wr] = dictContainerFromItemInListHandlers[wr];
                    }
                }
            }
            dictNewList[new WeakReference(generator)] = handler;
            dictContainerFromItemInListHandlers = dictNewList;
        }

        static public DependencyObject ContainerFromItemInList(ItemsControl list, Object target)
        {
            return ContainerFromItemInList(list, target, null);
        }

        static public DependencyObject ContainerFromItemInList(ItemsControl list, Object target, Delegate callback)
        {
            DependencyObject found = list.ItemContainerGenerator.ContainerFromItem(target);
            if (found != null)
            {
                if (callback != null) callback.Method.Invoke(callback.Target, new object[] { found });
                return found;
            }

            if (list is TreeViewItem) { ((TreeViewItem)list).IsExpanded = true; }

            if (list.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.NotStarted ||
                list.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.GeneratingContainers)
            {
                EventHandler handler = null;
                handler = (EventHandler)delegate
                {
                    if (list.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                    {
                        list.ItemContainerGenerator.StatusChanged -= handler;
                        list.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render,
                            (Action)delegate()
                            {
                                ContainerFromItemInList(list, target, callback);
                            }
                        );
                    }
                    else if (list.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.Error)
                    {
                        list.ItemContainerGenerator.StatusChanged -= handler;
                    }
                };

                ReplaceContainerFromItemInListHandlers(list.ItemContainerGenerator, handler);
                list.ItemContainerGenerator.StatusChanged += handler;
            }
            else
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    found = list.ItemContainerGenerator.ContainerFromIndex(i);
                    if (found != null && found is ItemsControl)
                    {
                        found = Utils.ContainerFromItemInList((ItemsControl)found, target, callback);
                        if (found != null) return found;
                    }
                }
            }
            return null;
        }

        static public childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem) return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null) return childOfChild;
                }
            }
            return null;
        }


        static public IEnumerable<childItem> FindVisualChildren<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    yield return (childItem)child;
                }
                else
                {
                    IEnumerable<childItem> childOfChild = FindVisualChildren<childItem>(child);
                }
            }
        }
#if false
        // Not Being Used

        static public childItem[] FindVisualChildren<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            List<childItem> ret = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    if (ret == null) ret = new List<childItem>();
                    ret.Add((childItem)child);
                }
            }
            if (ret != null) return ret.ToArray();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                childItem[] childOfChild = FindVisualChildren<childItem>(child);
                if (childOfChild.Length > 0) return childOfChild; 
            }
            return new childItem[0]{};
        }
#endif

        static public parentItem FindVisualParent<parentItem>(DependencyObject obj) where parentItem : DependencyObject
        {
            for (DependencyObject parent = VisualTreeHelper.GetParent(obj); parent != null; parent = VisualTreeHelper.GetParent(parent))
            {
                if (parent is parentItem) return (parentItem) parent;
            }
            return null;
        }


        /// <summary>
        /// Locate the first real focusable child.  We keep going down
        /// the visual tree until we hit a leaf node.
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        static public IInputElement GetLeafFocusableChild(IInputElement fe)
        {
            IInputElement ie = GetFirstFocusableChild(fe), final = ie;
            while (final != null)
            {
                ie = final;
                final = GetFirstFocusableChild(final);
            }

            return ie;
        }

        /// <summary>
        /// This searches the Visual Tree looking for a valid child which can have focus.
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        static IInputElement GetFirstFocusableChild(IInputElement fe)
        {
            var dpo = fe as DependencyObject;
            return dpo == null ? null : (from vc in EnumerateVisualTree(dpo,
                                             c => !FocusManager.GetIsFocusScope(c) &&
                                                 (!(c is IInputElement) || (((IInputElement)c).IsEnabled)) &&
                                                 (!(c is FrameworkElement) || (((FrameworkElement)c).Visibility == Visibility.Visible))
                                             )
                                         let ci = vc as IInputElement
                                         let cc = vc as Control
                                         where ci != null && ci.Focusable
                                         select ci).FirstOrDefault();
        }


        /// <summary>
        /// A simple iterator method to expose the visual tree to LINQ
        /// </summary>
        /// <param name="start"></param>
        /// <param name="eval"></param>
        /// <returns></returns>
        static IEnumerable<T> EnumerateVisualTree<T>(T start, Predicate<T> eval) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(start); i++)
            {
                var child = VisualTreeHelper.GetChild(start, i) as T;
                if (child != null && (eval != null && eval(child)))
                {
                    yield return child;
                    foreach (var childOfChild in EnumerateVisualTree(child, eval))
                        yield return childOfChild;
                }
            }
        }

        static public string GetExternalFile(string path)
        {
            string newPath;
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);

            string[] locals = new string[] { "-private", "-" + "", "" };//App.Active.Rpc.Company, string.Empty };

            foreach (string local in locals)
            {
                newPath = Path.Combine(Path.GetDirectoryName(asm.Location), filename + local + extension);
                if (File.Exists(newPath)) return newPath;
                newPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), asm.GetName().Name), filename + local + extension);
                if (File.Exists(newPath)) return newPath;
            }
            return null;
        }

        static public void PlaySound(Stream stream)
        {
            try
            {
                Thread playingThread = new Thread(new ThreadStart(delegate()
                {
                    using (System.Media.SoundPlayer player = new System.Media.SoundPlayer())
                    {
                        player.Stream = stream;
                        player.Load();
                        player.PlaySync();
                    }
                }
                ));
                playingThread.IsBackground = true;
                playingThread.Start();
            }
            catch (Exception) { }
        }

        static public void PlaySound(string file)
        {
            try
            {
                Thread playingThread = new Thread(new ThreadStart(delegate()
                {
                    using (System.Media.SoundPlayer player = new System.Media.SoundPlayer())
                    {
                        player.SoundLocation = file;
                        player.Load();
                        player.Play();
                    }
                }
                ));
                playingThread.IsBackground = true;
                playingThread.Start();
            }
            catch (Exception) { }
        }

        //static public void MoveHightlightToNext(this DataGridControl listResult)
        //{
        //    if (listResult.SelectedItems.Count == 1 && listResult.Items.Count > listResult.SelectedIndex + 1)
        //    {
        //        object itemNext = listResult.Items[listResult.SelectedIndex + 1];
        //        listResult.BringItemIntoView(itemNext);
        //        listResult.SelectedItem = itemNext;
        //        listResult.CurrentItem = itemNext;
        //    }
        //}

        static public void MoveHightlightToNext(this ListView listResult)
        {
            if (listResult.SelectedItems.Count == 1 && listResult.Items.Count > listResult.SelectedIndex + 1)
            {
                object itemNext = listResult.Items[listResult.SelectedIndex + 1];
                listResult.SelectedIndex++;
                listResult.ScrollIntoView(itemNext);
                listResult.SelectedItem = itemNext;
                (listResult.ItemContainerGenerator.ContainerFromItem(itemNext) as ListViewItem).Focus();
            }
        }

        static public void CopyToClipboard(string text)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    return;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(20);
                }
            }
        }
    }

    public class GridViewSorter
    {
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public void ListViewHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListView view = e.Source as ListView;
            ListSortDirection direction;

            if (headerClicked != null && headerClicked.Column != null)
            {
                if (headerClicked != _lastHeaderClicked)
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    if (_lastDirection == ListSortDirection.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;

                string header = null;
                if (headerClicked.Tag != null)
                {
                    header = headerClicked.Tag.ToString();
                    Sort(view, header, direction);
                }
                else
                {
                    if (headerClicked.Column.DisplayMemberBinding is Binding)
                    {
                        header = (headerClicked.Column.DisplayMemberBinding as Binding).XPath ?? (headerClicked.Column.DisplayMemberBinding as Binding).Path.Path;
                    }
                    Sort(view, header, direction);
                }
                return;
            }
        }


        public void Sort(ListView view, string sortBy, ListSortDirection direction)
        {
            using (view.Items.DeferRefresh())
            {
                view.Items.SortDescriptions.Clear();
                AddSort(view, sortBy, direction);
            }
        }

        public void AddSort(ListView view, string sortBy, ListSortDirection direction)
        {        
            if (!string.IsNullOrEmpty(sortBy) && view != null && view.ItemsSource != null)
            {
                System.Type type;
                if (view.ItemsSource.GetType() == typeof(ListCollectionView))
                {
                    type = ((ListCollectionView)view.ItemsSource).SourceCollection.GetType();
                }
                else
                {
                    type = view.ItemsSource.GetType();
                }
                if (type.GetElementType() != null || type.IsGenericType)
                {
                    type = type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType();
                }

                string drill = sortBy;
                System.Type drillType = type;
                if (drill.IndexOf(".") >= 0)
                {
                    string parent = drill.Substring(0, drill.IndexOf("."));
                    drill = drill.Substring(parent.Length + 1);

                    drillType = drillType.GetProperty(parent).PropertyType;
                    if (drillType.IsGenericType)
                    {
                        drillType = drillType.GetGenericArguments()[0];
                    }
                }
                if (drillType.GetProperty(drill) != null)
                {
                    System.Type propertyType = drillType.GetProperty(drill).PropertyType;
                    if (propertyType.IsGenericType)
                    {
                        if (!typeof(IEnumerable<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                        }
                    }
                    if (propertyType.GetInterface("IComparable") == null)
                    {
                        drill += "Comparer";
                        if (drillType.GetProperty(drill) != null &&
                            drillType.GetProperty(drill).PropertyType.GetInterface("IComparable") != null)
                        {
                            sortBy += "Comparer";
                            ExecuteSort(view, sortBy, direction);
                        }
                    }
                    else
                    {
                        ExecuteSort(view, sortBy, direction);
                    }
                }
            }
            else if (sortBy == string.Empty)
            {
                ExecuteSort(view, sortBy, direction);
            }
        }

        private void ExecuteSort(ListView view, string sortBy, ListSortDirection direction)
        {
            SortDescription sd = new SortDescription(sortBy, direction);
            view.Items.SortDescriptions.Add(sd);
        }
    }

    public class PerfCount
    {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long perfcount);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long freq);

        public static long QueryPerformanceCounter()
        {
            long perfcount;
            QueryPerformanceCounter(out perfcount);
            return perfcount;
        }

        public static long QueryPerformanceFrequency()
        {
            long freq;
            QueryPerformanceFrequency(out freq);
            return freq;
        }
    }
}