using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlMin.Models;

namespace UrlMin.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api")]
    public class UrlController : MyControllerBase
    {
        private static readonly List<Url> _urlInMemoryStore = new List<Url>();

        public UrlController()
        {
            if (_urlInMemoryStore.Count == 0)
            {
                _urlInMemoryStore.Add(new Url("http://www.google.com"));
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Url>> GetAll() => Ok(_urlInMemoryStore);

        [HttpGet("{urlRef}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Url> GetByRef(string urlRef)
        {
            var url = _urlInMemoryStore.FirstOrDefault(p => p.UrlRef == urlRef);
            if (url == null)
            {
                return NotFound();
            }
            return Ok(url);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Put(Url url)
        {
            var originalUrl = _urlInMemoryStore.Find(item => item.UrlRef.Equals(url.UrlRef));
            if (originalUrl == null)
            {
                return BadRequest("Cannot update a none existing url.");
            } else
            {
                originalUrl.LongUrl = url.LongUrl;
                return Ok();
            }
        }

        [HttpPost("{longUrl}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Url> Create(string longUrl)
        {
            var url = new Url(longUrl);
            _urlInMemoryStore.Add(url);
            return CreatedAtAction(nameof(GetByRef), new { urlRef = url.UrlRef }, url);
        }

        [HttpDelete("{urlRef}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Delete(string urlRef)
        {

            if (urlRef == null || urlRef=="")
            {
                return NotFound();
            }
            _urlInMemoryStore.Remove(_urlInMemoryStore.FirstOrDefault(p => p.UrlRef == urlRef));
            return NoContent();
        }
    }
}
