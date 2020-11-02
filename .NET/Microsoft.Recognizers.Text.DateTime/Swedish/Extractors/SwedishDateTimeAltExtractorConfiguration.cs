﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishDateTimeAltExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeAltExtractorConfiguration
    {
        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags);

        public static readonly Regex RangePrefixRegex =
            new Regex(DateTimeDefinitions.RangePrefixRegex, RegexFlags);

        public static readonly Regex[] RelativePrefixList =
        {
            ThisPrefixRegex, PreviousPrefixRegex, NextPrefixRegex,
        };

        public static readonly Regex[] AmPmRegexList =
        {
            AmRegex, PmRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexFlags);

        private static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexFlags);

        public SwedishDateTimeAltExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new SwedishDateExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SwedishDatePeriodExtractorConfiguration(this));
        }

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.RelativePrefixList => RelativePrefixList;

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.AmPmRegexList => AmPmRegexList;

        Regex IDateTimeAltExtractorConfiguration.OrRegex => OrRegex;

        Regex IDateTimeAltExtractorConfiguration.ThisPrefixRegex => ThisPrefixRegex;

        Regex IDateTimeAltExtractorConfiguration.DayRegex => DayRegex;

        Regex IDateTimeAltExtractorConfiguration.RangePrefixRegex => RangePrefixRegex;

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }
    }
}