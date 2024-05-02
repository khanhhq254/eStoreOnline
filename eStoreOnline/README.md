# Razer Ecommerce Assessment - eStoreOnline

## Introduction

This is a simple e-commerce website that allows users to view products, add products to cart, and checkout. The website is responsive and works on both desktop and mobile devices.
The website is built using ASP.NET Core MVC, Entity Framework Core, and Razor Pages.

## Features

- View products
- Add products to cart
- Remove products from cart
- View cart
- Checkout
- View order history 
- Responsive design
- User authentication and authorization
- Admin panel to manage products

## Technologies

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- Stripe API and CLI
- Identity Provider
- Razor Pages
- Bootstrap and JQuery
- MySQL
- SendGrid

## Project Structure

```
https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures
```
The project follows clean architecture principles and is divided into the following layers:

- **eStoreOnline**: Web UI layer, includes Razor Pages, controllers, and views
- **eStoreOnline.Domain**: Domain layer, includes all POCO classes
- **eStoreOnline.Infrastructure**: Data access layer, includes Entity Framework Core context and migrations, and third-party services
- **eStoreOnline.Application**: Business logic layer, includes services and interfaces

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio
3. Update the connection string in `appsettings.json` to point to your MySQL database
    - MySQL can be installed by following the instructions [here](https://dev.mysql.com/doc/mysql-installation-excerpt/8.0/en/) or using Docker
    - The default connection string is `server=localhost;port=3306;database=estoreonline;user=root;password=defaultrootpasswordinlocal`
4. Run the following commands in the Package Manager Console to create the database and apply migrations:

```
dotnet ef database update -p eStoreOnline.Infrastructure -s eStoreOnline
```

5. Run the application