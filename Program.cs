using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Week3EntityFramework.Dtos;

var context = new IndustryConnectWeek2Context();

//var customer = new Customer
//{
//    DateOfBirth = DateTime.Now.AddYears(-20)
//};


//Console.WriteLine("Please enter the customer firstname?");

//customer.FirstName = Console.ReadLine();

//Console.WriteLine("Please enter the customer lastname?");

//customer.LastName = Console.ReadLine();


//var customers = context.Customers.ToList();

//foreach (Customer c in customers)
//{   
//    Console.WriteLine("Hello I'm " + c.FirstName);
//}

//Console.WriteLine($"Your new customer is {customer.FirstName} {customer.LastName}");

//Console.WriteLine("Do you want to save this customer to the database?");

//var response = Console.ReadLine();

//if (response?.ToLower() == "y")
//{
//    context.Customers.Add(customer);
//    context.SaveChanges();
//}



var sales = context.Sales.Include(c => c.Customer)
    .Include(p => p.Product).ToList();

var salesDto = new List<SaleDto>();

foreach (Sale s in sales)
{
    salesDto.Add(new SaleDto(s));
}



//context.Sales.Add(new Sale
//{
//    ProductId = 1,
//    CustomerId = 1,
//    StoreId = 1,
//    DateSold = DateTime.Now
//});


//context.SaveChanges();




Console.WriteLine("Which customer record would you like to update?");

var response = Convert.ToInt32(Console.ReadLine());

var customer = context.Customers.Include(s => s.Sales)
    .ThenInclude(p => p.Product)
    .FirstOrDefault(c => c.Id == response);


var total = customer.Sales.Select(s => s.Product.Price).Sum();


var customerSales = context.CustomerSales.ToList();

//var totalsales = customer.Sales



//Console.WriteLine($"The customer you have retrieved is {customer?.FirstName} {customer?.LastName}");

//Console.WriteLine($"Would you like to updated the firstname? y/n");

//var updateResponse = Console.ReadLine();

//if (updateResponse?.ToLower() == "y")
//{

//    Console.WriteLine($"Please enter the new name");

//    customer.FirstName = Console.ReadLine();
//    context.Customers.Add(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//    context.SaveChanges();
//}

//1) List of customer from database who dont have sales

var customersWithNoSales = context.Customers
.Where(c => !context.Sales.Any(s => s.CustomerId == c.Id))
.ToList();


foreach (var Customer in customersWithNoSales)
{
    Console.WriteLine(Customer.FirstName + ' ' + Customer.LastName);
}


//2)Insert new customer with sale record

Console.WriteLine("Enter Customer First Name:");
string firstName = Console.ReadLine().ToLower();

Console.WriteLine("Enter Customer Last Name:");
string lastName = Console.ReadLine().ToLower();


Console.WriteLine("Enter Product ID:");
if (!int.TryParse(Console.ReadLine(), out int productId))
{
    Console.WriteLine("Invalid Product ID.");
    return;
}

var newCustomer = new Customer
{
    FirstName = firstName,
    LastName = lastName
};

var product = context.Products.FirstOrDefault(p => p.Id == productId);
if (product == null)
{
    Console.WriteLine($"Product with ID '{productId}' does not exist. Please check the Product ID.");
    return;
}

var newSale = new Sale
{
    Customer = newCustomer,
    Product = product,
};

context.Customers.Add(newCustomer);
context.Sales.Add(newSale);

context.SaveChanges();

Console.WriteLine("Customer and sale record added successfully.");

// 3)Add a new store

Console.WriteLine("Enter Store Name:");
string Name = Console.ReadLine();

var newStore = new Store
{
    Name = Name
};

context.Stores.Add(newStore);
context.SaveChanges();


Console.WriteLine();


//4) List of all stores

var storesWithSales = context.Stores.Include(s => s.Sales)
.Where(s => s.Sales.Count > 0)
.ToList();

foreach (var store in storesWithSales)
{
    Console.WriteLine($"{store.Name}");
}


Console.ReadLine();








