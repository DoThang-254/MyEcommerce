using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ProductService.Domain.Entities;
using ProductService.Domain.Events;

namespace ProductService.Domain.Tests;

public class ProductTests
{
    [Fact]
    public void CreateProduct_Should_Add_ProductCreatedEvent()
    {
        // Act
        var product = Product.Create("Iphone 15", "Apple", 1000, 10, Guid.NewGuid());
        
        // Assert
        // Kiểm tra xem trong cái "túi" chứa event của Product có cái nào tên là ProductCreatedEvent không
        Assert.Contains(product.DomainEvents, e => e is ProductCreatedEvent);
    }
}