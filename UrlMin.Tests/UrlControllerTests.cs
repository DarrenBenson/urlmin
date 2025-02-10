using Microsoft.AspNetCore.Mvc;
using Xunit;
using UrlMin.Controllers;
using UrlMin.Models;
using System.Collections.Generic;

namespace UrlMin.Tests;

public class UrlControllerTests
{
    private readonly UrlController _controller;

    public UrlControllerTests()
    {
        _controller = new UrlController();
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithUrls()
    {
        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var urls = Assert.IsAssignableFrom<IEnumerable<Url>>(okResult.Value);
        Assert.NotEmpty(urls); // Should contain default Google URL
    }

    [Theory]
    [InlineData("http://www.example.com")]
    [InlineData("https://www.test.org")]
    public void Create_WithValidUrl_ReturnsCreatedAtAction(string longUrl)
    {
        // Act
        var result = _controller.Create(longUrl);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var url = Assert.IsType<Url>(createdAtActionResult.Value);
        Assert.Equal(longUrl, url.LongUrl);
        Assert.Equal(6, url.UrlRef.Length);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetByRef_WithInvalidRef_ReturnsNotFound(string urlRef)
    {
        // Act
        var result = _controller.GetByRef(urlRef);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetByRef_WithValidRef_ReturnsUrl()
    {
        // Arrange
        var createResult = _controller.Create("http://www.example.com");
        var createdUrl = (createResult.Result as CreatedAtActionResult)?.Value as Url;
        Assert.NotNull(createdUrl);

        // Act
        var result = _controller.GetByRef(createdUrl.UrlRef);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var url = Assert.IsType<Url>(okResult.Value);
        Assert.Equal(createdUrl.LongUrl, url.LongUrl);
    }

    [Fact]
    public void Update_WithValidUrl_ReturnsUpdatedUrl()
    {
        // Arrange
        var createResult = _controller.Create("http://www.example.com");
        var createdUrl = (createResult.Result as CreatedAtActionResult)?.Value as Url;
        Assert.NotNull(createdUrl);
        var updatedUrl = new Url(createdUrl.UrlRef, "http://www.updated.com");

        // Act
        var result = _controller.Update(updatedUrl);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var url = Assert.IsType<Url>(okResult.Value);
        Assert.Equal(updatedUrl.LongUrl, url.LongUrl);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Delete_WithInvalidRef_ReturnsNotFound(string urlRef)
    {
        // Act
        var result = _controller.Delete(urlRef);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_WithValidRef_ReturnsNoContent()
    {
        // Arrange
        var createResult = _controller.Create("http://www.example.com");
        var createdUrl = (createResult.Result as CreatedAtActionResult)?.Value as Url;
        Assert.NotNull(createdUrl);

        // Act
        var result = _controller.Delete(createdUrl.UrlRef);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
} 