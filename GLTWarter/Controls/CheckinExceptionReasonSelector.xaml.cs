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
    /// CheckinExceptionReasonSelector.xaml 的交互逻辑
    /// 将需要绑定的数据(EventLog) 传入DataContext即可
    /// </summary>
    public partial class CheckinExceptionReasonSelector : UserControl
    {
        public CheckinExceptionReasonSelector()
        {
            InitializeComponent();
            this.Init();
        }
        
        #region 初始化
        /// <summary>
        /// 初始化动态Item 和事件
        /// </summary>
        private void Init()
        {
            this.btnExceptionReason.ContextMenu.PlacementTarget = this.btnExceptionReason;
            this.InitChangeDate(); //设置客户修改送货时间菜单
            LastUseReasons.RefreshSubItems(null, "");
            this.SetEvent();
        }

        /// <summary>
        /// 初始化item绑定对象
        /// </summary>
        /// <param name="p_ItemInput"></param>
        void initSubItemTag(MenuItem p_ItemInput)
        {
            if ((p_ItemInput.Items == null || p_ItemInput.Items.Count <= 0) && p_ItemInput.Tag != null)
            {
                if (p_ItemInput.Tag is LastItemTagStruct) //初始化时,不需初始化最近选中的ITEMS的TAG
                    return;
                string strHeader = p_ItemInput.Header.ToString().Substring(3);
                LastItemTagStruct tagStruct = new LastItemTagStruct();
                tagStruct.EventCode = p_ItemInput.Tag.ToString();
                tagStruct.EventComment = p_ItemInput.Name.Length >= 11 ? ((strHeader == "其他原因" || strHeader == "其他时间") ? string.Empty : strHeader) : string.Empty;
                p_ItemInput.Tag = tagStruct;
                return;
            }
            foreach (MenuItem item in p_ItemInput.Items)
            {
                initSubItemTag(item);
            }
        }

        #endregion

        #region 事件
        private MenuItem lastMenuItem;
        public bool IsRiaseSelectedEvent = true;
        public Galant.DataEntity.EventLog ExceptionEvent = new Galant.DataEntity.EventLog();
        /// <summary>
        /// 选中异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExceptionReason_Checked(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(MenuItem))
            {
                MenuItem item = (MenuItem)sender;
                this.comboReason.SelectedValue = ((LastItemTagStruct)item.Tag).EventCode;
                this.textNote.Text = ((LastItemTagStruct)item.Tag).EventComment;
                this.lastMenuItem = item;
                this.textNote.Focus(); //选中备注栏
                this.textNote.SelectAll();
                this.ExceptionEvent.DigestedEventData["R"] = this.comboReason.SelectedValue == null ? string.Empty : this.comboReason.SelectedValue.ToString();
                this.ExceptionEvent.DigestedEventData["C"] = this.textNote.Text.Trim();
                this.RaiseSelectedEvent(string.Empty);
            }
        }

        void btnExceptionReason_Click(object sender, RoutedEventArgs e)
        {
            this.OpenMenu();
        }

        /// <summary>
        /// 修改完备注记录最近输入的对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textNote_LostFocus(object sender, RoutedEventArgs e)
        {
            string textNoteInput = this.textNote.Text.Trim();
            if (this.lastMenuItem != null)
            {
                LastUseReasons.RefreshSubItems(lastMenuItem, textNoteInput);
            }
            if (this.comboReason.SelectedValue == null && this.DataContext != null && textNoteInput != string.Empty)//填写备注后未选择异常原因则默认选中其它
            {
                this.comboReason.SelectedValue = "OTHER";
            }
        }

        private void textNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.textNote.IsFocused && IsRiaseSelectedEvent)
            {
                string textNoteInput = this.textNote.Text.Trim();
                if (this.comboReason.SelectedValue == null && this.DataContext != null && textNoteInput != string.Empty)//填写备注后未选择异常原因则默认选中其它
                {
                    this.comboReason.SelectedValue = "OTHER";
                }
                this.ExceptionEvent.DigestedEventData["R"] = this.comboReason.SelectedValue == null ? string.Empty : this.comboReason.SelectedValue.ToString();
                this.ExceptionEvent.DigestedEventData["C"] = textNoteInput;
                this.RaiseSelectedEvent(string.Empty);
            }
            IsRiaseSelectedEvent = true;
        }

        private void btnExceptionReason_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        #region 私有方法

        public void OpenMenu()
        {
            if (!this.btnExceptionReason.ContextMenu.IsOpen)
            {
                if (this.btnExceptionReason.ContextMenu.PlacementTarget == null)
                    this.btnExceptionReason.ContextMenu.PlacementTarget = this.btnExceptionReason;
                this.btnExceptionReason.ContextMenu.IsOpen = true;
                ((MenuItem)this.btnExceptionReason.ContextMenu.Items[0]).Focus();
            }
        }

        private void SetEvent()
        {
            foreach (MenuItem item in this.btnExceptionReason.ContextMenu.Items)
            {
                SetMenuItemEvent(item);
            }
            this.btnExceptionReason.Click += new RoutedEventHandler(btnExceptionReason_Click);
            this.btnExceptionReason.ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);
            foreach (MenuItem item in this.btnExceptionReason.ContextMenu.Items)
            {
                this.initSubItemTag(item);
            }
        }

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            LastUseReasons.ReloadLastItems((MenuItem)this.btnExceptionReason.ContextMenu.Items[0]);
            this.SetMenuItemEvent((MenuItem)this.btnExceptionReason.ContextMenu.Items[0]);
        }        

        /// <summary>
        /// 设置ContextMenu的最后一级菜单的选中事件
        /// </summary>
        /// <param name="p_ItemInput"></param>
        private void SetMenuItemEvent(MenuItem p_ItemInput)
        {
            if ((p_ItemInput.Items == null || p_ItemInput.Items.Count <= 0) && p_ItemInput.Visibility == System.Windows.Visibility.Visible)
            {
                p_ItemInput.Click += new RoutedEventHandler(btnExceptionReason_Checked);
                p_ItemInput.Checked += new RoutedEventHandler(btnExceptionReason_Checked);
                return;
            }
            foreach (MenuItem item in p_ItemInput.Items)
            {
                SetMenuItemEvent(item);
            }
        }

        /// <summary>
        /// 设置客户修改送货时间菜单
        /// </summary>
        void InitChangeDate()
        {
            this.menuItem211.Header = "_1 " + DateTime.Now.AddDays(1).ToString("M\\/d(ddd)");
            this.menuItem212.Header = "_2 " + DateTime.Now.AddDays(2).ToString("M\\/d(ddd)");
            this.menuItem213.Header = "_3 " + DateTime.Now.AddDays(3).ToString("M\\/d(ddd)");
            this.menuItem214.Header = "_4 " + DateTime.Now.AddDays(4).ToString("M\\/d(ddd)");
            this.menuItem215.Header = "_5 " + DateTime.Now.AddDays(5).ToString("M\\/d(ddd)");
            this.menuItem216.Header = "_6 " + DateTime.Now.AddDays(6).ToString("M\\/d(ddd)");
            this.menuItem217.Header = "_7 " + DateTime.Now.AddDays(7).ToString("M\\/d(ddd)");
        }

        #endregion

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(ExceptionReasonSelectedEventHandler), typeof(CheckinExceptionReasonSelector));
        /// <summary>
        /// 异常原因选择/输入完成
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        void RaiseSelectedEvent(string enteredText)
        {
            RoutedEventArgs arg = new RoutedEventArgs(SelectedEvent, this);
            RaiseEvent(arg);
        }
    }

    public delegate void ExceptionReasonSelectedEventHandler(object sender, RoutedEventArgs e);
    /// <summary>
    /// 最近使用过的原因--0 最近使用过的原因 控制类
    /// </summary>
    public class LastUseReasons
    {
        /// <summary>
        /// 最近使用过的10个原因项
        /// </summary>
        static List<MenuItem> subItems = new List<MenuItem>();

        /// <summary>
        /// 刷新最近选中的异常原因
        /// </summary>
        /// <param name="p_itemCurrent">最后选中的Item</param>
        /// <param name="p_Comment">备注</param>
        public static void RefreshSubItems(MenuItem p_itemCurrent, string p_Comment)
        {
            MenuItem newItem = null;
            if (p_itemCurrent != null)
            {
                if (subItems.Count > 0)
                {
                    string itemHeader = GetLastUseItemHeader(p_itemCurrent, p_Comment);
                    MenuItem currentItem = (from item in subItems where item.Header.ToString().Substring(3) == itemHeader select item).FirstOrDefault();
                    if (currentItem != null)
                        subItems.Remove(currentItem);
                }
                newItem = GenerateNewItem(p_itemCurrent, p_Comment);
                subItems.Insert(0, newItem);
                if (subItems.Count > 10)//是否超过
                    subItems.RemoveAt(subItems.Count - 1);
            }           
            RefreshItemHeaderName();
        }

        /// <summary>
        /// 重新加载最近使用的项目
        /// </summary>
        /// <param name="p_ItmeParent"></param>
        /// <returns></returns>
        public static MenuItem ReloadLastItems(MenuItem p_ItmeParent)
        {            
            p_ItmeParent.Items.Clear();
            foreach (MenuItem item in subItems)
            {
                MenuItem itemNew = new MenuItem();
                itemNew.Header = item.Header;
                itemNew.Tag = item.Tag;
                itemNew.Name = item.Name;
                p_ItmeParent.Items.Add(itemNew);
            }
            if (subItems.Count > 0 && p_ItmeParent.Visibility != Visibility.Visible)
                p_ItmeParent.Visibility = Visibility.Visible;
            return p_ItmeParent;
        }

        /// <summary>
        /// 获取最近使用项的显示Header
        /// </summary>
        /// <param name="p_itemCurrent"></param>
        /// <param name="p_Comment"></param>
        /// <returns></returns>
        private static string GetLastUseItemHeader(MenuItem p_itemCurrent, string p_Comment)
        {
            string strCurrentHeader = p_Comment;
            if (p_itemCurrent.Name.Length >= 11)//选中备注项目
            {
                strCurrentHeader = ((MenuItem)p_itemCurrent.Parent).Header.ToString().Substring(3) + "-" + strCurrentHeader;
            }
            else if (p_itemCurrent.Name.StartsWith("menuItem0"))//选择最近选项
            {
                strCurrentHeader = p_itemCurrent.Header.ToString().Substring(3).Split('-')[0] + "-" + strCurrentHeader;
            }
            else//直接选中原因
            {
                strCurrentHeader = p_itemCurrent.Header.ToString().Substring(3) + "-" + strCurrentHeader;
            }
            return strCurrentHeader.TrimEnd('-');
        }

        private static MenuItem GenerateNewItem(MenuItem p_itemCurrent, string p_Comment)
        {
            MenuItem item = new MenuItem();
            item.Header = "_0 " + GetLastUseItemHeader(p_itemCurrent, p_Comment);
            LastItemTagStruct itemTag = new LastItemTagStruct();
            itemTag.EventComment = p_Comment;
            itemTag.EventCode = ((LastItemTagStruct)p_itemCurrent.Tag).EventCode;
            item.Tag = itemTag;
            return item;
        }
        private static void RefreshItemHeaderName()
        {
            int i = 0;
            foreach (MenuItem item in subItems)
            {
                item.Name = "menuItem0" + i.ToString();
                item.Header = "_" + i.ToString() + " " + item.Header.ToString().Substring(3);
                i++;
            }
        }
    }

    public class LastItemTagStruct
    {
        /// <summary>
        /// 事件代码
        /// </summary>
        public string EventCode;
        /// <summary>
        /// 备注
        /// </summary>
        public string EventComment;
    }
}