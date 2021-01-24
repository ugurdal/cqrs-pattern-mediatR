using System;

namespace Models.Token
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime ValidThrough { get; set; }
    }
}