﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, IFullDateTimeParserConfiguration
    {
        public JapaneseDateTimeParserConfiguration(IDateTimeOptionsConfiguration config)
                : base(config)
        {
            DateExtractor = new JapaneseDateExtractorConfiguration();

            DateParser = new JapaneseDateParserConfiguration(this);
            TimeParser = new JapaneseTimeParserConfiguration(this);
            DateTimeParser = new JapaneseDateTimeParser(this);
            DatePeriodParser = new JapaneseDatePeriodParserConfiguration(this);
            TimePeriodParser = new JapaneseTimePeriodParserConfiguration(this);
            DateTimePeriodParser = new JapaneseDateTimePeriodParserConfiguration(this);
            DurationParser = new JapaneseDurationParserConfiguration(this);
            GetParser = new JapaneseSetParserConfiguration(this);
            HolidayParser = new JapaneseHolidayParserConfiguration(this);

            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.ParserConfigurationUnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();
            SeasonValueMap = DateTimeDefinitions.ParserConfigurationSeasonValueMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.ParserConfigurationDayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();

            Numbers = InitNumbers();

            DateRegexList = JapaneseDateExtractorConfiguration.DateRegexList;
            NextRegex = JapaneseDateExtractorConfiguration.NextRegex;
            ThisRegex = JapaneseDateExtractorConfiguration.ThisRegex;
            LastRegex = JapaneseDateExtractorConfiguration.LastRegex;
            YearRegex = JapaneseDateExtractorConfiguration.YearRegex;
            RelativeRegex = JapaneseDateExtractorConfiguration.RelativeRegex;
            StrictWeekDayRegex = JapaneseDateExtractorConfiguration.WeekDayRegex;
            WeekDayOfMonthRegex = JapaneseDateExtractorConfiguration.WeekDayOfMonthRegex;
            BeforeRegex = JapaneseMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = JapaneseMergedExtractorConfiguration.AfterRegex;
            UntilRegex = JapaneseMergedExtractorConfiguration.UntilRegex;
            SincePrefixRegex = JapaneseMergedExtractorConfiguration.SincePrefixRegex;
            SinceSuffixRegex = JapaneseMergedExtractorConfiguration.SinceSuffixRegex;
            EqualRegex = JapaneseMergedExtractorConfiguration.EqualRegex;
        }

        public int TwoNumYear => int.Parse(DateTimeDefinitions.TwoNumYear);

        public string LastWeekDayToken => DateTimeDefinitions.ParserConfigurationLastWeekDayToken;

        public string NextMonthToken => DateTimeDefinitions.ParserConfigurationNextMonthToken;

        public string LastMonthToken => DateTimeDefinitions.ParserConfigurationLastMonthToken;

        public string DatePrefix => DateTimeDefinitions.ParserConfigurationDatePrefix;

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DatePeriodParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public ImmutableDictionary<string, string> UnitMap { get; }

        public ImmutableDictionary<string, long> UnitValueMap { get; }

        public ImmutableDictionary<string, string> SeasonMap { get; }

        public ImmutableDictionary<string, int> SeasonValueMap { get; }

        public ImmutableDictionary<string, int> CardinalMap { get; }

        public ImmutableDictionary<string, int> DayOfMonth { get; }

        public ImmutableDictionary<string, int> DayOfWeek { get; }

        public ImmutableDictionary<string, int> MonthOfYear { get; }

        public ImmutableDictionary<string, int> Numbers { get; }

        public IEnumerable<Regex> DateRegexList { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex YearRegex { get; }

        public Regex DatePeriodYearRegex { get; }

        public Regex RelativeRegex { get; }

        public Regex StrictWeekDayRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex UntilRegex { get; }

        public Regex SincePrefixRegex { get; }

        public Regex SinceSuffixRegex { get; }

        public Regex EqualRegex { get; }

        public static int GetSwiftDay(string text)
        {
            // Today: 今天, 今日, 最近, きょう, この日
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.StartsWith("来", StringComparison.Ordinal) ||
                text.Equals("あす", StringComparison.Ordinal) ||
                text.Equals("あした", StringComparison.Ordinal) ||
                text.Equals("明日", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.StartsWith("昨", StringComparison.Ordinal) ||
                     text.Equals("きのう", StringComparison.Ordinal) ||
                     text.Equals("前日", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("大后天", StringComparison.Ordinal) ||
                     text.Equals("大後天", StringComparison.Ordinal))
            {
                value = 3;
            }
            else if (text.Equals("大前天", StringComparison.Ordinal))
            {
                value = -3;
            }
            else if (text.Equals("后天", StringComparison.Ordinal) ||
                     text.Equals("後天", StringComparison.Ordinal) ||
                     text.Equals("明後日", StringComparison.Ordinal) ||
                     text.Equals("あさって", StringComparison.Ordinal))
            {
                value = 2;
            }
            else if (text.Equals("前天", StringComparison.Ordinal) ||
                     text.Equals("一昨日", StringComparison.Ordinal) ||
                     text.Equals("二日前", StringComparison.Ordinal) ||
                     text.Equals("おととい", StringComparison.Ordinal))
            {
                value = -2;
            }

            return value;
        }

        public static int GetSwiftMonth(string text)
        {
            // Current month: 今月
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.Equals("来月", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.Equals("前月", StringComparison.Ordinal) ||
                     text.Equals("先月", StringComparison.Ordinal) ||
                     text.Equals("昨月", StringComparison.Ordinal) ||
                     text.Equals("先々月", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("再来月", StringComparison.Ordinal))
            {
                value = 2;
            }

            return value;
        }

        public static int GetSwiftYear(string text)
        {
            // Current year: 今年
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.Equals("来年", StringComparison.Ordinal) ||
                text.Equals("らいねん", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.Equals("昨年", StringComparison.Ordinal) ||
                     text.Equals("前年", StringComparison.Ordinal))
            {
                value = -1;
            }

            return value;
        }

        private static ImmutableDictionary<string, int> InitNumbers()
        {
            return new Dictionary<string, int>
            {
            }.ToImmutableDictionary();
        }
    }
}
