using CleanArchitecture.Common.Infra.Utilities.Mapping;

namespace CleanArchitecture.Common.Application.Dto
{

    public class PagingNullableDto
    {
        public int? Page { get; set; } = 0;
        public int? RowsPerPage { get; set; } = 30;
        public int? PageSkip { get => Page * RowsPerPage; }
        public bool? UsePaging { get; set; } = true;
    }

    public class PagingDto : BaseDto, IQueryPaging
    {
        public int Page { get; set; } = 0;
        public int RowsPerPage { get; set; } = 30;
        public int PageSkip { get => Page * RowsPerPage; }
        public bool UsePaging { get; set; } = true;
    }
    public class BaseSearchItem : BaseDto, IQueryPaging, IDateFilterDto
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public List<string> ValueList { get; set; }

        public string Name { get; set; }

        public BaseSearchItem()
        {
            RowsPerPage = 50;
            Page = 0;
            Roles = new List<string>();
        }
        public DateTime? FromDate { get; set; }

        public long FromDateUnixTime
        {
            get
            {
                if (FromDate == null) return 0;
                return new DateTimeOffset(FromDate.Value).ToUnixTimeMilliseconds();
            }
        }

        public bool IsSearchByDate { get; set; }


        public string FromDateSh
        {
            get { return SHDateTime.ToDateString(FromDate); }
            set
            { FromDate = SHDateTime.ToDateTimeN(value); }
        }

        public string ToDateSh
        {
            get { return SHDateTime.ToDateString(ToDate); }
            set { ToDate = SHDateTime.ToDateTimeN(value); }
        }

        public DateTime? ToDate { get; set; }

        public long EndDateUnixTime
        {
            get
            {
                if (ToDate == null) return 0;
                return new DateTimeOffset(ToDate.Value).ToUnixTimeMilliseconds();
            }
        }

        public string Username { get; set; }

        public List<string> Roles { get; set; }

        public bool HasRole(string roleName)
        {
            if (Roles == null) return false;
            return Roles.Contains(roleName);
        }

        public int Page { get; set; }

        public int RowsPerPage { get; set; }
        public int PageSkip
        {
            get
            {
                return Page * RowsPerPage;
            }
        }

        public bool UsePaging { get; set; } = true;
    }
}
