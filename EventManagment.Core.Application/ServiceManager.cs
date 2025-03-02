using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagment.Core.Application
{
    public class ServiceManager : IServiceManager
    {

        private readonly Lazy< IAuthService> _authService;

        public ServiceManager(Func<IAuthService> authfactory)
        {
            _authService = new Lazy<IAuthService>(authfactory,LazyThreadSafetyMode.ExecutionAndPublication);
            
        }


        public IAuthService AuthService => _authService.Value;
    }
}
