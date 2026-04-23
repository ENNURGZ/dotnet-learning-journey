namespace StoreDAL.Data.InitDataFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreDAL.Entities;

public class TestDataFactory : AbstractDataFactory
{
    public override Category[] GetCategoryData()
    {
        return new[]
        {
            new Category(1, "fruits"),
            new Category(2, "water"),
            new Category(3, "vegetables"),
            new Category(4, "seafood"),
            new Category(5, "meet"),
            new Category(6, "grocery"),
            new Category(7, "milk food"),
            new Category(8, "smartphones"),
            new Category(9, "laptop"),
            new Category(10, "photocameras"),
            new Category(11, "kitchen accesories"),
            new Category(12, "spices"),
            new Category(13, "Juice"),
            new Category(14, "alcohol drinks"),
        };
    }

    public override CustomerOrder[] GetCustomerOrderData()
    {
        return new[]
        {
            new CustomerOrder(1, "2023-01-01 10:00:00", 2, 1),
        };
    }

    public override Manufacturer[] GetManufacturerData()
    {
        return new[]
        {
            new Manufacturer(1, "Apple"),
            new Manufacturer(2, "Samsung"),
            new Manufacturer(3, "Sony"),
        };
    }

    public override OrderDetail[] GetOrderDetailData()
    {
        return new[]
        {
            new OrderDetail(1, 1, 1, 999.99m, 1),
        };
    }

    public override OrderState[] GetOrderStateData()
    {
        return new[]
        {
            new OrderState(1, "New Order"),
            new OrderState(2, "Cancelled by user"),
            new OrderState(3, "Cancelled by administrator"),
            new OrderState(4, "Confirmed"),
            new OrderState(5, "Moved to delivery company"),
            new OrderState(6, "In delivery"),
            new OrderState(7, "Delivered to client"),
            new OrderState(8, "Delivery confirmed by client"),
        };
    }

    public override Product[] GetProductData()
    {
        return new[]
        {
            new Product(1, 1, 1, "Latest iPhone model", 999.99m),
            new Product(2, 2, 2, "Flagship Samsung phone", 899.99m),
        };
    }

    public override ProductTitle[] GetProductTitleData()
    {
        return new[]
        {
            new ProductTitle(1, "iPhone 13", 8),
            new ProductTitle(2, "Galaxy S21", 8),
        };
    }

    public override User[] GetUserData()
    {
        return new[]
        {
            new User(1, "Admin", "Admin", "admin", string.Empty, 1),
            new User(2, "User", "User", "user", string.Empty, 2),
        };
    }

    public override UserRole[] GetUserRoleData()
    {
        return new[]
        {
            new UserRole(1, "Admin"),
            new UserRole(2, "Registered"),
            new UserRole(3, "Guest"),
        };
    }
}
