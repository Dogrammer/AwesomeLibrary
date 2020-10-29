using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LibraryApp.Infrastructure.ApiModel
{

    public class LibraryGenericsResponse<T> where T : class
    {
        public HttpStatusCode Code { get; set; }
        public ICollection<LibraryErrorInformation> Errors { get; set; }
        public T Response { get; set; }

        public static LibraryGenericsResponse<T> CreateResponse(T data)
        {
            LibraryGenericsResponse<T> response = new LibraryGenericsResponse<T>();
            response.Response = data;

            return response;
        }

        public static LibraryGenericsResponse<T> CreateResponse(HttpStatusCode statusCode, T data)
        {
            var response = CreateResponse(data);
            response.Code = statusCode;
            return response;
        }

        public static LibraryGenericsResponse<T> CreateErrorResponse(HttpStatusCode statusCode, ICollection<LibraryErrorInformation> errors)
        {
            LibraryGenericsResponse<T> response = new LibraryGenericsResponse<T>();
            response.Code = statusCode;
            response.Errors = errors;

            return response;
        }

        public static LibraryGenericsResponse<T> CreateErrorResponse(HttpStatusCode statusCode, LibraryErrorInformation error)
        {
            ICollection<LibraryErrorInformation> errors = new List<LibraryErrorInformation>();
            errors.Add(error);

            return CreateErrorResponse(statusCode, errors);
        }
    }
}
