using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.FeedBacks;
using System.Security.Claims;

namespace EventManagment.Core.Application.Abstraction.Services.FeedBacks
{
    public interface IFeedBackService
    {

        public Task<Response<string>> CreateFeedBack(ClaimsPrincipal claims, CreateFeedBackDto createFeedBackDto, CancellationToken cancellationToken);

        public Task<Response<string>> RemoveFeedBack(int id, CancellationToken cancellationToken);

        public Task<Pagination<FeedBackToRetuen>> GetAllFeedBack(SpecParams specParams, CancellationToken cancellationToken);

    }
}
