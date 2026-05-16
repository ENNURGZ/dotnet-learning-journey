using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportsStore.Models;

public class SessionCart : Cart
{
    public static Cart GetCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
        SessionCart cart = new SessionCart { Session = session };
        string? cartData = session?.GetString("Cart");
        if (!string.IsNullOrEmpty(cartData))
        {
            var lines = JsonSerializer.Deserialize<List<CartLine>>(cartData);
            if (lines != null)
            {
                cart.Lines.Clear();
                cart.Lines.AddRange(lines);
            }
        }
        return cart;
    }

    [JsonIgnore]
    public ISession? Session { get; set; }

    public override void AddItem(Product product, int quantity)
    {
        base.AddItem(product, quantity);
        SaveCart();
    }

    public override void RemoveLine(Product product)
    {
        base.RemoveLine(product);
        SaveCart();
    }

    public override void RemoveLine(long productId)
    {
        base.RemoveLine(productId);
        SaveCart();
    }

    public override void Clear()
    {
        base.Clear();
        Session?.Remove("Cart");
    }

    private void SaveCart()
    {
        if (Session != null)
        {
            Session.SetString("Cart", JsonSerializer.Serialize(Lines));
        }
    }
}
