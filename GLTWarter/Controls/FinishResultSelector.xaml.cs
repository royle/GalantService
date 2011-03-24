using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using GLTWarter.Data;

namespace GLTWarter.Controls
{
    /// <summary>
    /// FinishResultSelector.xaml 的交互逻辑
    /// 将需要绑定的数据 传入DataContext即可
    /// </summary>
    public partial class FinishResultSelector : UserControl
    {
        static MenuItem lastCheckedItem;

        public FinishResultSelector()
        {
            InitializeComponent();            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Init();
        }
        #region 事件

        private void Init()
        {
            this.btnContextMenu.ContextMenu = null;
            this.btnContextMenu.PlacementTarget = this.btnFinishResult;
            this.setEvent();
            this.BindLastOperationItem();
        }

        /// <summary>
        /// 选中归班结果,跳转到下一个控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinishResult_Checked(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
                return;
            Galant.DataEntity.Paper context = ((Galant.DataEntity.Paper)this.DataContext);
            if (sender.GetType() == typeof(MenuItem))
            {                
                MenuItem item = (MenuItem)sender;
                lastCheckedItem = item;
                context.NewSubStatus = item.Tag.ToString(); //设置shipment的新归班结果
                this.BindLastOperationItem();
                this.btnFinishResult.Focus();
                RaiseSelectedEvent(string.Empty);
            }
        }

        private void BindLastOperationItem()
        {
            if (lastCheckedItem != null)
            {
                foreach (MenuItem itemInMenu in this.btnContextMenu.Items) //如果最后一次选择的结果,在当前订单中不显示,则不添加到第0项
                {
                    if (itemInMenu.Tag.ToString() == lastCheckedItem.Tag.ToString() && itemInMenu.Visibility != System.Windows.Visibility.Visible)
                        return;
                }
                MenuItem item = new MenuItem();
                if (lastCheckedItem.Header.ToString().StartsWith("_0 上次的选择"))
                {
                    item.Header = "_0 " + lastCheckedItem.Header.ToString().Substring(3);
                }
                else
                {
                    item.Header = "_0 上次的选择：" + lastCheckedItem.Header.ToString().Substring(3);
                }
                item.Tag = lastCheckedItem.Tag;
                item.Click += new RoutedEventHandler(btnFinishResult_Checked);
                item.Checked += new RoutedEventHandler(btnFinishResult_Checked);
                if (((MenuItem)this.btnContextMenu.Items[0]).Header.ToString().StartsWith("_0 上次的选择"))
                {
                    this.btnContextMenu.Items.RemoveAt(0);
                }
                this.btnContextMenu.Items.Insert(0, item);
            }
        }

        void btnFinishResult_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.OpenMenu();
        }
       

        private void btnFinishResult_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        #region 私有方法

        public void OpenMenu()
        {
            if (!this.btnContextMenu.IsOpen)
            {
                this.btnContextMenu.IsOpen = true;
                if (this.btnContextMenu.PlacementTarget == null)
                    this.btnContextMenu.PlacementTarget = this.btnFinishResult;
                ((MenuItem)this.btnContextMenu.Items[0]).Focus();
            }
        }

        private void setEvent()
        {
            foreach (MenuItem item in this.btnContextMenu.Items)
            {
                item.Click += new RoutedEventHandler(btnFinishResult_Checked);
                item.Checked += new RoutedEventHandler(btnFinishResult_Checked);
            }
            this.btnFinishResult.Click += new RoutedEventHandler(btnFinishResult_Click);
        }

        void btnFinishResult_Click(object sender, RoutedEventArgs e)
        {
            this.OpenMenu();
        }

        #endregion       

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(FinishResultSelectedEventHandler), typeof(FinishResultSelector));
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        void RaiseSelectedEvent(string enteredText)
        {
            RoutedEventArgs arg = new RoutedEventArgs(SelectedEvent, this)
            {                
            };
            RaiseEvent(arg);
        }
    }

    public delegate void FinishResultSelectedEventHandler(object sender, RoutedEventArgs e);

   
}