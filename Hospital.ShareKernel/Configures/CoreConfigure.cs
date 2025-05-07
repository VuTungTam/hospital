using System.Globalization;
using System.Text;
using System.Text.Json;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Caching.In_Memory;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.CoreConfigs;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Infrastructure.Behaviors;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Services.Cloud;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Middlewares;
using Hospital.SharedKernel.Presentations.SignalR;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Hospital.SharedKernel.Runtime.Filters;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using StackExchange.Redis;

namespace Hospital.SharedKernel.Configures
{
    public static class CoreConfigure
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton(_ => Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                x.MultipartHeadersLengthLimit = int.MaxValue;
                x.ValueCountLimit = int.MaxValue;
            });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            //services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            //services.AddResponseCompression(options =>
            //{
            //    options.EnableForHttps = true;
            //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            //    options.Providers.Add<GzipCompressionProvider>();
            //});

            services.AddCors();

            services.AddCoreLocalization();

            services.AddCoreRateLimit();

            services.AddCoreBehaviors();

            services.AddCoreApiVersioning();

            services.AddScoped<IDateService, DateService>();

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<HideOcelotControllersFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            #region AddController + CamelCase + FluentValidation
            services.AddControllersWithViews()
                    .AddNewtonsoftJson()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    })
                    .AddFluentValidation(delegate (FluentValidationMvcConfiguration f)
                    {
                        f.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic));
                        //f.RegisterValidatorsFromAssembly(Assembly.GetEntryAssembly());
                    })
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
            #endregion

            return services;
        }

        public static IServiceCollection AddCoreRateLimit(this IServiceCollection services)
        {
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.RealIpHeader = HeaderNamesExtension.RealIpHeader;
                options.ClientIdHeader = HeaderNamesExtension.ClientIdHeader;
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 5,
                    }
                };
            });

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();

            return services;
        }

        public static IServiceCollection AddCoreApiVersioning(this IServiceCollection services)
        {
            return services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        }

        public static IServiceCollection AddCoreAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration["Auth:JwtSettings:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Auth:JwtSettings:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:JwtSettings:SecretKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                jwtOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/subscribe"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
        public static IServiceCollection AddCoreLocalization(this IServiceCollection services)
        {
            var supportedCultures = new List<CultureInfo> { new("en-US"), new("vi-VN") };
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };

            });

            return services;
        }

        public static IServiceCollection AddCoreExecutionContext(this IServiceCollection services)
        {
            return services.AddScoped<IExecutionContext, Runtime.ExecutionContext.ExecutionContext>();
        }

        public static IServiceCollection AddCoreBehaviors(this IServiceCollection services)
        {
            return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        }
        #region Middlewares
        #endregion
        public static void UseCoreLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
        public static IServiceCollection AddCoreCache(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { $"{RedisConfig.Host}:{RedisConfig.Port}" },
                    Password = RedisConfig.Password,
                    DefaultDatabase = RedisConfig.DbNumber,
                    ConnectTimeout = RedisConfig.Timeout
                };
                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            return services;
        }

        public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            return services;
        }

        public static void UseCoreCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var origins = configuration.GetRequiredSection("Allowedhosts").Value;
            if (origins.Equals("*"))
            {
                app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }
            else
            {
                app.UseCors(x => x.WithOrigins(origins.Split(";")).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            }
        }

        public static void UseCoreConfigure(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseCoreLocalization();
            app.UseCoreExceptionHandler();

            if (ElasticSearchConfig.Enabled)
            {
                app.UseCoreLogRequestBody();
                app.UseSerilogRequestLogging(options =>
                {
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        var ec = httpContext.RequestServices.GetRequiredService<IExecutionContext>();
                        diagnosticContext.Set("TraceId", ec.TraceId);
                        diagnosticContext.Set("IP", AuthUtility.TryGetIP(httpContext.Request));
                        diagnosticContext.Set("Origin", httpContext.Request.Headers[HeaderNames.Origin]);
                        diagnosticContext.Set("QueryString", httpContext.Request.QueryString);
                        diagnosticContext.Set("Header", JsonConvert.SerializeObject(httpContext.Request.Headers));
                        diagnosticContext.Set("Uid", httpContext.Request.Headers[HeaderNamesExtension.Uid]);
                        diagnosticContext.Set("Screen", httpContext.Request.Headers[HeaderNamesExtension.Screen]);
                        diagnosticContext.Set("DeviceType", httpContext.Request.Headers[HeaderNamesExtension.DeviceType]);
                        diagnosticContext.Set("PagePath", httpContext.Request.Headers[HeaderNamesExtension.PagePath]);

                        if (!ec.IsAnonymous)
                        {
                            diagnosticContext.Set("UserId", ec.Identity.ToString());
                            diagnosticContext.Set("Email", ec.Email);
                        }
                        else
                        {
                            diagnosticContext.Set("IsAnonymous", true);
                        }
                    };
                });
            }

            app.UseIpRateLimiting();
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRHub>("/subscribe");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCoreSwagger();
            // app.UseCoreHealthChecks();
        }
        public static void UseCoreSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
                c.RoutePrefix = "swagger";
            });
        }
        public static void UseCoreExceptionHandler(this IApplicationBuilder app)
        {
            // Handle exception
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();
                context.Response.ContentType = "application/json";

                // Unauthorize
                if (exception is UnauthorizeException)
                {
                    context.Response.StatusCode = 401;
                    var body = new
                    {
                        Message = localizer["auth_unauthorized"].Value,
                        Data = (exception as UnauthorizeException).DataException
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(body, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
                    return;
                }

                // For bi đần
                if (exception is ForbiddenException)
                {
                    context.Response.StatusCode = 403;

                    var body = new
                    {
                        Message = string.IsNullOrEmpty(exception.Message) ? localizer["auth_not_permission"].Value : exception.Message
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(body, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));

                    if (!string.IsNullOrEmpty(exception.Message))
                    {
                        Log.Logger.Debug("[403]: {Message}", exception.Message);
                    }
                    Log.Logger.Debug("[403] {Stacktrace}: ", exception.StackTrace);

                    return;
                }

                var responseContent = new BaseResponse();
                // Catchable
                if (exception is CatchableException)
                {
                    context.Response.StatusCode = 500;
                    responseContent.Code = ErrorCodeConstant.SERVER_ERROR;
                    responseContent.Message = exception.Message;
                    Log.Logger.Error(exception, exception.Message);
                }
                // Sql Injection
                else if (exception is SqlInjectionException)
                {
                    context.Response.StatusCode = 400;
                    responseContent.Code = ErrorCodeConstant.SQL_INJECTOR_DETECTED;
                    responseContent.Message = Secure.MsgDetectedSqlInjection;
                    Log.Logger.Error(exception, exception.Message);
                }
                // Bad request
                else if (exception is BadRequestException)
                {
                    context.Response.StatusCode = 400;
                    if ((exception as BadRequestException).Body != null)
                    {
                        responseContent = new SimpleDataResult
                        {
                            Data = (exception as BadRequestException).Body,
                            Code = (exception as BadRequestException).Code,
                            Message = exception.Message
                        };
                    }
                    else
                    {
                        responseContent.Code = (exception as BadRequestException).Code;
                        responseContent.Message = exception.Message;
                    }
                    Log.Logger.Debug("[CE]: {Message}", exception.Message);
                }
                // Unknown exception
                else
                {
                    context.Response.StatusCode = 500;
                    responseContent.Code = ErrorCodeConstant.UNKNOWN_ERROR;
                    responseContent.Message = InfrastructureConfiguration.EnabledShowError ? exception.Message : localizer["common_system_error_occurred"].Value;
                    Log.Logger.Error(exception, exception.Message);
                }
                await context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            }));
        }
    }
}