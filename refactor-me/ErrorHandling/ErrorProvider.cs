using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.ErrorHandling
{
    public enum ErrorCode
    {
        ProductNotFound = 1,
        ProductOptionNotFound,
        Generic,
    }

    public class ErrorProvider
    {
        public static Error ReturnError(ErrorCode code, string details = null)
        {
            switch (code)
            {
                case ErrorCode.ProductNotFound:
                    return new Error() { Code = (int)code, Message = "Product not found.", Details = details };
                case ErrorCode.ProductOptionNotFound:
                    return new Error() { Code = (int)code, Message = "Product option not found.", Details = details };
            }

            return null;
        }

        public static Error ReturnGenericFromException(Exception ex)
        {
            return new Error() { Code = (int)ErrorCode.Generic, Message = "Generic error occurred.", Details = $"From {ex.Source}: {ex.Message}" };
        }

    }
}