using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Sesstions;

namespace EventManagment.Core.Application.Abstraction.Services.Sesstions
{
    public interface ISesstionService
    {

        public Task<Response<SesstionToreturn>> CreateSesstionAsync(SesstionDto sesstionDto, CancellationToken cancellationToken);
        public Task<Response<SesstionToreturn>> GetSesstionAsync(int id, CancellationToken cancellationToken);
        public Task<Response<string>> DeleteSesstionAsync(int id, CancellationToken cancellationToken);

        public Task<Pagination<SesstionToreturn>> GetAllSessionAsync(SpecParams specParams, CancellationToken cancellationToken);



    }
}
