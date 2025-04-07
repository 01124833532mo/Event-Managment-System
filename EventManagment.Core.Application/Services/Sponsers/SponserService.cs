using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Core.Application.Abstraction.Services.Sponsers;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Sponsers;
using EventManagment.Core.Domain.Specifications.Sponsers;
using EventManagment.Shared.Models.Sponsers;

namespace EventManagment.Core.Application.Services.Sponsers
{
    public class SponserService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService) : ResponseHandler, ISponserService
    {
        public async Task<Response<SponserToReturn>> CreateSponserAsync(CreateSponserDto createSponserDto, CancellationToken cancellationToken = default)
        {
            var sponserRepository = unitOfWork.GetRepository<Sponser, int>();
            var sponser = mapper.Map<Sponser>(createSponserDto);

            if (createSponserDto.LogoUrl is not null)
            {
                var uploadedImageUrl = await attachmentService.UploadAsynce(createSponserDto.LogoUrl, "SponserLogoes");

                if (uploadedImageUrl is not null)
                {
                    sponser.LogoUrl = uploadedImageUrl;
                }
                else
                {
                    sponser.LogoUrl = null;
                }
            }
            try
            {

                await sponserRepository.AddAsync(sponser);
            }
            catch (Exception ex)
            {
                return BadRequest<SponserToReturn>(ex.Message);
            }

            var complet = await unitOfWork.CompleteAsync() > 0;
            if (!complet)
            {
                return BadRequest<SponserToReturn>("Failed to create sponser");
            }


            var returnedservice = mapper.Map<SponserToReturn>(sponser);


            //var returnedservice = new SponserToReturn()
            //{
            //    Description = sponser.Description,
            //    Id = sponser.Id,
            //    LogoUrl = sponser.LogoUrl,
            //    Name = sponser.Name,
            //    Website = sponser.Website

            //};
            return Success(returnedservice);
        }

        public async Task<Response<string>> DeleteSponser(int id, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.GetRepository<Sponser, int>();

            var sponser = await repo.GetAsync(id, cancellationToken);
            if (sponser is null)
                return NotFound<string>(id, "sponser not found");

            try
            {
                repo.Delete(sponser);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }

            var result = await unitOfWork.CompleteAsync() > 0;
            if (!result)
                return BadRequest<string>("Failed to remove sponser");

            return Success("Succssfully Remove sponser");
        }

        public async Task<Pagination<SponserToReturn>> GetAllSponserAsync(SpecParams specParams, CancellationToken cancellationToken)
        {

            var spec = new GetAllSponserSpecification(specParams.PageSize, specParams.PageIndex);

            var sponsers = await unitOfWork.GetRepository<Sponser, int>().GetAllWithSpecAsync(spec);
            var data = mapper.Map<IEnumerable<SponserToReturn>>(sponsers);
            var count = sponsers.Count();
            return new Pagination<SponserToReturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };
        }

        public async Task<Response<SponserToReturn>> GetSponserAsync(int id, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.GetRepository<Sponser, int>();
            var spec = new GetAllSponserSpecification(id);
            var sponser = await repo.GetWithSpecAsync(spec, cancellationToken);
            if (sponser is null)
            {
                return NotFound<SponserToReturn>(id, "Sponser Not Found With This Id");
            };
            var mappedSponser = mapper.Map<SponserToReturn>(sponser);
            return Success(mappedSponser, 1);
        }
    }
}
