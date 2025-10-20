namespace CleanArchitecture.Common.Application.Results
{
    public enum ErrorTypes
    {

        Unknown = 0,
        FatalError = 1,
        Error = 2,
        CatchError = 3,
        PermissionError = 4,
        LogicalError = 5,
        ChildResultError = 6,
        //---------------------
        // from 100
        Permission_PermissionNotExist = 100,
        Permission_UserIsDenied = 101,
    }

    public enum ResultTypes
    {
        Success = 0,
        Warning = 1,
        Error = 2,
        BadRequest = 3,
        NotFound = 4,
    }
}
