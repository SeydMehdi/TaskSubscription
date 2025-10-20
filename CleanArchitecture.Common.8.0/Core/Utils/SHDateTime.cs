using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

public class SHDateTime
{
    #region "enumerations"

    public enum En_DayOfWeek
    {
        شنبه = 6,
        یکشنبه = 1,
        دوشنبه = 2,
        سه_شنبه = 3,
        چهارشنبه = 4,
        پنجشنبه = 5,
        جمعه = 6
    }

    public enum En_Months
    {
        فروردین,
        اردیبهشت,
        خرداد,
        تیر,
        مرداد,
        شهریور,
        مهر,
        آبان,
        آذر,
        دی,
        بهمن,
        اسفند
    }
    public enum En_StringFormats
    {
        yyyy_mm_dd,
        yy_mm_dd,
        dd_mm_yyyy,
        yyyy_MoMo_dd,
        yy_MoMo_dd,
        MoMo_dd,
        yyyy_mm_dd_HH_MM,
        yyyy_mm_dd_HH_MM_SS,
        yy_mm_dd_HH_MM_SS,
        HH_MM_SS,
        HH_MM,
        dd
    }

    public static bool IsValid(string startDate)
    {
        return Regex.IsMatch(startDate, @"^$|^([1۱][۰-۹ 0-9]{3}[/\/]([0 ۰][۱-۶ 1-6])[/\/]([0 ۰][۱-۹ 1-9]|[۱۲12][۰-۹ 0-9]|[3۳][01۰۱])|[1۱][۰-۹ 0-9]{3}[/\/]([۰0][۷-۹ 7-9]|[1۱][۰۱۲012])[/\/]([۰0][1-9 ۱-۹]|[12۱۲][0-9 ۰-۹]|(30|۳۰)))$");
    }

    public string TextFromNow { 
        get
        {
            try
            {
                var length = DateTime.Now - ToDateTime();
                if (length.TotalHours < 1) return $"{(int)length.TotalMinutes + 1} دقیقه قبل";
                else if (length.TotalHours < 24) return $"{(int)length.TotalHours} ساعت قبل";
                return $"{(int)length.Days} روز قبل";
            }
            catch (Exception)
            {

                return "---";
            }
            
        }
    }


    /// <summary>
    /// Convert shamsi date string into unix time millisecond
    /// </summary>
    /// <returns></returns>
    public static long ToUnixTime(string shamsidate, TimeOnly time)
    {
        if (IsValid(shamsidate))
        {
            var dt = FromShamsiString(shamsidate).ToDateTime();
            dt = new DateTime(dt.Year, dt.Month, dt.Day,
                time.Hour, time.Minute, time.Second, time.Millisecond);
            return new DateTimeOffset(dt).ToUnixTimeMilliseconds();
        }
        return 0;

    }

    public static string GetHejriFolderName()
    {
        PersianCalendar persian = new PersianCalendar();
        return persian.GetYear(DateTime.Now) + "_" + persian.GetMonth(DateTime.Now) + "_" + persian.GetSecond(DateTime.Now) + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
    }
    #endregion

    #region "Operators"
    //Public Shared Operator =(ByVal shdate1 As SHDateTime, ByVal shdate2 As SHDateTime) As Boolean
    //    If shdate1.Year = shdate2.Year And
    //        shdate1.Month = shdate2.Month And
    //        shdate1.Day = shdate2.Day And
    //        shdate1.hour = shdate2.hour And
    //        shdate1.minute = shdate2.minute And
    //        shdate1.second = shdate2.second  Then
    //        Return True
    //    End If
    //    Return False
    //End Operator
    public bool equalTo1(SHDateTime shdate1)
    {
        if (shdate1.Year == this.Year & shdate1.Month == this.Month & shdate1.Day == this.Day & shdate1.hour == this.hour & shdate1.minute == this.minute & shdate1.second == this.second)
        {
            return true;
        }
        return false;
    }
    //Public Shared Operator <>(ByVal shdate1 As SHDateTime, ByVal shdate2 As SHDateTime) As Boolean
    //    Return Not (shdate1 = shdate2)
    //End Operator
    #endregion

