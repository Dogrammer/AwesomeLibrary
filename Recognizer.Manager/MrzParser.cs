using Recognizer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace onMRZ
{
    public class MrzParser
    {
        //private readonly Nationalities _nationalities = new Nationalities();


        private readonly Dictionary<char, int> _checkDigitArray = new Dictionary<char, int>();

        public ParseUserData Parse(string mrz)
        {
            var validationMessage = MRZValidationMessage(mrz);
            if (!string.IsNullOrEmpty(validationMessage)) return new ParseUserData { IsValid = false, ValidationMessage = validationMessage };

            
            var output = new ParseUserData();
            output.DocumentType = DocumentType(mrz);
            output.Gender = Gender(mrz);
            output.ExpireDate = ExpireDate(mrz);
            //output.IssuingCountryIso = IssuingCountryIso(mrz);
            output.FirstName = FirstName(mrz);
            output.LastName = LastName(mrz);
            output.DocumentNumber = DocumentNumber(mrz);
            //output.NationalityIso = NationalityIso(mrz);
            output.DateOfBirth = DateOfBirth(mrz);

            return output;
        }

        private string MRZValidationMessage(string mrz)
        {
            if (string.IsNullOrEmpty(mrz)) return "Empty MRZ";
            if (mrz.Length < 90) return $"MRZ length is not valid should be 90 but it is {mrz.Length}";
            if (mrz.Substring(0, 1) != "A" && mrz.Substring(0, 1) != "C" && mrz.Substring(0, 1) != "I") return $"Document Type should either be A, C or I";

            return string.Empty;
        }

        private string DocumentType(string mrz)
        {
            var str = mrz.Substring(0, 2);
            if (str[1 ]== '<')
            {
                return str[0].ToString();
            }

            return str;

        }

        private string IssuingCountryIso(string mrz)
        {
            return mrz.Substring(2, 3);

        }

        //private string IssuingCountryName(string issIso)
        //{
        //    //var natItem = _nationalities.NationalitybyCode(issIso);
        //    return natItem != null ? natItem.Name : string.Empty;

        //}

        private string FirstName(string mrz)
        {
            var nameArraySplit = mrz.Substring(62).Split(new[] { "<<" }, StringSplitOptions.RemoveEmptyEntries);
            return nameArraySplit.Length >= 2 ? nameArraySplit[1].Replace("<", " ") : nameArraySplit[0].Replace("<", " ");

        }

        private string LastName(string mrz)
        {

            var nameArraySplit = mrz.Substring(62).Split(new[] { "<<" }, StringSplitOptions.RemoveEmptyEntries);
            return nameArraySplit.Length >= 2 ? nameArraySplit[0].Replace("<", " ") : string.Empty;

        }

        private string DocumentNumber(string mrz)
        {
            var documentNumberStart = mrz.Substring(5, 30);
            var documentNumber = documentNumberStart.Substring(0, documentNumberStart.IndexOf('<') -1);


            return documentNumber;

        }

        private string NationalityIso(string mrz)
        {
            return mrz.Substring(10 + 44, 3);

        }

        //private string NationalityName(string natIso)
        //{
        //    var natItem = _nationalities.NationalitybyCode(natIso);
        //    return natItem != null ? natItem.Name : string.Empty;

        //}

        private DateTime DateOfBirth(string mrz)
        {
            var dob = new DateTime(int.Parse(DateTime.Now.Year.ToString().Substring(0, 2) + mrz.Substring(30 + 1, 2)), int.Parse(mrz.Substring(30 + 3, 2)),
                    int.Parse(mrz.Substring(30 + 5, 2)));

            if (dob < DateTime.Now)
                return dob;

            return dob.AddYears(-100); //Subtract a century

        }



        private string Gender(string mrz)
        {
            var str = mrz.Substring(38, 1);
            return str;

        }

        private DateTime ExpireDate(string mrz)
        {
            //I am assuming all passports will certainly expire this century
            return new DateTime(int.Parse(DateTime.Now.Year.ToString().Substring(0, 2) + mrz.Substring(30 + 9, 2)), int.Parse(mrz.Substring(30 + 11, 2)),
                int.Parse(mrz.Substring(30 + 13, 2)));

        }

        internal string CheckDigit(string icaoPassportNumber)
        {
            if (!_checkDigitArray.Any())
                FillCheckDigitDictionary();
            icaoPassportNumber = icaoPassportNumber.ToUpper();
            var inputArray = icaoPassportNumber.Trim().ToCharArray();
            var multiplier = 7;
            var total = 0;
            foreach (var dig in inputArray)
            {
                total = total + _checkDigitArray[dig] * multiplier;
                if (multiplier == 7) multiplier = 3;
                else if (multiplier == 3) multiplier = 1;
                else if (multiplier == 1) multiplier = 7;
            }

            long result;
            Math.DivRem(total, 10, out result);
            return result.ToString();
        }

        private void FillCheckDigitDictionary()
        {
            _checkDigitArray.Add('<', 0);
            _checkDigitArray.Add('0', 0);
            _checkDigitArray.Add('1', 1);
            _checkDigitArray.Add('2', 2);
            _checkDigitArray.Add('3', 3);
            _checkDigitArray.Add('4', 4);
            _checkDigitArray.Add('5', 5);
            _checkDigitArray.Add('6', 6);
            _checkDigitArray.Add('7', 7);
            _checkDigitArray.Add('8', 8);
            _checkDigitArray.Add('9', 9);
            _checkDigitArray.Add('A', 10);
            _checkDigitArray.Add('B', 11);
            _checkDigitArray.Add('C', 12);
            _checkDigitArray.Add('D', 13);
            _checkDigitArray.Add('E', 14);
            _checkDigitArray.Add('F', 15);
            _checkDigitArray.Add('G', 16);
            _checkDigitArray.Add('H', 17);
            _checkDigitArray.Add('I', 18);
            _checkDigitArray.Add('J', 19);
            _checkDigitArray.Add('K', 20);
            _checkDigitArray.Add('L', 21);
            _checkDigitArray.Add('M', 22);
            _checkDigitArray.Add('N', 23);
            _checkDigitArray.Add('O', 24);
            _checkDigitArray.Add('P', 25);
            _checkDigitArray.Add('Q', 26);
            _checkDigitArray.Add('R', 27);
            _checkDigitArray.Add('S', 28);
            _checkDigitArray.Add('T', 29);
            _checkDigitArray.Add('U', 30);
            _checkDigitArray.Add('V', 31);
            _checkDigitArray.Add('W', 32);
            _checkDigitArray.Add('X', 33);
            _checkDigitArray.Add('Y', 34);
            _checkDigitArray.Add('Z', 35);
        }
    }
}