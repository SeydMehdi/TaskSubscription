using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Common.Application.Dto
{
    public class BaseSearchDto
    {
        public bool IsSearchByDate { get; set; } = false;
        public string FromDateSh
        {
            get
            {
                return SHDateTime.ToDateTimeString(FromDate);
            }
            set
            {
                FromDate = SHDateTime.ConvertToDateTime(value);
            }
        }
        public string ToDateSh
        {
            get
            {
                return SHDateTime.ToDateTimeString(ToDate);
            }
            set
            {
                ToDate = SHDateTime.ConvertToDateTime(value);
            }
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


    }
}
