using System;
using System.Collections.Generic;
using Xunit;

namespace TimeZoneDSTSample
{
    public class TimeZoneTests
    {
        // https://www.gov.uk/when-do-the-clocks-change

        // Year	    Clocks go forward	    Clocks go back
        // 2014	    30 March	            26 October
        // 2015	    29 March	            25 October
        // 2016	    27 March	            30 October

        // In the UK the clocks go forward 1 hour at 1am on the last Sunday in March, and back 1 hour at 2am on the last Sunday in October.
        // The period when the clocks are 1 hour ahead is called British Summer Time (BST). There's more daylight in the evenings and less in the mornings (sometimes called Daylight Saving Time).
        // When the clocks go back, the UK is on Greenwich Mean Time (GMT).

        [Fact]
        public void Should_Detect_Ambigious_Time()
        {
            const string ukTimeZoneId = "GMT Standard Time";
            TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ukTimeZoneId);
            for (int minute = 0; minute < 60; minute++)
            {
                var ambiguousUkDateTime = new DateTime(2014, 10, 26, 1, minute, 0);
                bool isAmbiguousTime = ukTimeZone.IsAmbiguousTime(ambiguousUkDateTime);
                Assert.True(isAmbiguousTime);
            }
        }

        [Fact]
        public void Should_Detect_Inambigious_Time()
        {
            const string ukTimeZoneId = "GMT Standard Time";
            TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ukTimeZoneId);
            IEnumerable<DateTime> inAmbiguousUkDateTimes = new[]
            {
                new DateTime(2014, 10, 26, 0, 59, 59),
                new DateTime(2014, 10, 26, 2, 0, 0)
            };

            foreach (DateTime inAmbiguousUkDateTime in inAmbiguousUkDateTimes)
            {
                bool isAmbiguousTime = ukTimeZone.IsAmbiguousTime(inAmbiguousUkDateTime);
                Assert.False(isAmbiguousTime);
            }
        }

        [Fact]
        public void Should_Detect_Daylight_Saving_Time()
        {
            const string ukTimeZoneId = "GMT Standard Time";
            TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ukTimeZoneId);
            DateTime dstStartDateTime = new DateTime(2014, 3, 30, 2, 0, 0);

            for (int i = 0; i < 210; i++)
            {
                bool isDst = ukTimeZone.IsDaylightSavingTime(dstStartDateTime.AddDays(i));
                Assert.True(isDst);
            }
        }

        public void Should_Detect_Non_Daylight_Saving_Time()
        {
            const string ukTimeZoneId = "GMT Standard Time";
            TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ukTimeZoneId);
            IEnumerable<DateTime> nonDsts = new[]
            {
                new DateTime(2014, 3, 30, 2, 0, 0),
                new DateTime(2014, 10, 26, 2, 0, 0)
            };
        }
    }
}
