using System.Collections.ObjectModel;
using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using FluentAssertions;
using Moq;

namespace Business.Tests;

public class CustomerServiceTests
{
    [Test]
    public async Task GetAllAsync_Always_ReturnsAllCustomers()
    {
        //arrange
        var expected = GetTestCustomerModels;
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(x => x.CustomerRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(GetTestCustomerEntities.AsEnumerable());

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await customerService.GetAllAsync();

        //assert
        _ = actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsCustomerModel()
    {
        //arrange
        var expected = GetTestCustomerModels[0];
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        _ = mockUnitOfWork
            .Setup(m => m.CustomerRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestCustomerEntities[0]);

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await customerService.GetByIdAsync(1);

        //assert
        _ = actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddAsync_ValidModel_AddsCustomer()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.AddAsync(It.IsAny<Customer>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var customer = GetTestCustomerModels[0];

        //act
        await customerService.AddAsync(customer);

        //assert
        mockUnitOfWork.Verify(x => x.CustomerRepository.AddAsync(It.Is<Customer>(x =>
            x.Id == customer.Id && x.DiscountValue == customer.DiscountValue)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task AddAsync_EmptyName_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.AddAsync(It.IsAny<Customer>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var customer = GetTestCustomerModels[0];
        customer.Name = string.Empty;

        //act
        Func<Task> act = async () => await customerService.AddAsync(customer);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }


    [Test]
    public async Task AddAsync_NullModel_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.AddAsync(It.IsAny<Customer>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        Func<Task> act = async () => await customerService.AddAsync(null!);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task DeleteAsync_ExistingId_DeletesCustomer(int id)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.DeleteByIdAsync(It.IsAny<int>()));
        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        await customerService.DeleteAsync(id);

        //assert
        mockUnitOfWork.Verify(x => x.CustomerRepository.DeleteByIdAsync(id), Times.Once());
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
    }

    [TestCase("2099-10-10")]
    [TestCase("1000-1-1")]
    public async Task AddAsync_InvalidBirthDate_ThrowsMarketException(DateTime birthDate)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.AddAsync(It.IsAny<Customer>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var customer = GetTestCustomerModels[0];
        customer.BirthDate = birthDate;

        //act
        Func<Task> act = async () => await customerService.AddAsync(customer);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [Test]
    public async Task UpdateAsync_ValidModel_UpdatesCustomer()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.Update(It.IsAny<Customer>()));
        _ = mockUnitOfWork.Setup(m => m.PersonRepository.Update(It.IsAny<Person>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var customer = GetTestCustomerModels[0];

        //act
        await customerService.UpdateAsync(customer);

        //assert
        mockUnitOfWork.Verify(x => x.CustomerRepository.Update(It.Is<Customer>(x =>
            x.Id == customer.Id && x.DiscountValue == customer.DiscountValue)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_EmptySurname_ThrowsMarketException()
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.Update(It.IsAny<Customer>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var collection = GetTestCustomerModels;
        var customer = collection[^1];
        customer.Surname = null!;

        //act
        Func<Task> act = async () => await customerService.UpdateAsync(customer);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    [TestCase("2050-1-1")]
    [TestCase("1330-1-1")]
    public async Task UpdateAsync_InvalidBirthDate_ThrowsMarketException(DateTime birthDate)
    {
        //arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork.Setup(m => m.CustomerRepository.Update(It.IsAny<Customer>()));

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());
        var customer = GetTestCustomerModels[0];
        customer.BirthDate = birthDate;

        //act
        Func<Task> act = async () => await customerService.UpdateAsync(customer);

        //assert
        _ = await act.Should().ThrowAsync<MarketException>();
    }

    private static readonly int[] ProductId7ExpectedCustomers = [1, 2, 3];
    private static readonly int[] ProductId3ExpectedCustomers = [1];
    private static readonly int[] ProductId8ExpectedCustomers = [1, 2, 3];
    private static readonly int[] ProductId5ExpectedCustomers = [4];

    private static IEnumerable<object[]> GetCustomersByProductIdAsyncTestCases =>
    [
        [7, ProductId7ExpectedCustomers],
        [3, ProductId3ExpectedCustomers],
        [8, ProductId8ExpectedCustomers],
        [5, ProductId5ExpectedCustomers],
    ];

    [TestCaseSource(nameof(GetCustomersByProductIdAsyncTestCases))]
    public async Task GetCustomersByProductIdAsync_ValidProductId_ReturnsExpectedCustomers(int productId,
        int[] expectedCustomerIds)
    {
        //arrange
        var expected = GetTestCustomerModels.Where(x => expectedCustomerIds.Contains(x.Id)).ToList();

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _ = mockUnitOfWork
            .Setup(m => m.CustomerRepository.GetAllWithDetailsAsync())
            .ReturnsAsync(GetTestCustomerEntities.AsQueryable());

        var customerService = new CustomerService(mockUnitOfWork.Object, UnitTestBusinessHelper.CreateMapperProfile());

        //act
        var actual = await customerService.GetCustomersByProductIdAsync(productId);

        //assert
        _ = actual.Should().BeEquivalentTo(expected);
    }


    private static ReadOnlyCollection<CustomerModel> GetTestCustomerModels =>
        new List<CustomerModel>()
        {
            new CustomerModel
            {
                Id = 1,
                Name = "Viktor",
                Surname = "Zhuk",
                BirthDate = new DateTime(1995, 1, 2),
                DiscountValue = 10,
                ReceiptsIds = [1]
            },
            new CustomerModel
            {
                Id = 2,
                Name = "Nassim",
                Surname = "Taleb",
                BirthDate = new DateTime(1965, 5, 12),
                DiscountValue = 15,
                ReceiptsIds = [2]
            },
            new CustomerModel
            {
                Id = 3,
                Name = "Desmond",
                Surname = "Morris",
                BirthDate = new DateTime(1955, 4, 12),
                DiscountValue = 5,
                ReceiptsIds = [3]
            },
            new CustomerModel
            {
                Id = 4,
                Name = "Lebron",
                Surname = "James",
                BirthDate = new DateTime(1983, 12, 31),
                DiscountValue = 12,
                ReceiptsIds = [4]
            }
        }.AsReadOnly();

    private static ReadOnlyCollection<Person> GetTestPersonEntities =>
        new List<Person>()
        {
            new Person { Id = 1, Name = "Viktor", Surname = "Zhuk", BirthDate = new DateTime(1995, 1, 2) },
            new Person { Id = 2, Name = "Nassim", Surname = "Taleb", BirthDate = new DateTime(1965, 5, 12) },
            new Person { Id = 3, Name = "Desmond", Surname = "Morris", BirthDate = new DateTime(1955, 4, 12) },
            new Person { Id = 4, Name = "Lebron", Surname = "James", BirthDate = new DateTime(1983, 12, 31) }
        }.AsReadOnly();

    private static ReadOnlyCollection<Customer> GetTestCustomerEntities =>
        new List<Customer>()
        {
            new Customer
            {
                Id = 1,
                PersonId = 1,
                Person = GetTestPersonEntities[0],
                DiscountValue = 10,
                Receipts = [GetTestReceiptsEntitiesWithReceiptDetails[0]]
            },
            new Customer
            {
                Id = 2,
                PersonId = 2,
                Person = GetTestPersonEntities[1],
                DiscountValue = 15,
                Receipts = [GetTestReceiptsEntitiesWithReceiptDetails[1]]
            },
            new Customer
            {
                Id = 3,
                PersonId = 3,
                Person = GetTestPersonEntities[2],
                DiscountValue = 5,
                Receipts = [GetTestReceiptsEntitiesWithReceiptDetails[2]]
            },
            new Customer
            {
                Id = 4,
                PersonId = 4,
                Person = GetTestPersonEntities[3],
                DiscountValue = 12,
                Receipts = [GetTestReceiptsEntitiesWithReceiptDetails[3]]
            },
        }.AsReadOnly();

    private static ReadOnlyCollection<Receipt> GetTestReceiptsEntitiesWithReceiptDetails =>
        new List<Receipt>()
        {
            new Receipt
            {
                Id = 1,
                CustomerId = 1,
                IsCheckedOut = false,
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 1,
                        ProductId = 8,
                        UnitPrice = 10,
                        Quantity = 2,
                        ReceiptId = 1
                    },
                    new ReceiptDetail
                    {
                        Id = 2,
                        ProductId = 7,
                        UnitPrice = 20,
                        Quantity = 3,
                        ReceiptId = 1
                    },
                    new ReceiptDetail
                    {
                        Id = 3,
                        ProductId = 3,
                        UnitPrice = 25,
                        Quantity = 1,
                        ReceiptId = 1,
                    }
                ]
            },
            new Receipt
            {
                Id = 2,
                CustomerId = 2,
                IsCheckedOut = false,
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 4,
                        ProductId = 8,
                        UnitPrice = 10,
                        Quantity = 10,
                        ReceiptId = 2
                    },
                    new ReceiptDetail
                    {
                        Id = 5,
                        ProductId = 7,
                        UnitPrice = 25,
                        Quantity = 1,
                        ReceiptId = 2
                    }
                ]
            },
            new Receipt
            {
                Id = 3,
                CustomerId = 3,
                IsCheckedOut = false,
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 6,
                        ProductId = 8,
                        UnitPrice = 10,
                        Quantity = 10,
                        ReceiptId = 3
                    },
                    new ReceiptDetail
                    {
                        Id = 7,
                        ProductId = 7,
                        UnitPrice = 25,
                        Quantity = 1,
                        ReceiptId = 3
                    }
                ]
            },
            new Receipt
            {
                Id = 4,
                CustomerId = 4,
                IsCheckedOut = false,
                ReceiptDetails =
                [
                    new ReceiptDetail
                    {
                        Id = 8,
                        ProductId = 5,
                        UnitPrice = 10,
                        Quantity = 10,
                        ReceiptId = 4
                    },
                    new ReceiptDetail
                    {
                        Id = 8,
                        ProductId = 6,
                        UnitPrice = 25,
                        Quantity = 1,
                        ReceiptId = 4
                    }
                ]
            }
        }.AsReadOnly();
}
