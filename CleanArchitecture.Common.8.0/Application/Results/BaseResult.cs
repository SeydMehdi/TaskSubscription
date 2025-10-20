using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace CleanArchitecture.Common.Application.Results
{


    public class BaseResult : IResult
    {
        public BaseResult()
        {

        }
        public List<ResultMessage> messageList { get; set; } = new List<ResultMessage>();


        public BaseResult(ILogger logger)
        {
            this.logger = logger;
        }



        public int? StatusCode { get; set; }

        public ILogger logger { get; set; }

        public BaseResult(Exception exc, string message)
        {
            InnerException = exc;
            messageList.Add(new ResultMessage
            {
                message = message,
                resultType = ResultTypes.Error,
                errorType = ErrorTypes.CatchError
            });
        }

        public BaseResult(ResultTypes resultType, string message)
        {
            messageList.Add(new ResultMessage
            {
                message = message,
                resultType = resultType,
            });
        }

        public int errorNumber { get; set; }


        public void AddMessage(string message, ResultTypes resultType)
        {
            messageList.Add(new ResultMessage { message = message, resultType = resultType });
        }
        public void AddMessage(string message, ResultTypes resultType, ErrorTypes errorType)
        {
            messageList.Add(new ResultMessage { message = message, resultType = resultType, errorType = errorType });
        }

        public void AddMessage(IResult result)
        {
            messageList.AddRange(result.messageList);
        }

        public void AddMessageRange(IEnumerable<string> messages, ResultTypes resultType)
        {
            messageList.AddRange(messages.Select(m => new ResultMessage { message = m, resultType = resultType }));
        }
        public bool hasError
        {
            get
            {
                return messageList != null && messageList.Count(m => m.resultType == ResultTypes.Error) > 0;
            }
        }
        public bool hasWarning
        {
            get
            {
                return messageList != null && messageList.Count(m => m.resultType == ResultTypes.Warning) > 0;
            }
        }

        public string message
        {
            get
            {
                if (messageList == null || messageList.Count == 0)
                    return "";
                return messageList[0].message;
            }
        }


        public string exceptionMessage
        {
            get
            {
                if (InnerException != null)
                    return $"{message}\r\n{InnerException.Message}";
                return "no exception:" + message;
            }
        }

        public Exception InnerException { get; set; }

        public object Object { get; set; }

        public List<object> DataList { set; get; }

        public virtual object ClientJSON()
        {

            return new { errorNumber, message, Object, messageList };
        }
        public BaseResult Warning(string v)
        {
            AddMessage(v, ResultTypes.Warning);
            return this;
        }



        public bool IsWarning { get { return errorNumber == (int)ResultTypes.Warning; } }
        public bool IsSuccess { get { return errorNumber == (int)ResultTypes.Success; } }
        public BaseResult Success(string message)
        {
            AddMessage(message, ResultTypes.Success);
            return this;
        }


        public BaseResult Error(ErrorTypes errorType, string message)
        {
            AddMessage(message, ResultTypes.Error, errorType);
            errorNumber = (int)errorType;
            return this;
        }


        public BaseResult Error(int errorNumber, string message)
        {
            var errorType = ErrorTypes.Unknown;
            if (Enum.IsDefined(typeof(ErrorTypes), errorNumber))
                errorType = (ErrorTypes)errorNumber;

            AddMessage(message, ResultTypes.Error, errorType);
            errorNumber = (int)errorType;
            return this;
        }


        public BaseResult Error(IResult result)
        {
            AddMessage(result);
            errorNumber = (int)ErrorTypes.ChildResultError;
            return this;
        }
        public BaseResult Error(Exception exc, string message)
        {
            InnerException = exc;
            logError(exc, message); ;
            var innerException = exc.InnerException;
            int i = 1;
            while (innerException != null && i++ < 10)
            {
                logError(innerException, $"{i}{new string('-', i)}inner exception : level{i}" + innerException.Message);
                innerException = innerException.InnerException;
            }
            return Error(message);
        }

        private void logError(Exception exc, string message)
        {
            if (logger != null)
            {
                logger.LogError("bresult error : " + exc.Message + "-" + message);
                var excTemp = exc.InnerException;
                int counter = 0;
                while (excTemp != null)
                {
                    logger.LogError("bresult inner exception : " + excTemp.Message);
                    if (counter++ > 2) break;
                    excTemp = excTemp.InnerException;
                }
            }
        }

        public BaseResult Error(Exception exc)
        {
            InnerException = exc;
            logError(exc, "not message has passed"); ;
            return Error(exc.Message);
        }
        public BaseResult Error(string v)
        {
            if (errorNumber == 0)
            {
                // check if errornumber is et by top methods.
                errorNumber = (int)ErrorTypes.LogicalError;
            }
            AddMessage(v, ResultTypes.Error, ErrorTypes.LogicalError);
            return this;
        }
        public BaseResult Error(string logExcMessage, string message)
        {
            logError(new Exception(logExcMessage), message);
            AddMessage(message, ResultTypes.Error, ErrorTypes.LogicalError);
            return this;
        }

    }
}
