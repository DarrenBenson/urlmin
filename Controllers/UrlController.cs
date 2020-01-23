using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlMin.Models;

namespace UrlMin.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("")]
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
        public ActionResult<List<Url>> GetAll() => _urlInMemoryStore;

        [HttpGet("{urlRef}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Url> GetByRef(string urlRef)
        {
            var url = _urlInMemoryStore.FirstOrDefault(p => p.UrlRef == urlRef);
            if (url == null)
            {
                return NotFound();
            }
            return url;
        }

        [HttpPost("{longUrl}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Url> Create(string longUrl)
        {
            var url = new Url(longUrl);
            _urlInMemoryStore.Add(url);
            return CreatedAtAction(nameof(GetByRef), new { urlRef = url.UrlRef }, url);
        }

        [HttpDelete("{urlRef}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Delete(string urlRef)
        {

            if (urlRef == null || urlRef=="")
            {
                return NotFound();
            }
            _urlInMemoryStore.Remove(_urlInMemoryStore.FirstOrDefault(p => p.UrlRef == urlRef));
            return Ok();
        }
    }
}
