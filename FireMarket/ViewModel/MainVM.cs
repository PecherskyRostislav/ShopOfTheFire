using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FireMarket.Model;

namespace FireMarket.ViewModel
{
    public class MainVM : BaseViewModel
    {
        private BaseViewModel[] Views { get; set; } = { new ProductsVM(), new ClientsVM(), new ProvidersVM(), new StoragesVM() };
        public string[] ViewsName { get; set; } = { "Товары", "Клиенты", "Поставщики", "Склады" };

        private BaseViewModel _currView;
        public BaseViewModel CurrentView
        {
            get => _currView;
            set
            {
                _currView = value;
                OnPropertyChanged("CurrentView");
            }
        }        
        private string _nameCurrView { get; set; }
        public string NameCurrentView
        {
            get => _nameCurrView;
            set
            {
                _nameCurrView = value;
                switch (value) 
                {
                    case "Товары":
                        Position = 0;
                        break;
                    case "Клиенты":
                        Position = 1;
                        break;
                    case "Поставщики":
                        Position = 2;
                        break;
                    case "Склады":
                        Position = 3;
                        break;
                }                
                OnPropertyChanged("NameCurrentView");
            }
        }

        private int _position { get; set; }
        private int Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                if(_position == 4)
                {
                    _position = 0;
                }
                if(_position == -1)
                {
                    _position = 3;
                }
                    CurrentView = Views[Position];
                    NameCurrentView = ViewsName[Position];
            }
        }

        public ICommand SwapLeft
        {
            get => new RelayCommand((obj) => Position--);
        }
        public ICommand SwapRight
        {
            get => new RelayCommand((obj) => Position++);
        }

        public MainVM()
        {
            _position = default(int);
            _position = 0;
            _currView = Views[_position];
            _nameCurrView = ViewsName[_position];
        }
    }
}
