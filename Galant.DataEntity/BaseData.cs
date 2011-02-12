using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    [KnownType(typeof(Entity))]
    [KnownType(typeof(Package))]
    public class BaseData: INotifyPropertyChanged, IDataErrorInfo,ICloneable
    {

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
            set { }
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
            set { }
        }
        
        public String[] Errors
        {
            get { 
                return errorStrings.Values.ToArray<String>();
            }
            set
            { }
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
