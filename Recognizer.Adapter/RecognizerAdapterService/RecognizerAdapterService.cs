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
            // Test URI for OKP service
            //var url = _okpSettings.OKPTestUri;
            //MockyResponse mockyResponse = null;

            StringContent jsonContent = SerializeObject(request);

            var test23 = jsonContent.ReadAsStringAsync();


            //HttpResponseMessage response = await client.GetAsync(_mockySettings.MockyUri);
            //client.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", "Zjc1OWQ1NjM2NTMxNDVlYmFlZTZmMGU1NGJiYWQ4NWQ6N2FiNDZiYmItMDQ3ZS00MTM3LWFjNWYtM2I0ZGQ5ZWRjM2Q3");
            //HttpResponseMessage response = await client.PostAsync("https://api.microblink.com/v1/recognizers/mrtd", jsonContent);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _recognizerSettings.AuthHeader);

            HttpResponseMessage response = await client.PostAsync(_recognizerSettings.RecognizerUrl, jsonContent);

            var gotovo = DeserializeObject(response);
            //var adjdevise = gotovo.


            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            //if (response.IsSuccessStatusCode)
            //{
            //    mockyResponse = JsonConvert.DeserializeObject<MockyResponse>(responseBody);
            //}

            //mockyResponse.HttpResponse = "StatusCode = " + response.StatusCode.ToString() +
            //                             ", ReasonPhrase = " + response.ReasonPhrase.ToString() +
            //                             ", Headers = " + response.Headers.ToString();



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
