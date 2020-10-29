using LibraryApp.Infrastructure.ApiModel;
using LibraryApp.Infrastructure.Exceptions;
using LibraryApp.Infrastructure.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.MIddlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        private readonly IStringLocalizer<LocalizationResources> _localizer;


        public ExceptionMiddleware(
            RequestDelegate next,

            IStringLocalizer<LocalizationResources> stringLocalizer)
        {
            this._next = next;
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };


            _localizer = stringLocalizer;
        }


        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);

            }
            //add more catch expressions to handel specific exceptions
            catch (DbUpdateException ex)
            {
                //Log.Fatal("{0}", ex);


                string message = _localizer["Error.INTERNAL_ERROR.UserMessage"];
                var innerException = ex.InnerException;

                if (innerException != null)
                {
                    var humanizedMessage = GetReadableErrorMessage(innerException);
                    message = humanizedMessage ?? message;
                }

                LibraryErrorInformation errorInformation = new LibraryErrorInformation
                {
                    UserMessage = message,
                    InternalMessage = $"{ex.Message}, {ex.InnerException}, {ex.StackTrace}",
                    Code = LibraryErrorCodes.RESOURCE_NOT_FOUND
                };

                await SendRepsonse(LibraryResponse.CreateErrorResponse(HttpStatusCode.NotFound, errorInformation), httpContext);

            }
            catch (ResourceNotFoundException ex)
            {
                LibraryErrorInformation errorInformation = new LibraryErrorInformation
                {
                    UserMessage = _localizer["Error.INTERNAL_ERROR.ResourceNotFound"],
                    InternalMessage = _localizer["Error.INTERNAL_ERROR.ResourceNotFound"],
                    Code = LibraryErrorCodes.RESOURCE_NOT_FOUND
                };

                await SendRepsonse(LibraryResponse.CreateErrorResponse(HttpStatusCode.NotFound, errorInformation), httpContext);
            }
            catch (BusinessException ex)
            {
                LibraryErrorInformation errorInformation = new LibraryErrorInformation
                {
                    UserMessage = _localizer[ex.Message],
                    InternalMessage = _localizer[ex.Message],
                    Code = LibraryErrorCodes.BUSINESS_ERROR
                };
                await SendRepsonse(LibraryResponse.CreateErrorResponse(HttpStatusCode.BadRequest, errorInformation), httpContext);
            }

            catch (AuthenticationException ex)
            {
                LibraryErrorInformation errorInformation = new LibraryErrorInformation
                {
                    UserMessage = ex.Message,
                    InternalMessage = ex.Message,
                    Code = LibraryErrorCodes.AUTHENTICATION_FAILED
                };

                //Log.Fatal("{0}", ex);

                await SendRepsonse(LibraryResponse.CreateErrorResponse(HttpStatusCode.Unauthorized, errorInformation), httpContext);
            }
            catch (Exception ex)
            {
                LibraryErrorInformation errorInformation = new LibraryErrorInformation
                {
                    UserMessage = _localizer["Error.INTERNAL_ERROR.UserMessage"],
                    InternalMessage = _localizer["Error.INTERNAL_ERROR.InternalMessage"],
                    Code = LibraryErrorCodes.INTERNAL_ERROR
                };

                //Log.Fatal("{0}", ex);

                await SendRepsonse(LibraryResponse.CreateErrorResponse(HttpStatusCode.InternalServerError, errorInformation), httpContext);
            }
        }

        private async Task SendRepsonse(LibraryResponse response, HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)response.Code;
            httpContext.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(response, _jsonSerializerSettings);
            await httpContext.Response.WriteAsync(json);
        }


        private string GetReadableErrorMessage(Exception innerException)
        {

            return null;

        }
    }
}
