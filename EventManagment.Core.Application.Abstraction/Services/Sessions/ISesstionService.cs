using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Sesstions;

namespace EventManagment.Core.Application.Abstraction.Services.Sesstions
{
    public interface ISesstionService
    {

        public Task<Response<SesstionToreturn>> CreateSesstionAsync(SesstionDto sesstionDto, CancellationToken cancellationToken);


    }
}
