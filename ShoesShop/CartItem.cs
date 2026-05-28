using System;
using System.ComponentModel;

namespace ShoesShop
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;

        public Products Product { get; set; }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 1) value = 1;
                _quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("TotalPrice");
            }
        }

        public string Name
        {
            get { return Product == null ? string.Empty : Product.Description; }
        }

        public double Price
        {
            get
            {
                if (Product == null) return 0;
                return Convert.ToDouble(Product.DiscountPrice);
            }
        }

        public double TotalPrice
        {
            get { return Price * Quantity; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
