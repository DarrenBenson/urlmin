using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace UrlMin.Models
{
    public class Url
    {
        private static Random _random = new Random();        

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
        public Url(string urlRef, string longUrl){
            this.UrlRef = urlRef;
            this.LongUrl = longUrl;
        }

       public Url(string longUrl){
            this.UrlRef = RandomString(6);
            this.LongUrl = longUrl;
        }

        [Required]
        public string UrlRef { get; set; }

        [Required]
        public string LongUrl { get; set; }

    }
}
