using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExcelTabellenAuswerung.Models
{
    public class Items : ObservableObject
    {
        private readonly Type _contentType;
        private object _content;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto;
        private Thickness _marginRequirement = new Thickness(16);
        private string _shownName;
        private string _name;
        private string _json;
        private Dictionary<string, dynamic> _extendedParameterList;

        public void Dispose()
        {
            dynamic obj = Convert.ChangeType(_content, _contentType);

            if (obj != null && obj.DataContext != null)
            {
                obj.DataContext.Dispose();
            }
        }

        public Items(string name, string shownName, Type contentType)
        {
            Name = name;
            ShownName = shownName;
            _contentType = contentType;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ShownName
        {
            get { return _shownName; }
            set { _shownName = value; }
        }

        public object Content
        {
            get
            {
                _content = CreateContent();
                return _content;
            }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get { return _horizontalScrollBarVisibilityRequirement; }
            set { SetProperty(ref _horizontalScrollBarVisibilityRequirement, value); }
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get { return _verticalScrollBarVisibilityRequirement; }
            set { SetProperty(ref _verticalScrollBarVisibilityRequirement, value); }
        }

        public Thickness MarginRequirement
        {
            get { return _marginRequirement; }
            set { SetProperty(ref _marginRequirement, value); }
        }

        public object DataContext()
        {

            if (_content is FrameworkElement)
            {
                FrameworkElement element = _content as FrameworkElement;

                return element.DataContext;
            }

            return null;

        }

        private object CreateContent()
        {
            var content = Activator.CreateInstance(_contentType);
            if (content is FrameworkElement)
            {
                FrameworkElement element = content as FrameworkElement;
            }

            return content;
        }
    }
}
