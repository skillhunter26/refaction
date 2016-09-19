using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.ErrorHandling
{
    public class ErrorPayload
    {
        public List<Error> Errors { get; set; }

        public ErrorPayload()
        {
            Errors = new List<Error>();
        }

        public ErrorPayload(params Error[] errors)
        {
            Errors = new List<Error>(errors);
        }

        public ErrorPayload Add(Error error)
        {
            Errors.Add(error);
            return this;
        }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        public ErrorPayload WrapInPayload()
        {
            return new ErrorPayload(this);
        }
    }
}