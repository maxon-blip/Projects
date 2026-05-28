using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Linq;
using System.Windows;

namespace ShoesShop
{
    public partial class CartWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<CartItem> CartItems { get; set; }

        public double TotalAmount
        {
            get { return CartItems == null ? 0 : CartItems.Sum(x => x.TotalPrice); }
        }

        public CartWindow(ObservableCollection<CartItem> cartItems)
        {
            InitializeComponent();
            CartItems = cartItems;
            foreach (var item in CartItems)
                item.PropertyChanged += CartItem_PropertyChanged;
            DataContext = this;
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("TotalAmount");
            CartGrid.Items.Refresh();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = CartGrid.SelectedItem as CartItem;
            if (selected == null)
            {
                MessageBox.Show("Выберите товар для удаления.", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CartItems.Remove(selected);
            OnPropertyChanged("TotalAmount");
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CartItems.Clear();
            OnPropertyChanged("TotalAmount");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CartItems.Any())
            {
                MessageBox.Show("Корзина пуста.", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var order = new Orders
                {
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(3),
                    UserID = CurrentSession.CurrentUser == null ? (int?)null : CurrentSession.CurrentUser.UserID,
                    CodeToReceive = new Random().Next(100, 999),
                    StatusID = GetDefaultStatusId(),
                    AddressPointID = GetDefaultPickPointId()
                };

                order.OrderID = Core.DB.Orders.Count == 0 ? 1 : Core.DB.Orders.Max(x => x.OrderID) + 1;
                Core.DB.Orders.Add(order);

                foreach (var item in CartItems)
                {
                    Core.DB.OrdersProducts.Add(new OrdersProducts
                    {
                        OrderID = order.OrderID,
                        ProductID = item.Product.ProductID,
                        Quantity = item.Quantity
                    });

                    if (item.Product.QuantityInStock.HasValue)
                        item.Product.QuantityInStock = Math.Max(0, item.Product.QuantityInStock.Value - item.Quantity);
                }

                CartItems.Clear();
                OnPropertyChanged("TotalAmount");

                MessageBox.Show("Заказ успешно оформлен. Код получения: " + order.CodeToReceive,
                    "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось оформить заказ: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int? GetDefaultStatusId()
        {
            var status = Core.DB.Status.FirstOrDefault();
            return status == null ? (int?)null : status.StatusID;
        }

        private int? GetDefaultPickPointId()
        {
            var point = Core.DB.PickPoint.FirstOrDefault();
            return point == null ? (int?)null : point.PickPointID;
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
