using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHMASTER.CORE.Attributes
{
    public class TokenObject
    {
        public TokenUserObject User { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string RawToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class TokenUserObject
    {
        public Guid Id { get; set; }
        public string Role { get;set; }
        public string Username { get; set; }
        public string Password { get; set; }    
        public string Email { get; set; }
    }
}
