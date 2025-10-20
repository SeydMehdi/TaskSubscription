using CleanArchitecture.Common.Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Common.Application.Results
{
    public class ListResult<T> : BaseResult
    {
        public ListResult() { }
        public ListResult(ILogger logger) : base(logger) { }
        public bool isSuccess { get { return errorNumber == 0; } }

        public List<T> dataList { get; set; }

        public object ErrorInfo { get; set; }

        public System.Collections.IList IDataList { get { return dataList; } }


        public IQueryable<T> Query { get; set; }
        public object info { get; set; }
        public List<string> Object2 { get; set; }

        public new ListResult<T> Exception(Exception exc, string v)
        {
            base.Error(exc, v);
            return this;
        }

        public new ListResult<T> Error(string v)
        {
            base.Error(v);
            return this;
        }

        public new ListResult<T> Error(IResult v)
        {
            base.Error(v);
            return this;
        }

        public new ListResult<T> Error(Exception exc, string v)
        {
            base.Error(exc, v);
            if (logger != null)
            {
                logger.LogError($"app error : {v}-exception message: {exc.Message}");
            }
            return this;
        }

        public ListResult<T> Success(string v, List<T> checkers)
        {
            dataList = checkers;
            return Success(v);
        }



        public new ListResult<T> Success(string v)
        {
            base.Success(v);
            return this;
        }



        public override object ClientJSON()
        {

            return new { errorNumber, message, dataList, info, Object, Object2 };
        }

        public async ValueTask<ListResult<T>> SuccessMapAsync<U>(string message, IQueryable<U> query, CancellationToken token)
        {
            Success(message, await query.Select(m => m.MapTo<T>()).ToListAsync(token));
            return this;
        }
    }
}