using LibraryApp.Model.Domain;
using Recognizer.Adapter.Infrastructure.RequestResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recognizer.Manager
{
    public interface IRecognizerManager
    {
        Task<User> PostData(string base64String);
    }
}
