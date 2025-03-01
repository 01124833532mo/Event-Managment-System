using EventManagment.Core.Application.Abstraction.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagment.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }
    }
}
