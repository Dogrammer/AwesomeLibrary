using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Infrastructure.ApiModel
{
    public class LibraryErrorInformation
    {
        public string UserMessage { get; set; }

        public string InternalMessage { get; set; }

        public LibraryErrorCodes Code { get; set; }
    }
}
