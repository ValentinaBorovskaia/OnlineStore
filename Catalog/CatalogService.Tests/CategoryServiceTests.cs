using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Interfaces;
using FluentAssertions;
using FluentAssertions.Specialized;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Application.Tests
{
    public class CategoryServiceTests
    {
        private ICategoryService categoryService;
        private Mock<ICategoryRepository> categoryRepository =
            new Mock<ICategoryRepository>();

        Category updatedCategory = new Category()
        {
            Id = 1,
            Image = "",
            Name = "Test",
            ParentId = 1
        };
        Category addedCategory = new Category()
        {
            Id = 1,
            Image = "",
            Name = "Test",
            ParentId = 0
        };

        [SetUp]
        public void Setup()
        {
            categoryService = new CategoryService(categoryRepository.Object);
        }

       

        [Test]
        public async Task AddCategory_OkIfInputDataIsValid()
        {
            var category = It.IsAny<Category>();
            categoryRepository.Setup(x => x.Add(category))
                .ReturnsAsync(addedCategory);

            var result = await categoryService.AddCategory(category);

            categoryRepository.Verify(x => x.Add(It.IsAny<Category>()), Times.Once);
        }

        [Test]
        public async Task AddCategory_ThrownExceptionIfInputDataIsInvalid()
        {           
            categoryRepository.Setup(x => x.Add(It.IsAny<Category>()))
                .ThrowsAsync(new ValidationException("Invalid input data"));
      
            Assert.ThrowsAsync<ValidationException>(async () => await categoryService.AddCategory(It.IsAny<Category>()));
            categoryRepository.Verify(x => x.Add(It.IsAny<Category>()));

        }

        [Test]
        public async Task DeleteCategory_OkIfExist()
        {
            categoryRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await categoryService.DeleteCategory(It.IsAny<int>());

            Assert.AreEqual(result, true);
        }

        [Test]
        public void DeleteCategory_ThrownExceptionIfNotExist()
        {
            categoryRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .ThrowsAsync(new NullReferenceException("Category doesn't exist"));

            Assert.ThrowsAsync<NullReferenceException>(async () => await categoryService.DeleteCategory(It.IsAny<int>()));
            categoryRepository.Verify(x => x.Delete(It.IsAny<int>()));
        }

        
        [Test]
        public async Task UpdateCategory_OkIfDataIsValid()
        {
            categoryRepository.Setup(x => x.Update(It.IsAny<int>(), updatedCategory))
                .ReturnsAsync(updatedCategory);

            var result = await categoryService.UpdateCategory(updatedCategory.Id, updatedCategory);

            categoryRepository.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()), Times.Once);
        }
        
        [Test]
        public void UpdateCategory_ThrowExceptionIfNotExist()
        {
            categoryRepository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()))
                           .ThrowsAsync(new NullReferenceException("Category doesn't exist"));

            Assert.ThrowsAsync<NullReferenceException>(async () => await categoryService.UpdateCategory(It.IsAny<int>(), It.IsAny<Category>()));
            categoryRepository.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()));
        }
        
        [Test]
        public void UpdateCategory_ThrowExceptionIfDataIsInvalid()
        {
            categoryRepository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()))
                           .ThrowsAsync(new ValidationException("Invalid input data"));

            Assert.ThrowsAsync<ValidationException>(async () => await categoryService.UpdateCategory(It.IsAny<int>(), It.IsAny<Category>()));
            categoryRepository.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()));

        }

        [Test]
        public async Task GetCategoryById_OkIfInputDataIsValid()
        {
            categoryRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(updatedCategory);

            var result = await categoryService.GetCategoryById(1);

            categoryRepository.Verify(x => x.GetById(It.IsAny<int>()));
        }

        [Test]
        public void GetCategoryById__ThrownExceptionIfNotExist()
        {
            categoryRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .ThrowsAsync(new NullReferenceException("Category doesn't exist"));

            Assert.ThrowsAsync<NullReferenceException>(async () => await categoryService.GetCategoryById(It.IsAny<int>()));
            categoryRepository.Verify(x => x.GetById(It.IsAny<int>()));
        }
        
        [Test]
        public async Task GetCategories()
        {
            categoryRepository.Setup(x => x.GetAll())
                .ReturnsAsync(new List<Category>());

            var result = await categoryService.GetCategories();

            Assert.AreEqual(result.Count(), 0);
        }
    }
}