using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonthlyBudget.Services
{
    public class Helper
    {
        public static bool IsValidMonth(int m)
        {
            if (m > 0 && m <= 12)
            {
                return true;
            }
            else return false;
        }

        public static bool IsValidYear(int y)
        {
            if (y >= 2016)
            {
                return true;
            }
            else return false;
        }

        public static bool ToLaterThanFrom(int m, int y, int fm, int fy)
        {
            DateTime to = new DateTime(y, m, 1);
            DateTime from = new DateTime(fy, fm, 1);
            return to >= from;
        }

        public static DateTime ReturnToDate(int month, int year)
        {
            if (month == 12)
            {
                return new DateTime(year + 1, 1, 1).AddDays(-1);
            }
            else
            {
                return new DateTime(year, month + 1, 1).AddDays(-1);
            }
        }
    }
}
