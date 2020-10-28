using LibraryApp.Model.Domain;
using onMRZ;
using Recognizer.Adapter.Infrastructure.RequestResponse;
using Recognizer.Adapter.RecognizerAdapterService;
using System;
using System.Collections.Generic;
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
            //tu puni Recognizer request i šalji na adapter i vrati nazad response
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

            var response = await _recognizerAdapterService.PostDataToRecognizerAPI(request);

            if (response.Result != null)
            {

                //var firstLine = response.Result.MrzData.RawMrzString.Substring(0, Math.Min(response.Result.MrzData.RawMrzString.Length, 30));
                //var gg = Regex.Match(firstLine, @"([0 - 9]{ 6})([0 - 9]{ 1})([M | F | X |<]{ 1})([0 - 9]{ 6})([0 - 9]{ 1})([A - Z]{ 3})([A - Z0 - 9 <]{ 11})([0 - 9]{ 1})");
                //string[] digits = Regex.Split(firstLine, "([0 - 9]{ 6})([0 - 9]{ 1})([M | F | X |<]{ 1})([0 - 9]{ 6})([0 - 9]{ 1})([A - Z]{ 3})([A - Z0 - 9 <]{ 11})([0 - 9]{ 1})");
                //([0 - 9]{ 6})([0 - 9]{ 1})([M | F | X |<]{ 1})([0 - 9]{ 6})([0 - 9]{ 1})([A - Z]{ 3})([A - Z0 - 9 <]{ 11})([0 - 9]{ 1})

                var newParse = new MrzParser();
                var parsed = newParse.Parse(response.Result.MrzData.RawMrzString);

                newUser.DateOfBirth = parsed.DateOfBirth;
                newUser.FirstName = parsed.FirstName;
                newUser.LastName = parsed.LastName;
                newUser.IsActive = true;
                newUser.IsDeleted = false;
                
                //sad kada imamo sve potrebne podatke trebamo spremiti u bazu podataka
                // spremi firstname,lastname, dateofbirth, isValid, 


            }
            //tu ide sva logika oko validacije itd


            return newUser;
        }
    }
}
