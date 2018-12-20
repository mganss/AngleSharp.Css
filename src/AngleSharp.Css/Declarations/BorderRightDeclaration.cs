namespace AngleSharp.Css.Declarations
{
    using AngleSharp.Css.Aggregators;
    using AngleSharp.Css.Converters;
    using System;
    using static ValueConverters;

    static class BorderRightDeclaration
    {
        public static String Name = PropertyNames.BorderRight;

        public static String Parent = PropertyNames.Border;

        public static IValueConverter Converter = Or(WithAny(LineWidthConverter.Option(), LineStyleConverter.Option(), CurrentColorConverter.Option()), AssignInitial());

        public static IValueAggregator Aggregator = new BorderRightAggregator();

        public static PropertyFlags Flags = PropertyFlags.Animatable | PropertyFlags.Shorthand;

        public static String[] Children = new[]
        {
            PropertyNames.BorderRightWidth,
            PropertyNames.BorderRightStyle,
            PropertyNames.BorderRightColor,
        };
    }
}