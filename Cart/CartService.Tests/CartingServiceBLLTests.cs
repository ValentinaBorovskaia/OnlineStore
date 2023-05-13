using CartingService.BLL.Interfaces;
using CartingService.BLL.Services;
using CartingService.DAL.Entities;
using CartingService.DAL.Interface;
using Moq;
using NUnit;
using FluentAssertions;
using FluentAssertions.Specialized;

namespace CartingService.Tests
{
    public class CartingServiceBLLTests
    {
        private Mock<ICartRepository> cartRepository;
        private ICartService cartService;
        private List<Item> testItems = new List<Item>() {
            new Item()
            { 
                Id = 1, 
                Name = "test item 1",
                Image = "http://ImageUrl/TestItem1.jpg",
                Quantity = 1,
            },
             new Item()
            {
                Id = 2,
                Name = "test item 2",
                Image = "http://ImageUrl/TestItem1.jpg",
                Quantity = 1,
            },
              new Item()
            {
                Id = 3,
                Name = "test item 3",
                Image = "http://ImageUrl/TestItem1.jpg",
                Quantity = 1
            },
       };
        [SetUp]
        public void Setup()
        {
            cartRepository = new Mock<ICartRepository>();
            cartService = new CartService(cartRepository.Object);
        }

        [Test]
        public void AddItem_ItemsNotExist()
        {
            //Arrange
            var cartId = Guid.NewGuid();

            //Act
            cartRepository.Setup(x => x.GetCartById(It.IsAny<Guid>())).Returns((Cart)null);
            cartRepository.Setup(x => x.SaveChanges(It.IsAny<Cart>())).Returns(true);
            //Assert
            Assert.AreEqual(true, cartService.AddItem(cartId, testItems[0]));
        }

        [Test]
        public void RemoveItem_WhenEverithingOk()
        {
            //Arrange
            var cartId = Guid.NewGuid();

            //Act
            cartRepository.Setup(x => x.GetCartById(It.IsAny<Guid>())).Returns(new Cart() { Id = cartId, Items = new List<Item>() { testItems[0] } });
            cartRepository.Setup(x => x.SaveChanges(It.IsAny<Cart>())).Returns(true);
            //Assert
            Assert.AreEqual(true, cartService.RemoveItem(cartId, 1));
        }

        [Test]
        public void RemoveItem_ThrowWhenCartNotExist()
        {
            //Arrange
            var cartId = Guid.NewGuid();

            //Act
            cartRepository.Setup(x => x.GetCartById(It.IsAny<Guid>())).Returns((Cart)null);

            //Assert
            Assert.Throws<NullReferenceException>(() => cartService.RemoveItem(cartId, It.IsAny<int>()));
        }

        [Test]
        public void RemoveItem_ThrowWhenItemNotExist()
        {
            //Arrange
            var cartId = Guid.NewGuid();

            //Act
            cartRepository.Setup(x => x.GetCartById(It.IsAny<Guid>())).Returns(new Cart() { Id = cartId, Items = new List<Item>() });

            //Assert
            Assert.Throws<NullReferenceException>(() => cartService.RemoveItem(cartId, It.IsAny<int>()));
        }

    }

}