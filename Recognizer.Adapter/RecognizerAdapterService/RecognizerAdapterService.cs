using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recognizer.Adapter.Infrastructure.RequestResponse;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Recognizer.Adapter.RecognizerAdapterService
{
    public class RecognizerAdapterService : IRecognizerAdapterService
    {
        private readonly RecognizerSettings _recognizerSettings;
        static HttpClient client = new HttpClient();

        public RecognizerAdapterService(IOptions<RecognizerSettings> recognizerSettings)
        {
            _recognizerSettings = recognizerSettings.Value;
        }

        public async Task<RecognizerResponse> PostDataToRecognizerAPI(RecognizerRequest request)
        {
            StringContent jsonContent = SerializeObject(request);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _recognizerSettings.AuthHeader);

            HttpResponseMessage response = await client.PostAsync(_recognizerSettings.RecognizerUrl, jsonContent);

            var gotovo = DeserializeObject(response);

            return await gotovo;

        }

        private StringContent SerializeObject(RecognizerRequest content)
        {
            //Serialize Object
            string jsonObject = JsonConvert.SerializeObject(content);

            //Create Json UTF8 String Content
            return new StringContent(jsonObject, Encoding.UTF8, "application/json");
        }

        private async Task<RecognizerResponse> DeserializeObject(HttpResponseMessage response)
        {
            //Read body 
            string responseBody = await response.Content.ReadAsStringAsync();

            //Deserialize Body to object
            var result = JsonConvert.DeserializeObject<RecognizerResponse>(responseBody);

            return result;
        }
    }
}
