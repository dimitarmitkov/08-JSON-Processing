using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public const string ResultDirectoryPath = "../../../Datasets/Results";

        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();

            //ResetDataBase(db);



            string inputJson = File.ReadAllText("../../../Datasets/parts.json");

            string result = ImportParts(db, inputJson);

            Console.WriteLine(result);





            InitializeMapper();

            //string json = GetSalesWithAppliedDiscount(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/sales-discounts.json", json);

        }

        public static void ResetDataBase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("DB deleted");
            db.Database.EnsureCreated();
            Console.WriteLine("DB created");

        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
                //cfg.CreateMap<>();
            });
        }

        //Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            //string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");

            //string result = ImportSuppliers(db, inputJson);

            //Console.WriteLine(result);

            Supplier[] suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}.";

        }

        //Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {

            //string inputJson = File.ReadAllText("../../../Datasets/parts.json");

            //string result = ImportParts(db, inputJson);

            //Console.WriteLine(result);


            var existingSuppliers = context.Suppliers
               .Select(s => s.Id)
               .ToArray();

            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => existingSuppliers.Contains(p.SupplierId))
                .ToArray();

            context.Parts.AddRange(parts);
            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}.";
        }

        //Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            //string inputJson = File.ReadAllText("../../../Datasets/cars.json");

            //string result = ImportCars(db, inputJson);

            //Console.WriteLine(result);

            Car[] cars = JsonConvert.DeserializeObject<Car[]>(inputJson);

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";

        }

        //Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ////10-13
            //string inputJson = File.ReadAllText("../../../Datasets/customers.json");

            //string result = ImportCustomers(db, inputJson);

            //Console.WriteLine(result);

            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";

        }

        //Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ////10-13
            //string inputJson = File.ReadAllText("../../../Datasets/sales.json");

            //string result = ImportSales(db, inputJson);

            //Console.WriteLine(result);

            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";

        }

        //Query 14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {

            //string json = GetOrderedCustomers(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/ordered-customers.json", json);

            var customersBirthDate = context
                .Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                //.Select(c => new
                //{
                //    Name = c.Name,
                //    BirthDate = c.BirthDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                //    IsYoungDriver = c.IsYoungDriver
                //})
                .ProjectTo<OrderedCustomersDTO>()
                .ToArray();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy",
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(customersBirthDate, settings);

            return json;
        }

        //Query 15. Export Cars from Make Toyota

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            //string json = GetCarsFromMakeToyota(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/toyota-cars.json", json);

            var customersToyota = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                //.Select(c => new
                //{
                //    c.Id,
                //    c.Make,
                //    c.Model,
                //    c.TravelledDistance
                //})
                .ProjectTo<ToyotaSelectCarsDTO>()
                .ToArray();

            string json = JsonConvert.SerializeObject(customersToyota, Formatting.Indented);

            return json;
        }

        //Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {

            //string json = GetLocalSuppliers(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/local-suppliers.json", json);

            var suplliersLocal = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                //.Select(s => new
                //{
                //    s.Id,
                //    s.Name,
                //    //PartsCount = s.Parts.Select(p => new
                //    //{
                //    //    p.Quantity
                //    //})
                //    //.Count()
                //    s.Parts.Count
                //})
                .ProjectTo<LocalSuppliersDTO>()
                .ToArray();


            string json = JsonConvert.SerializeObject(suplliersLocal, Formatting.Indented);

            return json;
        }

        //Query 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            //string json = GetCarsWithTheirListOfParts(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/cars-and-parts.json", json);

            //var carListOfParts = context
            //    .Cars
            //    .ProjectTo<CarsDataDTO>()
            //    .ToArray();

            var carListOfParts = context
               .Cars
               .Select(c => new
               {
                   car = new
                   {
                       c.Make,
                       c.Model,
                       c.TravelledDistance
                   },
                   parts = c.PartCars.Select(p => new
                   {
                       Name = p.Part.Name,
                       Price = $"{p.Part.Price:F2}"
                   })
                   .ToArray()
               })
               .ToArray();


            string json = JsonConvert.SerializeObject(carListOfParts, Formatting.Indented);

            return json;
        }

        //Query 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //string json = GetTotalSalesByCustomer(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/customers-total-sales.json", json);

            var customersTotalSales = context
                .Customers
                .Where(c => c.Sales.Count() >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            string json = JsonConvert.SerializeObject(customersTotalSales, Formatting.Indented);

            return json;
        }

        //Query 19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //string json = GetSalesWithAppliedDiscount(db);

            //if (!Directory.Exists(ResultDirectoryPath))
            //{
            //    Directory.CreateDirectory(ResultDirectoryPath);
            //}

            //File.WriteAllText(ResultDirectoryPath + "/sales-discounts.json", json);

            var salesWithDiscount = context
                .Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },

                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount:F2}",
                    price = $"{s.Car.PartCars.Sum(pp => pp.Part.Price):F2}",
                    priceWithDiscount = $"{(s.Car.PartCars.Sum(pp => pp.Part.Price) - s.Car.PartCars.Sum(pp => pp.Part.Price) * s.Discount / 100):F2}"
                })
                .ToArray();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(salesWithDiscount, settings);

            return json;
        }
    }
}