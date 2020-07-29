using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.DTO.Product;
using ProductShop.DTO.User;
using ProductShop.Models;
using Remotion.Linq.Clauses;

namespace ProductShop
{
    public class StartUp
    {
        public const string ResultDirectoryPath = "../../../Datasets/Results";

        public static void Main(string[] args)
        {

            ProductShopContext db = new ProductShopContext();
            //ResetDataBase(db);

            InitializeMapper();

            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //string result = ImportCategoryProducts(db, inputJson);

            //Console.WriteLine(result);



            string json = GetUsersWithProducts(db);

            if (!Directory.Exists(ResultDirectoryPath))
            {
                Directory.CreateDirectory(ResultDirectoryPath);
            }

            File.WriteAllText(ResultDirectoryPath + "/users-and-products.json", json);



        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
                //cfg.CreateMap<>();
            });
        }

        public static void ResetDataBase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("DB deleted");
            db.Database.EnsureCreated();
            Console.WriteLine("DB created");

        }

        //Query 2. Import Users

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            //string inputJson = File.ReadAllText("../../../Datasets/users.json");

            //string result = ImportUsers (db, inputJson);

            //Console.WriteLine(result);

            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        //Query 3. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {

            //string inputJson = File.ReadAllText("../../../Datasets/products.json");

            //string result = ImportProducts (db, inputJson);

            //Console.WriteLine(result);

            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Query 4. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");

            //string result = ImportCategories (db, inputJson);

            //Console.WriteLine(result);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson, settings)
                    .Where(e => e.Name != null)
                    .ToArray();


            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        //Query 5. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {

            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //string result = ImportCategoryProducts (db, inputJson);

            //Console.WriteLine(result);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            CategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson, settings);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        //Query 6. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {

            //string json = GetProductsInRange(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/products-in-range.json", json);

            var products = context
                .Products
                .Where(pr => pr.Price >= 500 && pr.Price <= 1000)
                .OrderBy(pr => pr.Price)
                .ProjectTo<ListProductsInRangeDTO>()
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        //Query 7. Export Successfully Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {


            //string json = GetSoldProducts(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/users-sold-products.json", json);

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName

                    })
                    .ToArray()
                })
                //.ProjectTo<ListUsersInRangeDTO>()
                .ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        //Query 8. Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            //string json = GetCategoriesByProductsCount(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/categories-by-products.json", json);

            var categories = context
                .Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = $"{c.CategoryProducts.Average(p => p.Product.Price):F2}",
                    totalRevenue = $"{c.CategoryProducts.Sum(p => p.Product.Price):F2}"
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();


            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;

        }

        //Query 9. Export Users and Products

        public static string GetUsersWithProducts(ProductShopContext context)
        {

            //string json = GetUsersWithProducts(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/users-and-products.json", json);

            var users = context
                .Users
                .ToArray()
                .Where(u => u.ProductsSold.Any())
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                        .Count(p => p.Buyer != null),
                        products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                        .ToArray()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                //.ProjectTo<UserCountProductSold>()
                //.ProjectTo<UsersDTO>()
                .ToArray();

            var resultObj = new
            {
                usersCoun = context.Users.Count(u => u.ProductsSold.Any()),
                users = users
            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(resultObj, settings);

            return json;

            //var users = context
            //    .Users
            //    .ToArray()
            //    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            //    .Select(u => new
            //    {
            //        firstName = u.FirstName,
            //        lastName = u.LastName,
            //        age = u.Age ?? 0,
            //        soldProducts = new
            //        {
            //            count = u.ProductsSold
            //            .Count(p => p.Buyer != null),
            //            products = u.ProductsSold
            //            .Where(p => p.Buyer != null)
            //            .Select(p => new
            //            {
            //                name = p.Name,
            //                price = $"{p.Price:F2}"
            //            })
            //            .ToArray()
            //        }
            //    })
            //    .OrderByDescending(u => u.soldProducts.count)
            //    //.ProjectTo<UserCountProductSold>()
            //    //.ProjectTo<UsersDTO>()
            //    .ToArray();

            //var resultObj = new
            //{
            //    usersCoun = context.Users.Count(u => u.ProductsSold.Any(p => p.Buyer != null)),
            //    users = users
            //};

            //JsonSerializerSettings settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    Formatting = Formatting.Indented
            //};

            //string json = JsonConvert.SerializeObject(resultObj, settings);

            //return json;


        }
    }
}