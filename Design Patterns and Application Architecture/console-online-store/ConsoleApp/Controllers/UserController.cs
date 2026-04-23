namespace ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using ConsoleApp.Controllers;
using ConsoleApp.Handlers.ContextMenuHandlers;
using ConsoleApp.Helpers;
using ConsoleMenu;
using StoreBLL.Models;
using StoreBLL.Services;
using StoreDAL.Data;

public static class UserController
{
    private static StoreDbContext context = UserMenuController.Context;

    public static void AddUser()
    {
        Console.WriteLine("--- Registration ---");
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();
        Console.Write("Login: ");
        var login = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        var service = new UserService(context);
        service.Add(new UserModel(0, name ?? string.Empty, lastName ?? string.Empty, login ?? string.Empty, password ?? string.Empty, 2));
        Console.WriteLine("Registration successful! You can now login.");
        Console.ReadKey();
    }

    public static void UpdateUser()
    {
        var service = new UserService(context);
        var user = service.GetById(UserMenuController.CurrentUserId) as UserModel;
        if (user != null)
        {
            Console.WriteLine("--- Update Personal Data ---");
            Console.WriteLine($"Current Name: {user.Name}");
            Console.Write("New Name (leave empty to keep): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                user.Name = name;
            }

            Console.WriteLine($"Current Last Name: {user.LastName}");
            Console.Write("New Last Name (leave empty to keep): ");
            var lastName = Console.ReadLine();
            if (!string.IsNullOrEmpty(lastName))
            {
                user.LastName = lastName;
            }

            Console.Write("New Password (leave empty to keep): ");
            var password = Console.ReadLine();
            if (!string.IsNullOrEmpty(password))
            {
                user.Password = password;
            }

            service.Update(user);
            Console.WriteLine("Data updated successfully!");
        }

        Console.ReadKey();
    }

    public static void DeleteUser()
    {
        throw new NotImplementedException();
    }

    public static void ShowUser()
    {
        throw new NotImplementedException();
    }

    public static void ShowAllUsers()
    {
        throw new NotImplementedException();
    }

    public static void AddUserRole()
    {
        throw new NotImplementedException();
    }

    public static void UpdateUserRole()
    {
        throw new NotImplementedException();
    }

    public static void DeleteUserRole()
    {
        throw new NotImplementedException();
    }

    public static void ShowAllUserRoles()
    {
        var service = new UserRoleService(context);
        var menu = new ContextMenu(new AdminContextMenuHandler(service, InputHelper.ReadUserRoleModel), service.GetAll);
        menu.Run();
    }

    public static void AddProductTitle()
    {
        throw new NotImplementedException();
    }

    public static void UpdateProductTitle()
    {
        throw new NotImplementedException();
    }

    public static void DeleteProductTitle()
    {
        throw new NotImplementedException();
    }

    public static void ShowAllProductTitles()
    {
        throw new NotImplementedException();
    }

    public static void AddManufacturer()
    {
        throw new NotImplementedException();
    }

    public static void UpdateManufacturer()
    {
        throw new NotImplementedException();
    }

    public static void DeleteManufacturer()
    {
        throw new NotImplementedException();
    }

    public static void ShowAllManufacturers()
    {
        throw new NotImplementedException();
    }
}
