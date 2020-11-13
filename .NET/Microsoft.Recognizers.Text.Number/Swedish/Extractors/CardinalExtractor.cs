﻿using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Swedish
{
    public class CardinalExtractor : CachedNumberExtractor
    {
        private static readonly ConcurrentDictionary<string, CardinalExtractor> Instances =
            new ConcurrentDictionary<string, CardinalExtractor>();

        private readonly string keyPrefix;

        private CardinalExtractor(BaseNumberOptionsConfiguration config)
            : base(config.Options)
        {
            keyPrefix = string.Intern(ExtractType + "_" + config.Options + "_" + config.Placeholder + "_" + config.Culture);

            var builder = ImmutableDictionary.CreateBuilder<Regex, TypeTag>();

            // Add Integer Regexes
            var intExtract = IntegerExtractor.GetInstance(placeholder);
            builder.AddRange(intExtract.Regexes);

            // Add Double Regexes
            var douExtract = DoubleExtractor.GetInstance(placeholder);
            builder.AddRange(douExtract.Regexes);

            Regexes = builder.ToImmutable();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        // "Cardinal";
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_CARDINAL;

        public static CardinalExtractor GetInstance(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new CardinalExtractor(config);
                Instances.TryAdd(extractorKey, instance);
            }

            return Instances[extractorKey];
        }

        protected override object GenKey(string input)
        {
            return (keyPrefix, input);
        }
    }
}