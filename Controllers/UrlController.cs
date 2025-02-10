using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlMin.Models;

namespace UrlMin.Controllers;

/// <summary>
/// Controller for managing shortened URLs
/// </summary>
[Produces(MediaTypeNames.Application.Json)]
[Route("api")]
public class UrlController : MyControllerBase
{
    private static readonly List<Url> _urlStore = new();

    /// <summary>
    /// Initializes the controller with a default URL if store is empty
    /// </summary>
    public UrlController()
    {
        if (!_urlStore.Any())
        {
            _urlStore.Add(new Url("http://www.google.com"));
        }
    }

    /// <summary>
    /// Retrieves all stored URLs
    /// </summary>
    /// <returns>Collection of all URLs</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Url>> GetAll() => Ok(_urlStore);

    /// <summary>
    /// Retrieves a specific URL by its reference
    /// </summary>
    /// <param name="urlRef">The URL reference to look up</param>
    /// <returns>The matching URL or 404 if not found</returns>
    [HttpGet("{urlRef}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Url> GetByRef(string urlRef)
    {
        var url = _urlStore.FirstOrDefault(p => p.UrlRef == urlRef);
        return url is null ? NotFound() : Ok(url);
    }

    /// <summary>
    /// Updates an existing URL
    /// </summary>
    /// <param name="url">The URL to update</param>
    /// <returns>The updated URL or 400 if not found</returns>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Update(Url url)
    {
        var originalUrl = _urlStore.Find(item => item.UrlRef == url.UrlRef);
        if (originalUrl is null) 
            return BadRequest("Cannot update a non-existing URL.");
        
        originalUrl.LongUrl = url.LongUrl;
        return Ok(originalUrl);
    }

    /// <summary>
    /// Creates a new shortened URL
    /// </summary>
    /// <param name="longUrl">The original URL to shorten</param>
    /// <returns>The newly created shortened URL</returns>
    [HttpPost("{longUrl}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Url> Create(string longUrl)
    {
        var url = new Url(longUrl);
        _urlStore.Add(url);
        return CreatedAtAction(nameof(GetByRef), new { urlRef = url.UrlRef }, url);
    }

    /// <summary>
    /// Deletes a URL by its reference
    /// </summary>
    /// <param name="urlRef">The URL reference to delete</param>
    /// <returns>204 No Content on success, 404 if not found</returns>
    [HttpDelete("{urlRef}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult Delete(string urlRef)
    {
        if (string.IsNullOrWhiteSpace(urlRef))
        {
            return NotFound();
        }
        
        _urlStore.RemoveAll(p => p.UrlRef == urlRef);
        return NoContent();
    }
}