    #region "fileds"
    private int m_year = DateTime.Now.Year;
    private int m_month = DateTime.Now.Month;
    private int m_day = DateTime.Now.Day;


    private int m_dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek);
    private int m_dayOfYear = DateTime.Now.DayOfYear;
    private int m_hour = DateTime.Now.Hour;
    private int m_minute = DateTime.Now.Minute;
    private int m_second = DateTime.Now.Second;
    private int m_miliSecond = DateTime.Now.Millisecond;
    #endregion
    private DateTime m_DateTime;

    #region "properties"

    public static SHDateTime MinValue
    {
        get { return new SHDateTime(1200, 1, 1); }
    }


    public static SHDateTime MaxValue
    {
        get { return new SHDateTime(1945, 12, 29); }
    }

    public int Year
    {
        get { return m_year; }
        set
        {
            if ((value < 1300))
                value = 1300;
            m_year = value;
        }
    }
    public int Month
    {
        get { return m_month; }
        set
        {
            if ((value > 12))
            {
                value = 1;
                Year += 1;
            }
            else if ((value < 1))
            {
                value = 12;
                Year -= 1;
            }
            m_month = value;
        }
    }
    public int MonthDayCount
    {
        get
        {
            if ((this.Month <= 6))
            {
                return 31;
            }
            else if ((this.Month > 6) & (this.Month < 12))
            {
                return 30;
            }
            else
            {
                if ((isKabiseh))
                {
                    return 30;
                }
                return 29;
            }

        }
        set { m_month = value; }
    }
    public string MonthString
    {
        get
        {
            string[] str = {
                "فروردین",
                "اردیبهشت",
                "خرداد",
                "تیر",
                "مرداد",
                "شهریور",
                "مهر",
                "آبان",
                "آذر",
                "دی",
                "بهمن",
                "اسفند"
            };
            return str[(Month - 1)].ToString();
        }
    }

    public int Day
    {
        get { return m_day; }
        set
        {
            if ((m_month <= 6))
            {
                if ((value > 31))
                {
                    value = 1;
                    Month += 1;
                }
            }
            else if ((m_month > 6 & m_month < 12))
            {
                if ((value > 30))
                {
                    value = 1;
                    Month += 1;
                }
            }
            else
            {
                if (isKabiseh)
                {
                    if ((value > 30))
                    {
                        value = 1;
                        Month += 1;
                    }
                }
                else
                {
                    if ((value > 29))
                    {
                        value = 1;
                        Month += 1;
                    }

                }
            }
            if ((m_month <= 7))
            {
                if ((value < 1))
                {
                    if ((m_month == 1))
                    {
                        Month = -1;
                        if ((isKabiseh))
                        {
                            value = 30;
                        }
                        else
                        {
                            value = 29;
                        }
                    }
                    else
                    {
                        value = 31;
                        Month -= 1;
                    }

                }
            }
            else if ((m_month > 7))
            {
                if ((value < 1))
                {
                    value = 30;
                    Month -= 1;
                }
            }
            m_day = value;
        }
    }
    public int dayOfWeek
    {
        get
        {
            m_dayOfWeek = Convert.ToInt32(ToDateTime().DayOfWeek);
            return m_dayOfWeek;
        }
    }
    public int dayOfYear
    {
        get { return m_dayOfYear; }
        set { m_dayOfYear = value; }
    }
    public int hour
    {
        get { return m_hour; }
        set
        {
            if ((value > 23))
            {
                value = 0;
                Day += 1;
            }
            else if ((value < 1))
            {
                value = 23;
                Day -= 1;
            }
            m_hour = value;
        }
    }
    public int minute
    {
        get { return m_minute; }
        set
        {
            if ((value > 59))
            {
                value = 0;
                hour += 1;
            }
            if ((value < 0))
            {
                value = 59;
                hour -= 1;
            }
            m_minute = value;
        }
    }
    public int second
    {
        get { return m_second; }
        set { m_second = value; }
    }
    public int miliSecond
    {
        get { return m_miliSecond; }
        set { m_miliSecond = value; }
    }

    public bool isKabiseh
    {
        get
        {
            if (((m_year + 1) % 4) == 0)
            {
                return true;
            }
            return false;
        }
    }
    #endregion

    #region "constructors"

    public SHDateTime()
    {
        try
        {
            PersianCalendar per = new PersianCalendar();
            this.m_DateTime = DateTime.Now;
            m_year = per.GetYear(m_DateTime);

            m_month = per.GetMonth(m_DateTime);
            m_day = per.GetDayOfMonth(m_DateTime);
            m_hour = m_DateTime.Hour;
            m_minute = m_DateTime.Minute;
            m_second = m_DateTime.Second;



        }
        catch (Exception)
        {
        }
    }
    public SHDateTime(DateTime MiladiDate)
    {
        try
        {
            PersianCalendar per = new PersianCalendar();
            this.m_DateTime = MiladiDate;
            m_year = per.GetYear(m_DateTime);
            m_month = per.GetMonth(m_DateTime);
            m_day = per.GetDayOfMonth(m_DateTime);
            m_hour = m_DateTime.Hour;
            m_minute = m_DateTime.Minute;
            m_second = m_DateTime.Second;
        }
        catch (Exception)
        {
            SHDateTime minValue = SHDateTime.MinValue;
            m_year = minValue.Year;
            m_month = minValue.Month;
            m_day = minValue.Day;
            m_hour = minValue.hour;
            m_minute = minValue.minute;
            m_second = minValue.second;
        }
    }

    public SHDateTime(int year)
    {
        m_year = year;
        m_month = 1;
        m_day = 1;
        m_hour = 0;
        m_minute = 0;
        m_second = 0;
    }
    public SHDateTime(int year, int month)
    {
        m_year = year;
        m_month = month;
        m_day = 1;
        m_hour = 0;
        m_minute = 0;
        m_second = 0;
    }
    public SHDateTime(int year, int month, int day)
    {
        m_year = year;
        m_month = month;
        m_day = day;
        m_hour = 0;
        m_minute = 0;
        m_second = 0;
    }
    public SHDateTime(string ShamsiString)
    {
        try
        {
            
            m_hour = 0;
            m_minute = 0;
            m_second = 0;
            string[] datePart = ShamsiString.Split(' ');
            string[] parts = null;
            if ((datePart.Length > 1))
            {
                ShamsiString = datePart[0];
                string[] timePart = datePart[1].Split(':');
                m_hour = Convert.ToInt32(timePart[0]);
                m_minute = Convert.ToInt32(timePart[1]);
                m_second = 0;
            }
            parts = ShamsiString.Split('/');
            if(parts.Length == 3 )
            {
                m_year = Convert.ToInt32(parts[0]);
                m_month = Convert.ToInt32(parts[1]);
                m_day = Convert.ToInt32(parts[2]);
            }

            m_DateTime = this.ToDateTime();

        }
        catch
        {
            throw;
        }


    }
    #endregion

    #region "private methods"
    private string add_zero(int digit)
    {
        string output = digit.ToString();
        if ((digit < 10))
        {
            output = "0" + digit.ToString();
        }
        return output;
    }
    #endregion

    #region "public methods"
    public DateTime ToDateTime()
    {
        PersianCalendar per = new PersianCalendar();
        try
        {
            return per.ToDateTime(this.m_year, this.m_month, this.m_day, this.m_hour, this.m_minute, this.m_second, this.m_miliSecond);
        }
        catch (Exception)
        {
            return DateTime.MinValue;
        }
    }


    public SHDateTime FirstOfWeek()
    {
        var dt = this.ToDateTime();
        dt.AddDays(-(int)dt.DayOfWeek - 2);
        return new SHDateTime(dt);
    }

    public SHDateTime EndOfWeek()
    {
        var dt = this.ToDateTime();
        var step = 4 - (int)dt.DayOfWeek;
        if (step == -1) step = 6;
        if (step == -2) step = 5;
        return new SHDateTime(dt.AddDays(step));
    }

    public SHDateTime FirstOfMonth()
    {
        SHDateTime output = (SHDateTime)this.MemberwiseClone();
        output.Day = 1;
        return output;
    }
    public SHDateTime FirstOfYear()
    {
        SHDateTime output = (SHDateTime)this.MemberwiseClone();
        output.Day = 1;
        output.Month = 1;
        return output;
    }
    public SHDateTime EndOfMonth()
    {
        SHDateTime output = (SHDateTime)this.MemberwiseClone();
        output.Day = 1;
        output.Month += 1;
        output.Day -= 1;
        return output;
    }
    public SHDateTime EndOfyear()
    {
        SHDateTime output = (SHDateTime)this.MemberwiseClone();
        output.Day = 1;
        output.Month = 1;
        output.Year += 1;
        output.Day -= 1;
        return output;
    }

    public SHDateTime addDays(int days)
    {
        SHDateTime output = (SHDateTime)this.MemberwiseClone();
        byte PosNeg = (byte)(days < 0 ? -1 : +1);
        for (int i = 1; i <= Math.Abs(days); i++)
        {
            output.Day += PosNeg;
        }
        return output;
    }
    public SHDateTime addMonths(int months)
    {
        SHDateTime output = (SHDateTime)this.MemberwiseClone();
        byte PosNeg = (byte)(months < 0 ? -1 : +1);
        for (int i = 1; i <= Math.Abs(months); i++)
        {
            output.Month += PosNeg;
        }
        return output;
    }

    public SHDateTime setMonth(int newMonth)
    {
        this.Month = newMonth;
        return this;
    }

    public SHDateTime setYear(int newYear)
    {
        this.Year = newYear;
        return this;
    }

    public SHDateTime setDay(int newDay)
    {
        this.Day = newDay;
        return this;
    }
    public SHDateTime resetTime()
    {
        this.m_hour = 0;
        this.m_minute = 0;
        this.m_day = 0;
        return this;
    }

    public static DateTime getFirstDayOfMonth(DateTime baseDate, int month)
    {
        SHDateTime shdate = new SHDateTime(baseDate);
        shdate.Month = month;
        shdate.Day = 1;
        return shdate.ToDateTime();
    }
    public static DateTime getLastDayOfMonth(DateTime baseDate, int month)
    {
        SHDateTime shdate = new SHDateTime(DateTime.Now);
        shdate.Month = month + 1;
        shdate.Day = 1;
        return shdate.ToDateTime().AddDays(-1);
    }


    public string ToString(En_StringFormats Format, char DateSpliter = '/', char TimeSpliter = ':')
    {
        try
        {
            string strYear = "";
            switch (Format)
            {
                case En_StringFormats.yyyy_mm_dd:
                    return string.Format("{0}{3}{1}{3}{2}", m_year, add_zero(m_month), add_zero(m_day), DateSpliter);
                case En_StringFormats.yy_mm_dd:
                    strYear = m_year.ToString().Substring(2, 2);
                    return string.Format("{0}{3}{1}{3}{2}", strYear, m_month, m_day, DateSpliter);
                case En_StringFormats.yyyy_mm_dd_HH_MM:
                    strYear = m_year.ToString().Substring(2, 2);
                    return string.Format("{4}{6}{5}-{0}{3}{1}{3}{2}", m_year, add_zero(m_month), add_zero(m_day), DateSpliter, add_zero(m_hour), add_zero(m_minute), TimeSpliter);
                case En_StringFormats.HH_MM:
                    strYear = m_year.ToString().Substring(2, 2);
                    return string.Format("{1}{0}{2}", TimeSpliter, add_zero(m_hour), add_zero(m_minute));
                case En_StringFormats.MoMo_dd:
                    break;
                case En_StringFormats.yy_MoMo_dd:
                    break;
                case En_StringFormats.dd_mm_yyyy:
                    return string.Format("{2}{3}{1}{3}{0}", m_year, add_zero(m_month), add_zero(m_day), DateSpliter);
            }

        }
        catch (Exception)
        {
            return null;
        }
        return ToString();
    }
    public new string ToString()
    {
        return ToString("hh:MM yyyy/mm/dd");
    }

    public static string ToDateString(DateTime dt)
    {
        if (dt == DateTime.MinValue) return "";
        SHDateTime shdate = new SHDateTime(dt);
        return shdate.ToString("yyyy/mm/dd");
    }


    public static string ToDateTimeString(DateTime dt, string dateSpliter = "/", string timeSpliter = ":")
    {

        if (dt == DateTime.MinValue) return "";
        SHDateTime shdate = new SHDateTime(dt);
        return shdate.ToString(string.Format("yyyy{0}mm{1}dd hh{2}MM", dateSpliter, dateSpliter, timeSpliter));
    }


    public static string ToDateTimeString(DateTime dt, string format)
    {
        SHDateTime shdate = new SHDateTime(dt);
        return shdate.ToString(format);
    }



    public static string ToDateString(DateTime? dt)
    {
        if (!dt.HasValue) return "";
        return ToDateString(dt.Value);
    }


    public static string ToDateTimeString(DateTime? dt)
    {
        if (!dt.HasValue) return "";
        return ToDateTimeString(dt.Value);
    }
    public static DateTime? ToDateTimeN(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        SHDateTime shdate = new SHDateTime(value);
        return shdate.ToDateTime();
    }
    public string ToString(string format)
    {
        string output = format.Replace("yyyy", this.Year.ToString());

        output = output.Replace("yy", this.Year.ToString().Substring(2));
        output = output.Replace("mm", this.Month.ToString("0#"));
        output = output.Replace("dd", this.Day.ToString("0#"));
        output = output.Replace("m", this.Month.ToString());
        output = output.Replace("d", this.Day.ToString());
        output = output.Replace("hh", this.hour.ToString("0#"));
        output = output.Replace("h", this.hour.ToString());
        output = output.Replace("MM", this.minute.ToString("0#"));
        output = output.Replace("M", this.minute.ToString());
        output = output.Replace("ss", this.second.ToString("0#"));
        output = output.Replace("s", this.second.ToString());
        return output;
    }



    /// <summary>
    /// This method return shamsi date string in yyyy/mm/dd format.
    /// </summary>
    /// <returns>####/##/##</returns>
    public string ToShamsiDateString()

    {
        return ToString(En_StringFormats.yyyy_mm_dd);
    }
    public string ToLongDateString()

    {
        return ToString("yyyy_mm_dd_hh_MM_ss");
    }


    public string ToTimeString()
    {
        return ToString(En_StringFormats.HH_MM);
    }

    public string ToDateStringHijri(En_StringFormats Format, char DateSpliter = '/', char TimeSpliter = ':')
    {
        HijriCalendar hej = new HijriCalendar();
        int hYear = hej.GetYear(m_DateTime);
        int hMonth = hej.GetMonth(m_DateTime);
        int hDay = hej.GetDayOfMonth(m_DateTime);
        string strYear = "";
        try
        {
            switch (Format)
            {
                case En_StringFormats.yyyy_mm_dd:
                    return string.Format("{0}{3}{1}{3}{2}", hYear, add_zero(hMonth), add_zero(hDay), DateSpliter);
                case En_StringFormats.yy_mm_dd:
                    strYear = hYear.ToString().Substring(2, 2);
                    return string.Format("{0}{3}{1}{3}{2}", strYear, hMonth, hDay, DateSpliter);
                case En_StringFormats.yyyy_mm_dd_HH_MM:
                    strYear = m_year.ToString().Substring(2, 2);
                    return string.Format("{4}{6}{5}-{0}{3}{1}{3}{2}", hYear, add_zero(hMonth), add_zero(hDay), DateSpliter, add_zero(m_hour), add_zero(m_minute), TimeSpliter);
                case En_StringFormats.MoMo_dd:
                    break;
                case En_StringFormats.yy_MoMo_dd:
                    break;
                case En_StringFormats.yyyy_mm_dd_HH_MM_SS:
                    return string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", hYear, add_zero(hMonth), add_zero(hDay), DateSpliter, add_zero(m_hour), add_zero(m_minute), TimeSpliter);
                case En_StringFormats.dd_mm_yyyy:
                    return string.Format("{2}{3}{1}{3}{0}", hYear, add_zero(hMonth), add_zero(hDay), DateSpliter);
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion

    #region "shared function"

    public static SHDateTime createNew(int year, int month, int day)
    {
        SHDateTime shdate = new SHDateTime(year, month, day);
        return shdate;
    }
    public static SHDateTime createNew(int year, int month)
    {
        SHDateTime shdate = new SHDateTime(year, month, 1);
        return shdate;
    }
    public static SHDateTime createNew(int year)
    {
        SHDateTime shdate = new SHDateTime(year, 1, 1);
        return shdate;
    }

    public static string getDayOfWeek(int dayOfWeek)
    {
        string[] weekDays = {
            "یکشنبه",
            "دوشنبه",
            "سه شنبه",
            "چهارشنبه",
            "پنجشنبه",
            "جمعه",
            "شنبه"
        };
        return weekDays[dayOfWeek].ToString();
    }

    public static object getMonthString(int month)
    {
        string[] monthString = {
            "فروردین",
            "اردیبهشت",
            "خرداد",
            "تیر",
            "مرداد",
            "شهریور",
            "مهر",
            "آبان",
            "آذر",
            "دی",
            "بهمن",
            "اسفند"
        };
        return monthString[month - 1];
    }

    public static object getMonthStringsJSON()
    {
        string[] monthString = {
            "فروردین",
            "اردیبهشت",
            "خرداد",
            "تیر",
            "مرداد",
            "شهریور",
            "مهر",
            "آبان",
            "آذر",
            "دی",
            "بهمن",
            "اسفند"
        };
        return "\"" + string.Join("\",\"", monthString) + "\"";
    }

    public static string getNextDayOfWeek(int dayOfWeek)
    {
        dayOfWeek += 1;
        if ((dayOfWeek > 6))
            dayOfWeek = 0;
        return getDayOfWeek(dayOfWeek);
    }
    public static string getPerviousDayOfWeek(int dayOfWeek)
    {
        dayOfWeek -= 1;
        if ((dayOfWeek < 0))
            dayOfWeek = 6;
        return getDayOfWeek(dayOfWeek);
    }
    public static SHDateTime NowShamsi()
    {
        SHDateTime output = new SHDateTime(DateTime.Now);
        return output;
    }
    /// <summary>
    /// dateString like 1391/10/08
    /// </summary>
    /// <param name="dateString"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static DateTime ConvertToDateTime(string dateString)
    {
        SHDateTime shdate = new SHDateTime(dateString);
        return shdate.ToDateTime();
    }
    /// <summary>
    /// dateString like 1391/10/08
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string ConvertToDateString(DateTime PDate)
    {
        if (PDate == DateTime.MinValue)
        {
            return "xxxx/xx/xx";
        }

        SHDateTime shdate = new SHDateTime(PDate);
        return shdate.ToString(En_StringFormats.yyyy_mm_dd);
    }

    public static SHDateTime FromDate(System.DateTime aDate)
    {
        SHDateTime shdate = new SHDateTime(aDate);
        return shdate;
    }
    /// <summary>
    /// Format must be  - > "1392/05/16"
    /// </summary>
    /// <param name="aDateString"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static SHDateTime FromShamsiString(string aDateString)
    {
        SHDateTime shdate = new SHDateTime(aDateString);
        return shdate;
    }
    public static SHDateTime FromShamsiDate(int year, int month, int day)
    {
        SHDateTime shdate = new SHDateTime(year, month, day);
        return shdate;
    }


    #endregion


    internal static bool ValidateString(string ShamsiString)
    {


        ShamsiString = ShamsiString.Trim();
        string[] parts = ShamsiString.Split('/');
        int m_year = Convert.ToInt32(parts[0]);
        int m_month = Convert.ToInt32(parts[1]);
        int m_day = Convert.ToInt32(parts[2]);
        if (!(m_year >= 1300 && m_year <= 1500))
            return false;
        if (!(m_month >= 1 && m_month <= 12))
            return false;
        if (m_month <= 6 && m_day > 31)
            return false;
        if (m_month > 6 && m_month <= 11 && m_day > 30)
            return false;
        if (m_month == 12)
        {
            if (((m_year + 1) % 4) == 0)
            {
                if (m_day > 30)
                    return false;

            }
            else if (m_day > 29) return false;
        }
        return true;
    }
}


