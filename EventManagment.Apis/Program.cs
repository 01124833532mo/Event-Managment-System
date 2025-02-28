using EventManagment.Apis.Controller;
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
.AddApplicationPart(typeof(AssemblyInformation).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
