using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Core.Application.Abstraction.Services.Sponsers;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Sponsers;
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
    }
}
