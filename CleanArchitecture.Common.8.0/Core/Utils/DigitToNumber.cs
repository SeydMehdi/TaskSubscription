namespace CleanArchitecture.Common.Core.Utils
{
    public class DigitToNumber
    {
        string[] _10 = new string[] { "", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
        string[] _20 = new string[] { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
        string[] _100 = new string[] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        string[] _1000 = new string[] { "", "صد", "دویست", "سیصد", "جهارصد", "پانصد", "ششصد", "هفتصد", "هشصد", "نهصد" };
        string[] group = new string[] { "", "هزار", "میلیون", "میلیارد", "تریلیون" };

        public int RemDiv(long a, long b, out long Div) { Div = a / b; return (int)(a % b); }
        public string getLess100Value(int value)
        {
            if (value < 10) return _10[value];
            if (value >= 10 && value < 20)
                return _20[value - 10];
            if (value >= 20 && value < 100)
                return _100[value / 10] + (_10[value % 10].Length > 0 ? " و " : "") + _10[value % 10];
            return "";
        }
        public string getValue(int value) { if (value < 100) return getLess100Value(value); return _1000[value / 100] + " و " + getLess100Value(value % 100); }
        public string ConvertNumberToDigit(long number)
        {
            int count = 0;
            var stack = new List<string>();
            while (number > 0)
                stack.Add(getValue(RemDiv(number, 1000, out number)) + " " + group[count++]);
            stack.Reverse();
            return string.Join(" و ", stack);
        }
    }
}
