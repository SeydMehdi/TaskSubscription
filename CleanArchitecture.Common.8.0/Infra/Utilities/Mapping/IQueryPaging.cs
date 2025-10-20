namespace CleanArchitecture.Common.Infra.Utilities.Mapping
{
    public interface IQueryPaging
    {
        int Page { get; set; }
        int RowsPerPage { get; set; }
        int PageSkip { get; }
        bool UsePaging { get; set; }
    }


    public interface IQueryPagingNullable
    {
        int? Page { get; set; }
        int? RowsPerPage { get; set; }
        int? PageSkip { get; }
        bool? UsePaging { get; set; }
    }

    public interface IDateFilterDto
    {
        bool IsSearchByDate { get; set; }
        DateTime? FromDate { get; set; }
        string FromDateSh { get; set; }
        string ToDateSh { get; set; }
        DateTime? ToDate { get; set; }

    }
}