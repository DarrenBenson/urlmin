using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace UrlMin.Models;

/// <summary>
/// Represents a shortened URL with its reference and original long URL
/// </summary>
public class Url
{
    private static readonly Random _random = new();

    /// <summary>
    /// Creates a new URL with a randomly generated reference
    /// </summary>
    /// <param name="longUrl">The original URL to shorten</param>
    public Url(string longUrl)
    {
        UrlRef = GenerateRandomString(6);
        LongUrl = longUrl;
    }

    /// <summary>
    /// Creates a new URL with a specified reference
    /// </summary>
    /// <param name="urlRef">The reference to use</param>
    /// <param name="longUrl">The original URL</param>
    public Url(string urlRef, string longUrl)
    {
        UrlRef = urlRef;
        LongUrl = longUrl;
    }

    /// <summary>
    /// Gets the shortened URL reference
    /// </summary>
    [Required]
    public string UrlRef { get; init; }

    /// <summary>
    /// Gets or sets the original long URL
    /// </summary>
    [Required]
    [Url]
    public string LongUrl { get; set; }

    /// <summary>
    /// Generates a random string for use as a URL reference
    /// </summary>
    /// <param name="length">The length of the string to generate</param>
    /// <returns>A random string of the specified length</returns>
    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)])
            .ToArray());
    }
}
