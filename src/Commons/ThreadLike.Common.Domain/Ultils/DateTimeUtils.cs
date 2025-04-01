using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain.Shares;

namespace ThreadLike.Common.Domain.Ultils
{
    public static class DateTimeUtils
    {
		public static string[] ALLOWED_FORMAT = [DateTimeFormatRules.DATE_FORMAT, DateTimeFormatRules.DATE_TIME_FORMAT];
		public static bool BeAValidDate(string dateString, string format)
		{
			// Simple date format validation
			return DateTime.TryParseExact(dateString,format , null, DateTimeStyles.None, out _);
		}
		public static bool BeGreaterThanUTCNow(string dateStr ,string format)
		{
			if (DateTime.TryParseExact(dateStr, format, null, DateTimeStyles.None, out DateTime date))
			{
				return date > DateTime.UtcNow;
			}
			return false;
		}
		public static bool TryParseDateTimeToAllowedRules(string dateStr, out DateTime dateTime)
		{
			// assume all the datestr are local ( it seems correct since client send the date time in local format, not much send in UTC)
			foreach (string format in ALLOWED_FORMAT)
			{
				if (DateTime.TryParseExact(dateStr, format, null, DateTimeStyles.AssumeLocal, out  dateTime))
				{
					return true;
				}
			}
			dateTime = default;
			return false;
		}
	}
}
