namespace StoreBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserModel : AbstractModel
{
    public UserModel(int id, string name, string lastName, string login, string password, int roleId)
        : base(id)
    {
        this.Name = name;
        this.LastName = lastName;
        this.Login = login;
        this.Password = password;
        this.RoleId = roleId;
    }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; }
}
