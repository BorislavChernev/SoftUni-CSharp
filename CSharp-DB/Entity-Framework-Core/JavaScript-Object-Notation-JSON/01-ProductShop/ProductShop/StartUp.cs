namespace ProductShop;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Net;
using System.Security.Principal;


public class StartUp
{
    public static void Main()
    {
        ProductShopContext context = new ProductShopContext();
        //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");


        string result = GetUsersWithProducts(context);
        Console.WriteLine(result);
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportUserDto[] userDtos =
            JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

        ICollection<User> validUsers = new HashSet<User>();

        foreach (ImportUserDto userDto in userDtos)
        {
            User user = mapper.Map<User>(userDto);

            validUsers.Add(user);
        }

        context.Users.AddRange(validUsers);
        context.SaveChanges();
        return $"Successfully imported {validUsers.Count}";

    }

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportProductDto[] productDtos =
            JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
        Product[] products = mapper.Map<Product[]>(productDtos);

        context.Products.AddRange(products);
        context.SaveChanges();
        return $"Successfully imported {products.Length}";

    }

    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        ImportCategoryDto[] categoryDtos =
            JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);
        Category[] categories = mapper.Map<Category[]>(categoryDtos);

        context.Categories.AddRange(categories);
        context.SaveChanges();
        return $"Successfully imported {categories.Length}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        CategoryProduct[] cpDtos = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

        List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

        foreach (var cp in cpDtos)
        {
            CategoryProduct categoryProduct = new CategoryProduct()
            {
                CategoryId = cp.CategoryId,
                ProductId = cp.ProductId,
            };

            categoryProducts.Add(categoryProduct);
        }

        context.AddRange();

        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count}";
    }
    public static string GetProductsInRange(ProductShopContext context)
    {
        var products = context.Products
            .AsNoTracking()
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Select(p => new
            {
                name = p.Name,
                price = p.Price,
                seller = p.Seller.FirstName + " " + p.Seller.LastName

            })
            .ToArray();

        return JsonConvert.SerializeObject(products, Formatting.Indented);
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        IContractResolver contractResolver = ConfigureCamelCaseNaming();

        var usersWithSoldProducts = context.Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Select(u => new
            {
                u.FirstName,
                u.LastName,
                SoldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                    .ToArray()
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(usersWithSoldProducts,
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
    }
    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        IContractResolver contractResolver = ConfigureCamelCaseNaming();

        var categories = context.Categories
            .AsNoTracking()
            .Select(u => new
            {
                u.Name,
                ProductsCount = u.CategoriesProducts.Count,
                averagePrice = $"{u.CategoriesProducts.Average(c => c.Product.Price):f2}",
                totalRevenue = $"{u.CategoriesProducts.Sum(c => c.Product.Price):f2}"
            })
            .OrderByDescending(c => c.ProductsCount)
            .ToArray();

        return JsonConvert.SerializeObject(
            categories,
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
    }
    public static string GetUsersWithProducts(ProductShopContext context)
    {
        IContractResolver contractResolver = ConfigureCamelCaseNaming();

        var users = context
            .Users
            .AsNoTracking()
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .Select(u => new
            {
                u.FirstName,
                u.LastName,
                u.Age,
                soldProducts = new
                {
                    Count = u.ProductsSold
                        .Count(p => p.Buyer != null),
                    Products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                        .ToArray()
                }
            })
            .OrderByDescending(u => u.soldProducts.Count)
            .ToArray();

        var userWrapperDto = new
        {
            UsersCount = users.Length,
            users = users
        };

        return JsonConvert.SerializeObject(userWrapperDto,
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore,
            });
    }
    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
    private static IContractResolver ConfigureCamelCaseNaming()
    {
        return new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy(false, true)
        };
    }
}