using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagment.Shared.Models.Auth
{
    public record UserToReturn(
      string Id ,
     string FullName ,
      string PhoneNumber ,
      string Email ,
      string Types,
      string Token ,
     string RefreshToken,
     DateTime RefreshTokenExpirationDate );

    
}
