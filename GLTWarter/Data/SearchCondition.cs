using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GLTWarter.Data
{
   
    public abstract class SearchCondition : INotifyPropertyChanged
    {
        /// <summary>
        /// Returns the Type of the search condition which could be recognized by the backend
        /// </summary>
        
        public abstract string Type { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Validate the search condition. Expecting localized error string or null.
        /// </summary>
        /// <returns>The localized error string to be shown</returns>
        public abstract string Validate();

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public interface ISearchDataWithConditions
    {
        ObservableCollection<SearchCondition> Conditions { get; }
        SearchCondition[] ConditionsInternal { get; set; }
        void RemoveCondition(SearchCondition condition);
    }
}
