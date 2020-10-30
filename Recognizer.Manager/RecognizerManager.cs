using LibraryApp.Model.Domain;
using onMRZ;
using Recognizer.Adapter.Infrastructure.RequestResponse;
using Recognizer.Adapter.RecognizerAdapterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recognizer.Manager
{
    public class RecognizerManager : IRecognizerManager
    {
        private readonly IRecognizerAdapterService _recognizerAdapterService;

        public RecognizerManager(IRecognizerAdapterService recognizerAdapterService)
        {
            _recognizerAdapterService = recognizerAdapterService;
        }

        public async Task<User> PostData(string base64String)
        {
            var newUser = new User();
            newUser.IsValid = false;

            // populate request for microblink cloud API
            var request = new RecognizerRequest()
            {
                traceId = "string",
                ageLimit = 0,
                allowBlurFilter = false,
                allowUnparsedMrzResults = false,
                allowUnverifiedMrzResults = true,
                anonymizationMode = "FULL_RESULT",
                anonymizeImage = true,
                returnFaceImage = false,
                returnFullDocumentImage = false,
                returnSignatureImage = false,
                validateResultCharacters = true,
                imageSource = base64String
            };

            // send request to adapter which will send data to microblink Cloud API
            var response = await _recognizerAdapterService.PostDataToRecognizerAPI(request);

            if (response.Result != null)
            {
                // if response is valid call the parse method from MrzParse class
                var newParse = new MrzParser();
                
                var parsed = newParse.Parse(response.Result.MrzData.RawMrzString);

                // populate user data with parsed rawmrzstring
                newUser.DateOfBirth = parsed.DateOfBirth;
                newUser.FirstName = parsed.FirstName;
                newUser.LastName = parsed.LastName;
                newUser.IsActive = true;
                newUser.IsDeleted = false;
                newUser.IsValid = true;

            }

            return newUser;
        }
    }
}
