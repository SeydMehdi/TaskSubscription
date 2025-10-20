using CleanArchitecture.Common.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Common.Application.Results
{
    public class ObjectResult<T> : BaseResult
    {
        public ObjectResult()
        {

        }
        public ObjectResult(ILogger logger) : base(logger) { }

        public new T Object { get; set; }

        public object Object2 { get; set; }

        public object ErrorInfo { get; set; }
        public bool isUpdate { get; set; }



        public new ObjectResult<T> Error(ErrorTypes errorType, string message)
        {
            base.Error(errorType, message);
            return this;
        }

        public new ObjectResult<T> Error(IResult result)
        {
            base.Error(result);
            return this;
        }

        public new ObjectResult<T> Error(string message)
        {

            base.Error(message);
            return this;
        }

        public new ObjectResult<T> Error(string exceptionMessage, string message)
        {
            base.Error(exceptionMessage, message);
            return this;
        }

        public new ObjectResult<T> Error(Exception exc, string message)
        {
            base.Error(exc, message);
            return this;
        }

        public new ObjectResult<T> Error(int number, string message)
        {
            base.Error(number, message);
            return this;
        }





        public new ObjectResult<T> Success(string message)
        {
            base.Success(message);
            return this;
        }


        public ObjectResult<T> Success(string message, T value)
        {
            base.Success(message);
            Object = value;
            return this;
        }


        public ObjectResult<T> SuccessMap<TDto>(string message, TDto value)
        {
            base.Success(message);
            Object = value.MapTo<T>();
            return this;
        }

        public ObjectResult<T> Warning(string message, T value)
        {

            Warning(message);
            Object = value;
            return this;
        }



        public override object ClientJSON()
        {
            return new { errorNumber, message, Object, Object2 };
        }
    }

    public class ObjectResult<T, U> : ObjectResult<T>
    {
        public new U Object2 { get; set; }


        public new ObjectResult<T, U> Error(Exception exc, string message)
        {
            base.Error(exc, message);
            return this;
        }

        public new ObjectResult<T, U> Error(IResult result)
        {
            base.Error(result);
            return this;
        }

        public new ObjectResult<T, U> Error(ErrorTypes errorType, string v)
        {
            base.Error(errorType, v);
            return this;
        }


        public new ObjectResult<T, U> Error(string message)
        {
            base.Error(message);
            return this;
        }

        public new ObjectResult<T, U> Error(int number, string message)
        {
            base.Error(number, message);
            return this;
        }


        public new ObjectResult<T, U> Success(string message)
        {
            base.Success(message);
            return this;
        }

        public ObjectResult<T, U> Success(string message, T object1, U object2)
        {
            Success(message, object1);
            Object2 = object2;
            return this;
        }
        public new object ClientJSON()
        {
            return new { errorNumber, message, Object, Object2 };
        }
    }

    public class ObjectResult<T, U, W> : ObjectResult<T, U>
    {
        public W Object3 { get; set; }
    }

}
