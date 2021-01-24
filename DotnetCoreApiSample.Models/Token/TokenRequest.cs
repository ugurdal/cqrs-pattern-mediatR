using System.ComponentModel.DataAnnotations;

namespace Models.Token
{
    public class TokenRequest
    {
        
        [Required]
        [Range(1, int.MaxValue)]
        public int AccountId { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string GrantType { get; set; }
    }
}