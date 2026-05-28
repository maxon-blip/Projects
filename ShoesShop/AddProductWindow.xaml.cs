using System;
using System.Linq;
using System.Windows;

namespace ShoesShop
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            LoadLists();
        }

        private void LoadLists()
        {
            CategoryBox.ItemsSource = Core.DB.Categories.OrderBy(x => x.Category).ToList();
            TypeBox.ItemsSource = Core.DB.ProductType.OrderBy(x => x.ProductType1).ToList();
            ProducerBox.ItemsSource = Core.DB.Producers.OrderBy(x => x.Producer).ToList();
            SupplierBox.ItemsSource = Core.DB.Suppliers.OrderBy(x => x.Supplier).ToList();

            for (int i = 1; i <= 10; i++)
                PhotoBox.Items.Add(i + ".jpg");
            PhotoBox.Items.Add("picture.png");

            CategoryBox.SelectedIndex = 0;
            TypeBox.SelectedIndex = 0;
            ProducerBox.SelectedIndex = 0;
            SupplierBox.SelectedIndex = 0;
            PhotoBox.SelectedIndex = 0;
            QuantityBox.Text = "1";
            PriceBox.Text = "0";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            double price;
            double quantity;

            if (string.IsNullOrWhiteSpace(ArticleBox.Text) || string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Заполните артикул и описание товара.", "Добавление товара", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(PriceBox.Text.Replace('.', ','), out price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену.", "Добавление товара", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(QuantityBox.Text.Replace('.', ','), out quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество.", "Добавление товара", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var category = CategoryBox.SelectedItem as Categories;
            var type = TypeBox.SelectedItem as ProductType;
            var producer = ProducerBox.SelectedItem as Producers;
            var supplier = SupplierBox.SelectedItem as Suppliers;

            var product = new Products
            {
                ProductID = Core.DB.Products.Count == 0 ? 1 : Core.DB.Products.Max(x => x.ProductID) + 1,
                Article = ArticleBox.Text.Trim(),
                ProductTypeID = type.ProductTypeID,
                ProductType = type,
                Unit = "шт.",
                Price = price,
                SupplierID = supplier.SupplierID,
                Suppliers = supplier,
                ProducerID = producer.ProducerID,
                Producers = producer,
                CategoryID = category.CategoryID,
                Categories = category,
                Discount = 0,
                QuantityInStock = quantity,
                Description = DescriptionBox.Text.Trim(),
                Photo = PhotoBox.SelectedItem == null ? "picture.png" : PhotoBox.SelectedItem.ToString()
            };

            Core.DB.Products.Add(product);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
