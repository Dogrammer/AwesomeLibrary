using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Infrastructure.ApiModel
{
    public enum LibraryErrorCodes
    {
        INTERNAL_ERROR = 1,
        VALIDATION_FAILED = 2,
        AUTHENTICATION_FAILED = 3,
        RESOURCE_NOT_FOUND = 4,
        BUSINESS_ERROR = 5

    }
}
