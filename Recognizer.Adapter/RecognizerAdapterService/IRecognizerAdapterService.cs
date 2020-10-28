using Recognizer.Adapter.Infrastructure.RequestResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recognizer.Adapter.RecognizerAdapterService
{
    public interface IRecognizerAdapterService
    {
        Task<RecognizerResponse> PostDataToRecognizerAPI(RecognizerRequest request);
    }
}
