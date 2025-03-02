using EventManagment.Apis.Extintions;
using EventManagment.Apis.MiddleWares;
using EventManagment.Core.Application;
using EventManagment.Infrastructure.Persistence;
using EventManagment.Shared;
using EventManagment.Shared.Errors.Response;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions((option) =>
    {
        option.SuppressModelStateInvalidFilter = false;
        option.InvalidModelStateResponseFactory = (action) =>
        {
            var errors = action.ModelState.
            Where(p => p.Value!.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage);

            return new BadRequestObjectResult(new ApiValidationErrorResponse() { Erroes = errors });
        };
    })
.AddApplicationPart(typeof(EventManagment.Apis.Controller.AssemblyInformation).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSharedDependency(builder.Configuration);

var app = builder.Build();
await app.InitializerEventManagmentContextAsync();
app.UseMiddleware<ExeptionHandlerMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/Errors/{0}");
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
