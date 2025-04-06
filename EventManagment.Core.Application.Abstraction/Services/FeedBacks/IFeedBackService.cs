using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.FeedBacks;
using System.Security.Claims;

namespace EventManagment.Core.Application.Abstraction.Services.FeedBacks
{
    public interface IFeedBackService
    {

        public Task<Response<string>> CreateFeedBack(ClaimsPrincipal claims, CreateFeedBackDto createFeedBackDto, CancellationToken cancellationToken);

    }
}
