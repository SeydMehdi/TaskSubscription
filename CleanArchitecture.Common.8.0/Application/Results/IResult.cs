using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Common.Application.Results
{
    public interface IResult
    {
        int errorNumber { get; set; }
        string message { get; }

        void AddMessage(string message, ResultTypes resultTypes);

        object Object { get; set; }

        object ClientJSON();
        void AddMessageRange(IEnumerable<string> messages, ResultTypes resultType);

        List<object> DataList { get; set; }


        bool hasError { get; }
        string exceptionMessage { get; }
        List<ResultMessage> messageList { get; set; }
        int? StatusCode { get; set; }
    }
}
