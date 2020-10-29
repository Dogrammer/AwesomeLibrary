using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LibraryApp.Infrastructure.ApiModel
{
    public class LibraryResponse
    {
        public HttpStatusCode Code { get; set; }
        public ICollection<LibraryErrorInformation> Errors { get; set; }
        public object Response { get; set; }

        public static LibraryResponse CreateResponse(object data)
        {
            LibraryResponse response = new LibraryResponse();
            response.Response = data;

            return response;
        }

        public static LibraryResponse CreateResponse(HttpStatusCode statusCode, object data)
        {
            var response = CreateResponse(data);
            response.Code = statusCode;
            return response;
        }

        public static LibraryResponse CreateResponse(HttpStatusCode statusCode, string message)
        {
            var response = CreateResponse(message);
            response.Code = statusCode;
            return response;
        }

        public static LibraryResponse CreateErrorResponse(HttpStatusCode statusCode, ICollection<LibraryErrorInformation> errors)
        {
            LibraryResponse response = new LibraryResponse();
            response.Code = statusCode;
            response.Errors = errors;

            return response;
        }

        public static LibraryResponse CreateErrorResponse(HttpStatusCode statusCode, LibraryErrorInformation error)
        {
            ICollection<LibraryErrorInformation> errors = new List<LibraryErrorInformation>();
            errors.Add(error);

            return CreateErrorResponse(statusCode, errors);
        }

    }
}
