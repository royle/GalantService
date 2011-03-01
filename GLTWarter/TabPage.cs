using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;


namespace GLTWarter
{
    internal class TabPage
    {
        private Frame page;

        public TabPage(string page)
        {
            this.page = new Frame();
            this.page.Style = App.Current.Resources["ContentFrameStyle"] as System.Windows.Style;
            this.page.Source = new Uri(page);

            CommandBinding cb_Navigate = new CommandBinding(NavigationCommands.NavigateJournal);
            cb_Navigate.PreviewCanExecute += new CanExecuteRoutedEventHandler(OnQueryCanNavigate);
            this.page.CommandBindings.Add(cb_Navigate);
        }

        protected virtual void OnQueryCanNavigate(object sender, CanExecuteRoutedEventArgs e)
        {
            // Navigation not allowed
            e.Handled = true;
            e.CanExecute = false;
        }


        public Frame Page
        {
            get { return this.page; }
        }
    }

    internal class TabPages : ObservableCollection<TabPage>
    {
        public TabPages()
            : base()
        {
        }
    }
}
