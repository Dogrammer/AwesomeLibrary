﻿using Recognizer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace onMRZ
{
    public class MrzParser
    {
        private readonly Nationalities _nationalities = new Nationalities();


        private readonly Dictionary<char, int> _checkDigitArray = new Dictionary<char, int>();

        //Parsing is based on https://en.wikipedia.org/wiki/Machine-readable_passport
        //Useful information https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf

        public ParseUserData Parse(string mrz)
        {
            var validationMessage = MRZValidationMessage(mrz);
            if (!string.IsNullOrEmpty(validationMessage)) return new ParseUserData { IsValid = false, ValidationMessage = validationMessage };

            //var output = new ParseUserData
            //{
            //    DocumentType = DocumentType(mrz),
            //    Gender = Gender(mrz),
            //    ExpireDate = ExpireDate(mrz),
            //    IssuingCountryIso = IssuingCountryIso(mrz),
            //    FirstName = FirstName(mrz),
            //    LastName = LastName(mrz),
            //    DocumentNumber = DocumentNumber(mrz),
            //    NationalityIso = NationalityIso(mrz),
            //    DateOfBirth = DateOfBirth(mrz)
            //};
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



            //output.Gender =


            //end

            //output.DocumentTypeDescription = DocumentTypeDescription(output.DocumentType);

            //output.IssuingCountryName = IssuingCountryName(output.IssuingCountryIso);

            //output.FullName = (output.FirstName + " " + output.LastName).Replace("  ", " ").Trim();

            //output.NationalityName = NationalityName(output.NationalityIso);

            //output.Age = (int)(DateTime.Now.Subtract(output.DateOfBirth).TotalDays / 365);

            //output.IssueDate = IssueDate(output.ExpireDate, output.NationalityIso);
            //output.IssuingAuthority = IssuingAuthority(output.IssuingCountryIso);
            //output.PlaceOfBirth = PlaceOfBirth(output.NationalityIso);

            return output;
        }

        private string MRZValidationMessage(string mrz)
        {
            if (string.IsNullOrEmpty(mrz)) return "Empty MRZ";
            if (mrz.Length < 88) return $"MRZ length is not valid should be 88 but it is {mrz.Length}";
            //if (mrz.Substring(0, 1) != "P" && mrz.Substring(0, 1) != "V" && mrz.Substring(0, 1) != "C") return $"Document Type should either be P, V or C";

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

        private string DocumentTypeDescription(string docType)
        {
            switch (docType)
            {
                case "P":
                    return "Passport";
                case "C":
                    return "Card";
                case "V":
                    return "Visa";
                default:
                    throw new Exception("Invalid Document Type");
            }

        }



        private string IssuingCountryIso(string mrz)
        {
            return mrz.Substring(2, 3);

        }

        private string IssuingCountryName(string issIso)
        {
            var natItem = _nationalities.NationalitybyCode(issIso);
            return natItem != null ? natItem.Name : string.Empty;

        }

        private string FirstName(string mrz)
        {
            //var nameArray = mrz.Substring(61,)
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

        private string NationalityName(string natIso)
        {
            var natItem = _nationalities.NationalitybyCode(natIso);
            return natItem != null ? natItem.Name : string.Empty;

        }

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

        private DateTime IssueDate(DateTime expireDate, string nationality)
        {
            return new DateTime(1900, 1, 1); //todo calculate based on Expire Date and nationality
        }

        private string IssuingAuthority(string issuingCountry)
        {
            return "USDS"; //todo calculate based on issuing Country
        }

        private string PlaceOfBirth(string nationality)
        {
            return "Karachi"; //todo calculate based on nationality
        }




        public string CreatMrz(ParseUserData customer, bool isMakeFullName)
        {
            if (string.IsNullOrEmpty(customer.IssuingCountryIso) || string.IsNullOrEmpty(customer.LastName) || string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.DocumentNumber) ||
                string.IsNullOrEmpty(customer.NationalityIso) || customer.DateOfBirth.Year < 1901 || string.IsNullOrEmpty(customer.Gender) || customer.ExpireDate.Year < 1901) return string.Empty;
            var line1 = "P<" + customer.IssuingCountryIso + (customer.LastName + "<<" + customer.FirstName).Replace(" ", "<");
            if (isMakeFullName)
                line1 = "P<" + customer.IssuingCountryIso + (customer.FirstName + "<" + customer.LastName).Replace(" ", "<");
            line1 = line1.PadRight(44, '<').Replace("-", "<");
            if (line1.Length > 44)
                line1 = line1.Substring(0, 44);
            var line2 = customer.DocumentNumber.PadRight(9, '<') + CheckDigit(customer.DocumentNumber.PadRight(9, '<')) + customer.NationalityIso +
                        customer.DateOfBirth.ToString("yyMMdd") +
                        CheckDigit(customer.DateOfBirth.ToString("yyMMdd")) + customer.Gender.Substring(0, 1) +
                        customer.ExpireDate.ToString("yyMMdd") +
                        CheckDigit(customer.ExpireDate.ToString("yyMMdd"));
            line2 = line2.PadRight(42, '<') + "0";
            var compositeCheckDigit =
                CheckDigit(line2.Substring(0, 10) + line2.Substring(13, 7) +
                           line2.Substring(21, 22));
            line2 = line2 + compositeCheckDigit.Replace("-", "<");
            return line1 + line2;
        }

        internal string CheckDigit(string icaoPassportNumber)
        {
            //http://www.highprogrammer.com/alan/numbers/mrp.html#checkdigit
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