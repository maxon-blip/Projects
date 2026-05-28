using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShoesShop
{
    public partial class ProductWindow : Window, INotifyPropertyChanged
    {
        private List<Products> _allProducts;
        private List<Products> _filteredProducts;
        private readonly ObservableCollection<CartItem> _cartItems = new ObservableCollection<CartItem>();

        public string UserInfo { get; set; }
        public bool CanUseCart { get; set; }

        public List<Products> FilteredProducts
        {
            get { return _filteredProducts; }
            set
            {
                _filteredProducts = value;
                OnPropertyChanged("FilteredProducts");
                OnPropertyChanged("ProductsCounter");
            }
        }

        public string ProductsCounter
        {
            get
            {
                var total = _allProducts == null ? 0 : _allProducts.Count;
                var current = FilteredProducts == null ? 0 : FilteredProducts.Count;
                return "Показано товаров: " + current + " из " + total;
            }
        }

        public ProductWindow()
        {
            InitializeComponent();
            CurrentSession.CurrentUser = null;
            UserInfo = "Гость";
            CanUseCart = false;
            LoadData();
        }

        public ProductWindow(Users user)
        {
            InitializeComponent();
            CurrentSession.CurrentUser = user;
            CanUseCart = user != null;
            UserInfo = "Пользователь: " + user.Login + " | Имя: " + user.Name + " | Роль: " +
                       (user.UsersRole == null ? "не указана" : user.UsersRole.UserRole);
            LoadData();
        }

        private void LoadData()
        {
            DataContext = this;
            if (Core.DB == null || Core.DB.Products == null)
                Core.ResetDatabase();

            _allProducts = Core.DB.Products == null ? new List<Products>() : Core.DB.Products.ToList();
            FilteredProducts = _allProducts.ToList();
            FillFilters();
            UpdateCartButton();
        }

        
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CanUseCart || CurrentSession.CurrentUser == null)
            {
                MessageBox.Show("Гость может только просматривать каталог. Чтобы удалить товар, войдите в аккаунт.", "Каталог", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var button = sender as Button;
            var product = button == null ? null : button.Tag as Products;

            if (product == null)
                return;

            var result = MessageBox.Show(
                "Удалить товар из каталога?",
                "Удаление",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            Core.DB.Products.Remove(product);
            Core.DB.SaveChanges();

            LoadData();
        }

        private void FillFilters()
        {
            CategoryBox.Items.Clear();
            CategoryBox.Items.Add("Все категории");
            foreach (var category in Core.DB.Categories.OrderBy(c => c.Category).ToList())
                CategoryBox.Items.Add(category.Category);
            CategoryBox.SelectedIndex = 0;

            BrandBox.Items.Clear();
            BrandBox.Items.Add("Все бренды");
            foreach (var producer in Core.DB.Producers.OrderBy(p => p.Producer).ToList())
                BrandBox.Items.Add(producer.Producer);
            BrandBox.SelectedIndex = 0;

            SortBox.Items.Clear();
            SortBox.Items.Add("Без сортировки");
            SortBox.Items.Add("Цена по возрастанию");
            SortBox.Items.Add("Цена по убыванию");
            SortBox.Items.Add("Скидка по убыванию");
            SortBox.SelectedIndex = 0;
        }

        private void ApplyFilters()
        {
            if (_allProducts == null)
                return;

            IEnumerable<Products> products = _allProducts;
            var searchText = SearchBox.Text == null ? string.Empty : SearchBox.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                products = products.Where(p =>
                    (p.Description != null && p.Description.ToLower().Contains(searchText)) ||
                    (p.Article != null && p.Article.ToLower().Contains(searchText)) ||
                    (p.Producers != null && p.Producers.Producer != null && p.Producers.Producer.ToLower().Contains(searchText)) ||
                    (p.Categories != null && p.Categories.Category != null && p.Categories.Category.ToLower().Contains(searchText)));
            }

            var category = CategoryBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(category) && category != "Все категории")
                products = products.Where(p => p.Categories != null && p.Categories.Category == category);

            var brand = BrandBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(brand) && brand != "Все бренды")
                products = products.Where(p => p.Producers != null && p.Producers.Producer == brand);

            var sort = SortBox.SelectedItem as string;
            if (sort == "Цена по возрастанию")
                products = products.OrderBy(p => Convert.ToDouble(p.DiscountPrice));
            else if (sort == "Цена по убыванию")
                products = products.OrderByDescending(p => Convert.ToDouble(p.DiscountPrice));
            else if (sort == "Скидка по убыванию")
                products = products.OrderByDescending(p => p.Discount ?? 0);

            FilteredProducts = products.ToList();
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CanUseCart || CurrentSession.CurrentUser == null)
            {
                MessageBox.Show("Гость может только просматривать каталог. Чтобы добавить товар в корзину, войдите в аккаунт.", "Каталог", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var button = sender as Button;
            var product = button == null ? null : button.Tag as Products;
            if (product == null)
                return;

            if (product.QuantityInStock.HasValue && product.QuantityInStock.Value <= 0)
            {
                MessageBox.Show("Товара нет на складе.", "Каталог", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cartItem = _cartItems.FirstOrDefault(x => x.Product.ProductID == product.ProductID);
            if (cartItem == null)
            {
                cartItem = new CartItem { Product = product, Quantity = 1 };
                _cartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            UpdateCartButton();
            MessageBox.Show("Товар добавлен в корзину.", "Каталог", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CanUseCart || CurrentSession.CurrentUser == null)
            {
                MessageBox.Show("Корзина доступна только авторизованным пользователям.", "Каталог", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var cartWindow = new CartWindow(_cartItems);
            cartWindow.Owner = this;
            cartWindow.ShowDialog();
            UpdateCartButton();
            _allProducts = Core.DB.Products == null ? new List<Products>() : Core.DB.Products.ToList();
            ApplyFilters();
        }

        private void UpdateCartButton()
        {
            if (CartButton != null)
            {
                CartButton.IsEnabled = CanUseCart;
                CartButton.Content = CanUseCart ? "Корзина (" + _cartItems.Sum(x => x.Quantity) + ")" : "Корзина недоступна";
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CategoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BrandBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CanUseCart || CurrentSession.CurrentUser == null)
            {
                MessageBox.Show("Добавлять товары может только авторизованный пользователь.", "Каталог", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addWindow = new AddProductWindow();
            addWindow.Owner = this;
            if (addWindow.ShowDialog() == true)
            {
                _allProducts = Core.DB.Products == null ? new List<Products>() : Core.DB.Products.ToList();
                FillFilters();
                ApplyFilters();
            }
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;
            new MainWindow().Show();
            Close();
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
