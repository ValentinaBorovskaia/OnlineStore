using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Interfaces;
using CatalogService.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Tests
{
    public class ItemServiceTests 
    {
        private IItemService itemService;
        private Mock<IItemRepository> itemRepository =
        new Mock<IItemRepository>();

        Item addedItem = new Item()
        {
            Id = 2,
            CategoryId = 1,
            Name = "Added Item"
        };
        Item updatedItem = new Item()
        {
            Id = 2,
            CategoryId = 2,
            Name = "Updated Item"
        };

        [SetUp]
        public void Setup()
        {
            itemService = new ItemService(itemRepository.Object);
        }

        [Test]
        public async Task AddItem_OkIfDataIsValid()
        {
            
            itemRepository.Setup(x => x.Add(It.IsAny<Item>()))
                .ReturnsAsync(addedItem);

            var result = await itemService.AddItem(It.IsAny<Item>());

            itemRepository.Verify(x => x.Add(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void AddItem_ThrowIfDataIsInvalid() 
        {
            itemRepository.Setup(x => x.Add(It.IsAny<Item>()))
                .ThrowsAsync(new ValidationException("Invalid input data"));

            Assert.ThrowsAsync<ValidationException>(async () => await itemService.AddItem(It.IsAny<Item>()));
            itemRepository.Verify(x => x.Add(It.IsAny<Item>()));
        }

        [Test]
        public async Task UpdateItem_OkIfDataIsValid()
        {
            itemRepository.Setup(x => x.Update(It.IsAny<int>(), updatedItem))
                .ReturnsAsync(updatedItem);

            var result = await itemService.UpdateItem(updatedItem.Id, updatedItem);

            itemRepository.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void UpdateItem_ThrowExceptionIfNotExist()
        {
            itemRepository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Item>()))
                           .ThrowsAsync(new NullReferenceException("Item doesn't exist"));

            Assert.ThrowsAsync<NullReferenceException>(async () => await itemService.UpdateItem(It.IsAny<int>(), It.IsAny<Item>()));
            itemRepository.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<Item>()));
        }

        [Test]
        public void UpdateItem_ThrowExceptionIfDataIsInvalid()
        {
            itemRepository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Item>()))
                           .ThrowsAsync(new ValidationException("Invalid input data"));

            Assert.ThrowsAsync<ValidationException>(async () => await itemService.UpdateItem(It.IsAny<int>(), It.IsAny<Item>()));
            itemRepository.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<Item>()));
        }


        [Test]
        public async Task DeleteItem_OkIfExist()
        {
            itemRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await itemService.DeleteItem(It.IsAny<int>());

            Assert.AreEqual(result, true);
        }

        [Test]
        public void DeleteItem_ThrownExceptionIfNotExist()
        {
            itemRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .ThrowsAsync(new NullReferenceException("Item doesn't exist"));

            Assert.ThrowsAsync<NullReferenceException>(async () => await itemService.DeleteItem(It.IsAny<int>()));
            itemRepository.Verify(x => x.Delete(It.IsAny<int>()));
        }

        [Test]
        public async Task GetItemId_OkIfItemExist()
        {
            var item = new Item();
            itemRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(item);

            var result = await itemService.GetItem(1);

            itemRepository.Verify(x => x.GetById(It.IsAny<int>()));
            Assert.AreEqual(result, item);
        }

        [Test]
        public void GetItemById_ThrowIfItemNotExist() 
        {
            itemRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .ThrowsAsync(new NullReferenceException("Item doesn't exist"));

            Assert.ThrowsAsync<NullReferenceException>(async () => await itemService.GetItem(It.IsAny<int>()));
            itemRepository.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [Test]
        public async Task GetItems()
        {
            itemRepository.Setup(x => x.GetItems(null))
                .ReturnsAsync(new List<Item>());

            var result = await itemService.GetItems(null);

            Assert.AreEqual(result.Count(), 0);
        }
    }
}
