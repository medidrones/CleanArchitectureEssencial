using CleanArchMvc.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchMvc.Domain.Tests;

public class ProductUnitTest
{
    [Fact]
    public void CreateProduct_WithValidParameters_ResultObjectValidState()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, "product image");
        action.Should().NotThrow<Validation.DomainExceptionValidation>();
    }

    [Fact]
    public void CreateProduct_NegativeIdValue_DomainExceptionInvalidId()
    {
        Action action = () => new Product(-1, "Product Name", "Product Description", 9.99m, 99, "product image");
        action.Should().Throw<Validation.DomainExceptionValidation>().WithMessage("Invalid Id value.");
    }

    [Fact]
    public void CreateProduct_ShortNameValue_DomainExceptionInvalidName()
    {
        Action action = () => new Product(1, "Pr", "Product Description", 9.99m, 99, "product image");
        action.Should().Throw<Validation.DomainExceptionValidation>().WithMessage("Invalid name, too short, minimum 3 charecters.");
    }

    [Fact]
    public void CreateProduct_LongImageName_DomainExceptionLongImageName()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, "product image tooooooooooooooooooooooooooooooooooooooooooooooooooooo loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo imaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa fuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu");
        action.Should().Throw<Validation.DomainExceptionValidation>().WithMessage("Invalid image, too short, minimum 250 charecters.");
    }

    [Fact]
    public void CreateProduct_WithNullImageName_NoDomainException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, null);
        action.Should().NotThrow<Validation.DomainExceptionValidation>();
    }

    [Fact]
    public void CreateProduct_WithNullImageName_NoNullReferenceException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, null);
        action.Should().NotThrow<NullReferenceException>();
    }

    [Fact]
    public void CreateProduct_WithEmptyImageName_NoDomainException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, "");
        action.Should().NotThrow<Validation.DomainExceptionValidation>();
    }

    [Fact]
    public void CreateProduct_InvalidPriceValue_DomainException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", -9.99m, 99, "product image");
        action.Should().Throw<Validation.DomainExceptionValidation>().WithMessage("Invalid price value.");
    }

    [Theory]
    [InlineData(-5)]
    public void CreateProduct_InvalidStockValue_DomainExceptionNegativeValue(int value)
    {
        Action action = () => new Product(1, "Pro", "Product Description", 9.99m, value, "product image");
        action.Should().Throw<Validation.DomainExceptionValidation>().WithMessage("Invalid stock value.");
    }
}
