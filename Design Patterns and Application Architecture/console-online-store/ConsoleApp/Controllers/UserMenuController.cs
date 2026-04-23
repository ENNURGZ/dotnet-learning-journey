using ConsoleMenu;
using ConsoleMenu.Builder;
using StoreDAL.Data;
using StoreDAL.Data.InitDataFactory;

namespace ConsoleApp1;

public enum UserRoles
{
    Guest,
    Administrator,
    RegistredCustomer,
}

public static class UserMenuController
{
    private static readonly Dictionary<UserRoles, Menu> RolesToMenu;
    private static int userId;
    private static UserRoles userRole;
    private static StoreDbContext context;

    static UserMenuController()
    {
        userId = 0;
        userRole = UserRoles.Guest;
        RolesToMenu = new Dictionary<UserRoles, Menu>();
        var factory = new StoreDbFactory(new TestDataFactory());
        context = factory.CreateContext();
        RolesToMenu.Add(UserRoles.Guest, new GuestMainMenu().Create(context));
        RolesToMenu.Add(UserRoles.RegistredCustomer, new UserMainMenu().Create(context));
        RolesToMenu.Add(UserRoles.Administrator, new AdminMainMenu().Create(context));
    }

    public static StoreDbContext Context
    {
        get { return context; }
    }

    public static int CurrentUserId
    {
        get { return userId; }
    }

    public static UserRoles CurrentUserRole
    {
        get { return userRole; }
    }

    public static void Login()
    {
        Console.WriteLine("Login: ");
        var login = Console.ReadLine();
        Console.WriteLine("Password: ");
        var password = Console.ReadLine();
        var userService = new StoreBLL.Services.UserService(context);
        var users = userService.GetAll().Cast<StoreBLL.Models.UserModel>();

        string hashedPwd = password ?? string.Empty;
        if (!string.IsNullOrEmpty(password))
        {
             var bytes = System.Text.Encoding.UTF8.GetBytes(password);
             hashedPwd = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(bytes));
        }

        var user = users.FirstOrDefault(u => u.Login == login && u.Password == hashedPwd);

        if (user != null)
        {
            userId = user.Id;
            if (user.RoleId == 1)
            {
                userRole = UserRoles.Administrator;
            }
            else if (user.RoleId == 2)
            {
                userRole = UserRoles.RegistredCustomer;
            }
            else
            {
                userRole = UserRoles.Guest;
            }

            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Invalid login or password.");
            Console.ReadKey();
        }
    }

    public static void Logout()
    {
        userId = 0;
        userRole = UserRoles.Guest;
    }

    public static void Start()
    {
        ConsoleKey resKey;
        bool updateItems = true;
        do
        {
                resKey = RolesToMenu[userRole].RunOnce(ref updateItems);
        }
        while (resKey != ConsoleKey.Escape);
    }
}