using System;
using System.Collections.Generic;
using System.Text;

namespace Recognizer.Adapter.Infrastructure.RequestResponse
{
    public class RecognizerRequest
    {
        public string traceId { get; set; }
        public bool returnFullDocumentImage { get; set; }
        public bool returnFaceImage { get; set; }
        public bool returnSignatureImage { get; set; }
        public bool allowBlurFilter { get; set; }
        public bool allowUnparsedMrzResults { get; set; }
        public bool allowUnverifiedMrzResults { get; set; }
        public bool validateResultCharacters { get; set; }
        public string anonymizationMode { get; set; }
        public bool anonymizeImage { get; set; }
        public int ageLimit { get; set; }
        public string imageSource { get; set; }
       
    }
}
