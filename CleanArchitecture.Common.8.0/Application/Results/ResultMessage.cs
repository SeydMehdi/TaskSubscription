namespace CleanArchitecture.Common.Application.Results
{
    public class ResultMessage
    {
        public string message { get; set; }
        public ResultTypes resultType { get; set; } = ResultTypes.Success;
        public ErrorTypes errorType { get; set; } = ErrorTypes.Unknown;
    }
}
