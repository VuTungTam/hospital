using Hospital.Application.DI;
using Hospital.Infrastructure.DI;
using Hospital.SharedKernel.Configures;
using FluentValidation.AspNetCore;
using System.Reflection;
using Hospital.Application.Dtos.SocialNetworks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Constants;
namespace Hospital.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews()
                            .AddFluentValidation(
                                c => c.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic))
                            )
                            .ConfigureApiBehaviorOptions(delegate (ApiBehaviorOptions options)
                            {
                                options.InvalidModelStateResponseFactory = delegate (ActionContext c)
                                {
                                    var errors = from v in c.ModelState.Values.Where((v) => v.Errors.Any()).SelectMany((v) => v.Errors) select v.ErrorMessage;
                                    var msg = string.Join(", ", errors.Distinct());

                                    if (msg.Contains("line") && msg.Contains("position"))
                                    {
                                        msg = "Dữ liệu không hợp lệ";
                                    }
                                    return new BadRequestObjectResult(new BaseResponse(ErrorCodeConstant.BAD_REQUEST, msg));
                                };
                            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            //builder.Services.AddValidatorsFromAssemblyContaining<SocialNetworkDtoValidator>();

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddInfrastructureService(builder.Configuration);

            builder.Services.AddCoreService(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            app.UseCoreLocalization();

            // Configure the HTTP request pipeline.
            app.UseCors("AllowAllOrigins");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            // app.UseCoreConfigure(app.Environment);
            app.Run();
        }
    }
}
