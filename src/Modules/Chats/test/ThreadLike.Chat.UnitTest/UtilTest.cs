using FluentAssertions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain.Shares;
using Xunit.Abstractions;

namespace ThreadLike.Chat.UnitTest
{
    public class UtilTest
    {
		private readonly ITestOutputHelper output;

		public UtilTest(ITestOutputHelper output)
		{
			this.output = output;
		}
		[Fact]
		public void Test()
		{
			try
			{
				DateTime utcnow = DateTime.Now;
				output.WriteLine(TimeZoneInfo.Local.DisplayName);
				output.WriteLine("utcnow kind: " + utcnow.Kind);
				TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(utcnow);
				output.WriteLine("Offset: " + offset);
				output.WriteLine("Offset from hour: " + TimeSpan.FromHours(7));
				DateTimeOffset now = new DateTimeOffset(utcnow, offset);
				output.WriteLine("Now: " + now);
				string formattedWithUtc = now.ToString(DateTimeFormatRules.DATE_TIME_FORMAT_WITH_UTC);

				string baseDateTime = "2021-09-01 12:00:00";
				bool result = DateTime.TryParseExact(baseDateTime, DateTimeFormatRules.DATE_TIME_FORMAT, null, System.Globalization.DateTimeStyles.AssumeLocal,
				out DateTime parsedDateTime);
				output.WriteLine("Formatted with UTC: " + formattedWithUtc);
				output.WriteLine("Parsed DateTime (UTC): " + parsedDateTime.ToUniversalTime());
				output.WriteLine("Parsed DateTimeOffset: " + new DateTimeOffset(parsedDateTime));
				output.WriteLine("Parsed Kind: " + parsedDateTime.Kind);
				output.WriteLine("Offset from parsed: " + TimeZoneInfo.Local.GetUtcOffset(parsedDateTime));
				output.WriteLine("Parsed DateTimeOffset: " + new DateTimeOffset(parsedDateTime, TimeSpan.FromHours(7))); // Corrected line

			}
			catch (Exception ex)
			{
				output.WriteLine("Error: " + ex.Message);
			}
		}
	}
}
