using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.Models
{
    public abstract class MasterModel : INotifyPropertyChanged
    {
        // A property changed event object
        public event PropertyChangedEventHandler PropertyChanged;

        public MasterModel()
        {
            Name = "";
            Index = -1;
        }

        /// <summary>
        /// Notifies the page when a property has changed so the view can be updated
        /// </summary>
        /// <param name="propName">The property name</param>
        protected void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        // The name of the model
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                if(index != value)
                {
                    index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

       override
       public string ToString()
        {
            return Name;
        }
    }
}
