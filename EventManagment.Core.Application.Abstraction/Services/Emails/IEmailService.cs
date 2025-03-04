using EventManagment.Shared.Models._Common.Emails;

namespace EventManagment.Core.Application.Abstraction.Services.Emails
{
    public interface IEmailService
    {
        public Task SendEmail(Email email);
    }
}
