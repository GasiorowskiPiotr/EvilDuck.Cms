using EvilDuck.Cms.Portal.Framework.Logging;
using Microsoft.AspNet.Http;
using System;

namespace EvilDuck.Cms.Portal.Framework
{
    public abstract class Result
    {
        protected Result(string message, HttpRequest request)
        {
            Message = message;
            Request = request;
            Log();
        }

        public HttpRequest Request { get; private set; }
        public string Message { get; private set; }

        public abstract void ThrowIfError();
        public abstract Result Combine(Result result);
        protected abstract void Log();

        public static Result Success(string message = null, HttpRequest request = null)
        {
            return new Success(message, request);
        }

        public static Result Failure(string message, HttpRequest request = null, Exception exception = null)
        {
            return new Failure(message, request, exception);
        }
    }

    public sealed class Success : Result
    {
        internal Success(string message, HttpRequest request) : base(message, request)
        {
            
        }

        public override Result Combine(Result result)
        {
            if(result is Failure)
            {
                return result;
            }
            return this;
        }

        protected override void Log() { }
        public override void ThrowIfError() { }
    }

    public sealed class Failure : Result
    {
        internal Failure(string message, HttpRequest request, Exception exception = null) : base(message, request)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }

        public override Result Combine(Result result)
        {
            if(result is Success)
            {
                return this;
            }

            return new Failure(String.Format("{0}{1}{2}", this.Message, Environment.NewLine, result.Message), Request);            
        }

        public override void ThrowIfError()
        {
            throw new FailureException(Exception);
        }

        protected override void Log()
        {
            if(Request != null)
            {
                var log = (ILog)Request.HttpContext.ApplicationServices.GetService(typeof(ILog));
                log.LogError(() => String.Format("{0}{1}{2}", Message, Environment.NewLine, Exception));
            }
            
        }
    }

    public class FailureException : Exception
    {
        public FailureException(Exception exception) : base("", exception)
        {
        }
    }
}