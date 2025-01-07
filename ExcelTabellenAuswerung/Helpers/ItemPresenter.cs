﻿using System.ComponentModel;


namespace ExcelTabellenAuswerung.Helpers
{
    public class ItemPresenter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string item;

        public string Item
        {
            get { return item; }
            set
            {
                item = value;
                this.RaisePropertyChanged("Item");
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }
    }
}
