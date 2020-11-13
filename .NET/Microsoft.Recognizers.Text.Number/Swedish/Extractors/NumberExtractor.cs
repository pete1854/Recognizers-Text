using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.Number.Swedish
{
    public class NumberExtractor : CachedNumberExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), NumberExtractor>();

        private readonly string keyPrefix;

        private NumberExtractor(NumberMode mode, NumberOptions options)
            : base(options)
        {

            this.mode = config.Mode;

            NegativeNumberTermsRegex = new Regex(NumbersDefinitions.NegativeNumberTermsRegex + "$", RegexFlags);

            NegativeNumberTermsRegex = new Regex(NumbersDefinitions.NegativeNumberTermsRegex + '$', RegexFlags);

            RelativeReferenceRegex = new Regex(NumbersDefinitions.RelativeOrdinalRegex, RegexFlags);

            Options = config.Options;

            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            // Add Cardinal
            CardinalExtractor cardExtract = null;
            switch (mode)
            {
                case NumberMode.PureNumber:
                    cardExtract = CardinalExtractor.GetInstance(NumbersDefinitions.PlaceHolderPureNumber);
                    break;
                case NumberMode.Currency:
                    builder.Add(
                        BaseNumberExtractor.CurrencyRegex,
                        RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX));
                    break;
                case NumberMode.Default:
                    break;
            }

            if (cardExtract == null)
            {
                cardExtract = CardinalExtractor.GetInstance();
            }

            builder.AddRange(cardExtract.Regexes);

            // Add Fraction
            var fracExtract = FractionExtractor.GetInstance(mode, Options);
            builder.AddRange(fracExtract.Regexes);

            Regexes = builder.ToImmutable();

            var ambiguityBuilder = ImmutableDictionary.CreateBuilder<Regex, Regex>();

            // Do not filter the ambiguous number cases like '$2000' in NumberWithUnit, otherwise they can't be resolved.
            if (config.Mode != NumberMode.Unit)
            {
                foreach (var item in NumbersDefinitions.AmbiguityFiltersDict)
                {
                    ambiguityBuilder.Add(new Regex(item.Key, RegexFlags), new Regex(item.Value, RegexFlags));
                }
            }

            AmbiguityFiltersDict = ambiguityBuilder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override ImmutableDictionary<Regex, Regex> AmbiguityFiltersDict { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM; // "Number";

        protected sealed override Regex NegativeNumberTermsRegex { get; }

        protected sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override Regex RelativeReferenceRegex { get; }

        public static NumberExtractor GetInstance(BaseNumberOptionsConfiguration config)
        {
            var cacheKey = (config.Mode, config.Options);
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new NumberExtractor(config);
                Instances.TryAdd(cacheKey, instance);
            }

        protected override object GenKey(string input)
        {
            return (keyPrefix, input);
        }

        protected override object GenKey(string input)
        {
            return (keyPrefix, input);
        }
    }
}