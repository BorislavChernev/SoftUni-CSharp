using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            string inputXml = File.ReadAllText("../../../Datasets/users.xml");

            ProductShopContext context = new ProductShopContext();

            string result = GetSoldProducts(context);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportUserDto[] importUserDtos =
                xmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

            ICollection<User> users = new HashSet<User>();
            foreach (ImportUserDto u in importUserDtos)
            {
                users.Add(new User()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                });
            }

            context.Users.AddRange(users);

            context.SaveChangesAsync();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportProductDto[] importProductDtos =
                xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            ICollection<Product> products = new List<Product>();
            foreach (ImportProductDto p in importProductDtos)
            {
                products.Add(new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId,
                });
            }

            context.Products.AddRange(products);

            context.SaveChangesAsync();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryDto[] importCategoryDtos =
                xmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            ICollection<Category> categories = new List<Category>();
            foreach (ImportCategoryDto c in importCategoryDtos)
            {
                if (string.IsNullOrEmpty(c.Name)) continue;
                categories.Add(new Category()
                {
                    Name = c.Name
                });
            }

            context.Categories.AddRange(categories);

            context.SaveChangesAsync();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryProductDto[] importCategoryProductDtos =
                xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            foreach (ImportCategoryProductDto cp in importCategoryProductDtos)
            {
                if (!context.Products.Any(p => p.Id == cp.ProductId) || !context.Categories.Any(p => p.Id == cp.CategoryId)) continue;

                categoryProducts.Add(new CategoryProduct()
                {
                    ProductId = cp.ProductId,
                    CategoryId = cp.CategoryId,
                });
            }

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChangesAsync();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ExportProductDto[] products = context.Products
                .AsNoTracking()
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(products, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ExportSoldProductsDto[] SoldProductsDtos = context.Users
                .Where(sp => sp.ProductsSold.Count >= 1)
                .OrderBy(sp => sp.LastName)
                .ThenBy(sp => sp.FirstName)
                .Take(5)
                .ProjectTo<ExportSoldProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportSoldProductsDto[]>(SoldProductsDtos, "Users");
        }
        private static IMapper InitializeAutoMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
    }
}