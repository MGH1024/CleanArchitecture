using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Services.IdentityProvider.DTOs.User
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string RefToken { get; set; }
    }
}
