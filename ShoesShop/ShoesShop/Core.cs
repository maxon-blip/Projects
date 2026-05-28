using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoesShop
{
    public class Core
    {
        public static Entities DB = CreateSeededDatabase();

        public static void ResetDatabase()
        {
            DB = CreateSeededDatabase();
        }

        public static Entities CreateSeededDatabase()
        {
            var db = new Entities();
            db.Categories = new List<Categories>
            {
                new Categories { CategoryID = 1, Category = @"Женская обувь" },
                new Categories { CategoryID = 2, Category = @"Мужская обувь" },
            };

            db.Suppliers = new List<Suppliers>
            {
                new Suppliers { SupplierID = 1, Supplier = @"Kari" },
                new Suppliers { SupplierID = 2, Supplier = @"Обувь для вас" },
            };

            db.Producers = new List<Producers>
            {
                new Producers { ProducerID = 1, Producer = @"Kari" },
                new Producers { ProducerID = 2, Producer = @"Marco Tozzi" },
                new Producers { ProducerID = 3, Producer = @"Рос" },
                new Producers { ProducerID = 4, Producer = @"Rieker" },
                new Producers { ProducerID = 5, Producer = @"Alessio Nesca" },
                new Producers { ProducerID = 6, Producer = @"CROSBY" },
            };

            db.ProductType = new List<ProductType>
            {
                new ProductType { ProductTypeID = 1, ProductType1 = @"Ботинки" },
                new ProductType { ProductTypeID = 2, ProductType1 = @"Туфли" },
                new ProductType { ProductTypeID = 3, ProductType1 = @"Кроссовки" },
                new ProductType { ProductTypeID = 4, ProductType1 = @"Полуботинки" },
                new ProductType { ProductTypeID = 5, ProductType1 = @"Кеды" },
                new ProductType { ProductTypeID = 6, ProductType1 = @"Тапочки" },
                new ProductType { ProductTypeID = 7, ProductType1 = @"Сапоги" },
            };

            db.UsersRole = new List<UsersRole>
            {
                new UsersRole { UserRoleID = 1, UserRole = "Администратор" },
                new UsersRole { UserRoleID = 2, UserRole = "Клиент" }
            };

            db.Users = new List<Users>
            {
                new Users { UserID = 1, UserRoleID = 1, Login = "admin", Password = "admin", Name = "Администратор" },
                new Users { UserID = 2, UserRoleID = 2, Login = "user", Password = "user", Name = "Пользователь" }
            };
            db.Users[0].UsersRole = db.UsersRole[0];
            db.Users[1].UsersRole = db.UsersRole[1];

            db.Status = new List<Status> { new Status { StatusID = 1, Status1 = "Новый" } };
            db.PickPoint = new List<PickPoint> { new PickPoint { PickPointID = 1, Address = "Пункт выдачи" } };
            db.Orders = new List<Orders>();
            db.OrdersProducts = new List<OrdersProducts>();

            db.Products = new List<Products>
            {
                new Products { ProductID = 1, Article = @"А112Т4", ProductTypeID = 1, Unit = @"шт.", Price = 4990, SupplierID = 1, ProducerID = 1, CategoryID = 1, Discount = 3, QuantityInStock = 6, Description = @"Женские Ботинки демисезонные kari", Photo = @"1.jpg" },
                new Products { ProductID = 2, Article = @"F635R4", ProductTypeID = 1, Unit = @"шт.", Price = 3244, SupplierID = 2, ProducerID = 2, CategoryID = 1, Discount = 2, QuantityInStock = 13, Description = @"Ботинки Marco Tozzi женские демисезонные, размер 39, цвет бежевый", Photo = @"2.jpg" },
                new Products { ProductID = 3, Article = @"H782T5", ProductTypeID = 2, Unit = @"шт.", Price = 4499, SupplierID = 1, ProducerID = 1, CategoryID = 2, Discount = 4, QuantityInStock = 5, Description = @"Туфли kari мужские классика MYZ21AW-450A, размер 43, цвет: черный", Photo = @"3.jpg" },
                new Products { ProductID = 4, Article = @"G783F5", ProductTypeID = 1, Unit = @"шт.", Price = 5900, SupplierID = 1, ProducerID = 3, CategoryID = 2, Discount = 2, QuantityInStock = 8, Description = @"Мужские ботинки Рос-Обувь кожаные с натуральным мехом", Photo = @"4.jpg" },
                new Products { ProductID = 5, Article = @"J384T6", ProductTypeID = 1, Unit = @"шт.", Price = 3800, SupplierID = 2, ProducerID = 4, CategoryID = 2, Discount = 2, QuantityInStock = 16, Description = @"B3430/14 Полуботинки мужские Rieker", Photo = @"5.jpg" },
                new Products { ProductID = 6, Article = @"D572U8", ProductTypeID = 3, Unit = @"шт.", Price = 4100, SupplierID = 2, ProducerID = 3, CategoryID = 2, Discount = 3, QuantityInStock = 6, Description = @"129615-4 Кроссовки мужские", Photo = @"6.jpg" },
                new Products { ProductID = 7, Article = @"F572H7", ProductTypeID = 2, Unit = @"шт.", Price = 2700, SupplierID = 1, ProducerID = 2, CategoryID = 1, Discount = 2, QuantityInStock = 14, Description = @"Туфли Marco Tozzi женские летние, размер 39, цвет черный", Photo = @"7.jpg" },
                new Products { ProductID = 8, Article = @"D329H3", ProductTypeID = 4, Unit = @"шт.", Price = 1890, SupplierID = 2, ProducerID = 5, CategoryID = 1, Discount = 4, QuantityInStock = 4, Description = @"Полуботинки Alessio Nesca женские 3-30797-47, размер 37, цвет: бордовый", Photo = @"8.jpg" },
                new Products { ProductID = 9, Article = @"B320R5", ProductTypeID = 2, Unit = @"шт.", Price = 4300, SupplierID = 1, ProducerID = 4, CategoryID = 1, Discount = 2, QuantityInStock = 6, Description = @"Туфли Rieker женские демисезонные, размер 41, цвет коричневый", Photo = @"9.jpg" },
                new Products { ProductID = 10, Article = @"G432E4", ProductTypeID = 2, Unit = @"шт.", Price = 2800, SupplierID = 1, ProducerID = 1, CategoryID = 1, Discount = 3, QuantityInStock = 15, Description = @"Туфли kari женские TR-YR-413017, размер 37, цвет: черный", Photo = @"10.jpg" },
                new Products { ProductID = 11, Article = @"S213E3", ProductTypeID = 4, Unit = @"шт.", Price = 2156, SupplierID = 2, ProducerID = 6, CategoryID = 2, Discount = 3, QuantityInStock = 6, Description = @"407700/01-01 Полуботинки мужские CROSBY", Photo = @"1.jpg" },
                new Products { ProductID = 12, Article = @"E482R4", ProductTypeID = 4, Unit = @"шт.", Price = 1800, SupplierID = 1, ProducerID = 1, CategoryID = 1, Discount = 2, QuantityInStock = 14, Description = @"Полуботинки kari женские MYZ20S-149, размер 41, цвет: черный", Photo = @"2.jpg" },
                new Products { ProductID = 13, Article = @"S634B5", ProductTypeID = 5, Unit = @"шт.", Price = 5500, SupplierID = 2, ProducerID = 6, CategoryID = 2, Discount = 3, QuantityInStock = 0, Description = @"Кеды Caprice мужские демисезонные, размер 42, цвет черный", Photo = @"3.jpg" },
                new Products { ProductID = 14, Article = @"K345R4", ProductTypeID = 4, Unit = @"шт.", Price = 2100, SupplierID = 2, ProducerID = 6, CategoryID = 2, Discount = 2, QuantityInStock = 3, Description = @"407700/01-02 Полуботинки мужские CROSBY", Photo = @"4.jpg" },
                new Products { ProductID = 15, Article = @"O754F4", ProductTypeID = 2, Unit = @"шт.", Price = 5400, SupplierID = 2, ProducerID = 4, CategoryID = 1, Discount = 4, QuantityInStock = 18, Description = @"Туфли женские демисезонные Rieker артикул 55073-68/37", Photo = @"5.jpg" },
                new Products { ProductID = 16, Article = @"G531F4", ProductTypeID = 1, Unit = @"шт.", Price = 6600, SupplierID = 1, ProducerID = 1, CategoryID = 1, Discount = 12, QuantityInStock = 9, Description = @"Ботинки женские зимние ROMER арт. 893167-01 Черный", Photo = @"6.jpg" },
                new Products { ProductID = 17, Article = @"J542F5", ProductTypeID = 6, Unit = @"шт.", Price = 500, SupplierID = 1, ProducerID = 1, CategoryID = 2, Discount = 13, QuantityInStock = 0, Description = @"Тапочки мужские Арт.70701-55-67син р.41", Photo = @"7.jpg" },
                new Products { ProductID = 18, Article = @"B431R5", ProductTypeID = 1, Unit = @"шт.", Price = 2700, SupplierID = 2, ProducerID = 4, CategoryID = 2, Discount = 2, QuantityInStock = 5, Description = @"Мужские кожаные ботинки/мужские ботинки", Photo = @"8.jpg" },
                new Products { ProductID = 19, Article = @"P764G4", ProductTypeID = 2, Unit = @"шт.", Price = 6800, SupplierID = 1, ProducerID = 6, CategoryID = 1, Discount = 15, QuantityInStock = 15, Description = @"Туфли женские, ARGO, размер 38", Photo = @"9.jpg" },
                new Products { ProductID = 20, Article = @"C436G5", ProductTypeID = 1, Unit = @"шт.", Price = 10200, SupplierID = 1, ProducerID = 5, CategoryID = 1, Discount = 15, QuantityInStock = 9, Description = @"Ботинки женские, ARGO, размер 40", Photo = @"10.jpg" },
                new Products { ProductID = 21, Article = @"F427R5", ProductTypeID = 1, Unit = @"шт.", Price = 11800, SupplierID = 2, ProducerID = 4, CategoryID = 1, Discount = 15, QuantityInStock = 11, Description = @"Ботинки на молнии с декоративной пряжкой FRAU", Photo = @"1.jpg" },
                new Products { ProductID = 22, Article = @"N457T5", ProductTypeID = 4, Unit = @"шт.", Price = 4600, SupplierID = 1, ProducerID = 6, CategoryID = 1, Discount = 3, QuantityInStock = 13, Description = @"Полуботинки Ботинки черные зимние, мех", Photo = @"2.jpg" },
                new Products { ProductID = 23, Article = @"D364R4", ProductTypeID = 2, Unit = @"шт.", Price = 12400, SupplierID = 1, ProducerID = 1, CategoryID = 1, Discount = 16, QuantityInStock = 5, Description = @"Туфли Luiza Belly женские Kate-lazo черные из натуральной замши", Photo = @"3.jpg" },
                new Products { ProductID = 24, Article = @"S326R5", ProductTypeID = 6, Unit = @"шт.", Price = 9900, SupplierID = 2, ProducerID = 6, CategoryID = 2, Discount = 17, QuantityInStock = 15, Description = @"Мужские кожаные тапочки ""Профиль С.Дали"" ", Photo = @"4.jpg" },
                new Products { ProductID = 25, Article = @"L754R4", ProductTypeID = 4, Unit = @"шт.", Price = 1700, SupplierID = 1, ProducerID = 1, CategoryID = 1, Discount = 2, QuantityInStock = 7, Description = @"Полуботинки kari женские WB2020SS-26, размер 38, цвет: черный", Photo = @"5.jpg" },
                new Products { ProductID = 26, Article = @"M542T5", ProductTypeID = 3, Unit = @"шт.", Price = 2800, SupplierID = 2, ProducerID = 4, CategoryID = 2, Discount = 18, QuantityInStock = 3, Description = @"Кроссовки мужские TOFA", Photo = @"6.jpg" },
                new Products { ProductID = 27, Article = @"D268G5", ProductTypeID = 2, Unit = @"шт.", Price = 4399, SupplierID = 2, ProducerID = 4, CategoryID = 1, Discount = 3, QuantityInStock = 12, Description = @"Туфли Rieker женские демисезонные, размер 36, цвет коричневый", Photo = @"7.jpg" },
                new Products { ProductID = 28, Article = @"T324F5", ProductTypeID = 7, Unit = @"шт.", Price = 4699, SupplierID = 1, ProducerID = 6, CategoryID = 1, Discount = 2, QuantityInStock = 5, Description = @"Сапоги замша Цвет: синий", Photo = @"8.jpg" },
                new Products { ProductID = 29, Article = @"K358H6", ProductTypeID = 6, Unit = @"шт.", Price = 599, SupplierID = 1, ProducerID = 4, CategoryID = 2, Discount = 20, QuantityInStock = 2, Description = @"Тапочки мужские син р.41", Photo = @"9.jpg" },
                new Products { ProductID = 30, Article = @"H535R5", ProductTypeID = 1, Unit = @"шт.", Price = 2300, SupplierID = 2, ProducerID = 4, CategoryID = 1, Discount = 2, QuantityInStock = 7, Description = @"Женские Ботинки демисезонные", Photo = @"10.jpg" },
            };

            foreach (var product in db.Products)
            {
                product.Categories = db.Categories.FirstOrDefault(x => x.CategoryID == product.CategoryID);
                product.Suppliers = db.Suppliers.FirstOrDefault(x => x.SupplierID == product.SupplierID);
                product.Producers = db.Producers.FirstOrDefault(x => x.ProducerID == product.ProducerID);
                product.ProductType = db.ProductType.FirstOrDefault(x => x.ProductTypeID == product.ProductTypeID);
            }

            return db;
        }
    }
}
