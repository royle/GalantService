﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Galant.DataEntity
{
    public abstract class BaseData: INotifyPropertyChanged, IDataErrorInfo,ICloneable
    {
        public BaseData()
        { 
            
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            ValidatePropertyChanged(propertyName);
            InternalPropertyChanged(propertyName);
        }

        public virtual void ValidatePropertyChanged(String propertyName)
        {
            
        }

        internal void InternalPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler hander = PropertyChanged;
            if (hander != null)
            {
                hander(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion //INotifyPropertyChanged

        #region IErrorInfo
        private Dictionary<String, String> errorStrings = new Dictionary<string, string>();
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                String error;
                if (errorStrings.TryGetValue(columnName, out error))
                {
                    return error;
                }
                return String.Empty;
            }
        }

        public String[] Errors
        {
            get { 
                return errorStrings.Values.ToArray<String>();
            }
        }
        #endregion // IErrorInfo

        #region ICloneable
        virtual public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion //ICloneable
    }
}
