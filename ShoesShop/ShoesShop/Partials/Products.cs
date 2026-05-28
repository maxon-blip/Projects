using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoesShop
{
    public partial class Products
    {
        public string SpecialBackground
        {
            get
            {
                if ((Discount ?? 0) >= 15) return "#E5F7E5";
                if ((QuantityInStock ?? 0) <= 0) return "#FFECEC";
                return "White";
            }
        }

        public double DiscountPrice
        {
            get
            {
                var price = Price ?? 0;
                var discount = Discount ?? 0;
                return discount > 0 ? price * (1 - discount / 100) : price;
            }
        }

        public List<string> DiscountPriceStyler
        {
            get
            {
                if ((Discount ?? 0) > 0)
                    return new List<string> { "Red", "Strikethrough", DiscountPrice.ToString("N2") };

                return new List<string> { "Black", "None", string.Empty };
            }
        }

        public string NewPhoto
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Photo))
                    return "Res/picture.png";

                if (Photo.StartsWith("Res/", StringComparison.OrdinalIgnoreCase) ||
                    Photo.StartsWith("/", StringComparison.OrdinalIgnoreCase) ||
                    Photo.Contains(":"))
                    return Photo;

                return "Res/" + Photo;
            }
        }
    }
}
