using Microsoft.AspNetCore.Mvc;
using SportsStore.Components;

namespace SportsStore.Tests;

[Category("Step5")]
public class AdminNavigationMenuViewComponentTests
{
    [Test]
    public void AdminNavigationMenuViewComponent_CanBeInstantiated()
    {
        // Arrange & Act
        var viewComponent = new AdminNavigationMenuViewComponent();

        // Assert
        Assert.That(viewComponent, Is.Not.Null);
        Assert.That(viewComponent, Is.InstanceOf<AdminNavigationMenuViewComponent>());
    }

    [Test]
    public void AdminNavigationMenuViewComponent_InheritsFromViewComponent()
    {
        // Arrange & Act
        var viewComponent = new AdminNavigationMenuViewComponent();

        // Assert
        Assert.That(viewComponent, Is.InstanceOf<ViewComponent>());
    }

    [Test]
    public void AdminNavigationMenuViewComponent_HasCorrectType()
    {
        // Arrange & Act
        var viewComponent = new AdminNavigationMenuViewComponent();

        // Assert
        Assert.That(viewComponent.GetType().Name, Is.EqualTo("AdminNavigationMenuViewComponent"));
    }
}