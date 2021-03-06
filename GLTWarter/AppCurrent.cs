﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using Galant.DataEntity;
using GLTWarter.Printings;

namespace GLTWarter
{
    public class AppCurrent : INotifyPropertyChanged
    {
        public readonly static log4net.ILog Logger = log4net.LogManager.GetLogger("MainLogger");

        public MainScreen MainScreen { get; set; }

        public Window MainWindow { get { return Application.Current.MainWindow; } }
        /// <summary>
        /// 当前登陆用户
        /// </summary>
        public Galant.DataEntity.Entity StaffCurrent { get; set; }

        private Galant.DataEntity.AppStatusCach appCach;

        /// <summary>
        /// 当前程序缓存
        /// </summary>
        public Galant.DataEntity.AppStatusCach AppCach {
            get { return appCach; }
            set { appCach = value; OnPropertyChanged("AppCach"); }
        }
        static private AppCurrent active;

        static public AppCurrent Active
        {
            get { return active; }
            set { active = value;}
        }

        public Printing Printing { get; set; }

        static public string ProgramTitle
        {
            get
            {
                return Resource.ProductNameLong;
            }
        }

        public AppCurrent()
        {
            
            Xceed.Wpf.DataGrid.Licenser.LicenseKey = "DGP31-M42XN-22M34-CWJA"; 
            Printing = new Printing();          
            System.Threading.Thread.CurrentThread.CurrentUICulture = DeploymentSettings.Default.Locale;
            System.Threading.Thread.CurrentThread.CurrentCulture = DeploymentSettings.Default.Locale;
            Resource.Culture = DeploymentSettings.Default.Locale;
            StaffCurrent = new Entity();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
