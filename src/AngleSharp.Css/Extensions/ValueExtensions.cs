﻿namespace AngleSharp.Css.Extensions
{
    using AngleSharp.Css.Dom;
    using AngleSharp.Css.Parser;
    using AngleSharp.Css.Values;
    using AngleSharp.Text;
    using System;
    using System.Globalization;

    /// <summary>
    /// A bunch of useful extension methods.
    /// </summary>
    static class ValueExtensions
    {
        public static Length? ToDistance(this StringSource value)
        {
            var percent = value.ToPercent();

            if (percent.HasValue)
            {
                return new Length(percent.Value.Value, Length.Unit.Percent);
            }

            return value.ToLength();
        }

        public static Length ToLength(this FontSize fontSize)
        {
            switch (fontSize)
            {
                case FontSize.Big://1.5em
                    return new Length(1.5f, Length.Unit.Em);
                case FontSize.Huge://2em
                    return new Length(2f, Length.Unit.Em);
                case FontSize.Large://1.2em
                    return new Length(1.2f, Length.Unit.Em);
                case FontSize.Larger://*120%
                    return new Length(120f, Length.Unit.Percent);
                case FontSize.Little://0.75em
                    return new Length(0.75f, Length.Unit.Em);
                case FontSize.Small://8/9em
                    return new Length(8f / 9f, Length.Unit.Em);
                case FontSize.Smaller://*80%
                    return new Length(80f, Length.Unit.Percent);
                case FontSize.Tiny://0.6em
                    return new Length(0.6f, Length.Unit.Em);
                default://1em
                    return new Length(1f, Length.Unit.Em);
            }
        }

        public static Percent? ToPercent(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null && test.Dimension == "%")
            {
                var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);
                return new Percent(value);
            }

            return null;
        }

        public static Boolean IsIdentifier(this StringSource str)
        {
            var test = str.ParseIdent();
            return test != null;
        }

        public static Boolean IsLiteral(this StringSource str)
        {
            var detected = 0;

            while (!str.IsDone)
            {
                var test = str.ParseIdent();

                if (test == null)
                    break;

                str.SkipSpaces();
            }

            return detected > 0;
        }

        public static Boolean IsAnimatableIdentifier(this StringSource str)
        {
            var test = str.ParseIdent();
            return test != null && (test.Isi(CssKeywords.All) || true);//TODO: Factory.Properties.IsAnimatable(value));
        }

        public static Single? ToSingle(this StringSource str)
        {
            return str.ParseNumber();
        }

        public static Single? ToNaturalSingle(this StringSource str)
        {
            var element = str.ParseNumber();
            return element.HasValue && element.Value >= 0f ? element : null;
        }

		public static Single? ToGreaterOrEqualOneSingle(this StringSource str)
		{
			var element = str.ParseNumber();
			return element.HasValue && element.Value >= 1f ? element : null;
		}

        public static Int32? ToInteger(this StringSource str)
        {
            return str.ParseInteger();
        }

        public static Int32? ToNaturalInteger(this StringSource str)
        {
            var element = str.ParseInteger();
            return element.HasValue && element.Value >= 0 ? element : null;
        }

        public static Int32? ToPositiveInteger(this StringSource str)
        {
            var element = str.ParseInteger();
            return element.HasValue && element.Value > 0 ? element : null;
        }

        public static Int32? ToWeightInteger(this StringSource str)
        {
            var element = str.ToPositiveInteger();
            return element.HasValue && IsWeight(element.Value) ? element : null;
        }

        public static Int32? ToBinary(this StringSource str)
        {
            var element = str.ParseInteger();
            return element.HasValue && (element.Value == 0 || element.Value == 1) ? element : null;
        }

        public static Angle? ToAngle(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null)
            {
                var unit = Angle.GetUnit(test.Dimension);

                if (unit != Angle.Unit.None)
                {
                    var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);
                    return new Angle(value, unit);
                }
            }

            return null;
        }

        public static Frequency? ToFrequency(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null)
            {
                var unit = Frequency.GetUnit(test.Dimension);

                if (unit != Frequency.Unit.None)
                {
                    var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);
                    return new Frequency(value, unit);
                }
            }

            return null;
        }

        public static Object ToLengthOrPercent(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null)
            {
                var unit = Length.Unit.Px;
                var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);

                if (test.Dimension == "%")
                {
                    return new Percent(value);
                }
                else if ((test.Dimension == String.Empty && test.Value == "0") ||
                    (unit = Length.GetUnit(test.Dimension)) != Length.Unit.None)
                {
                    return new Length(value, unit);
                }
            }

            return null;
        }

        public static Length? ToLength(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null)
            {
                var unit = Length.Unit.Px;

                if ((test.Dimension == String.Empty && test.Value == "0") ||
                    (unit = Length.GetUnit(test.Dimension)) != Length.Unit.None)
                {
                    var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);
                    return new Length(value, unit);
                }
            }

            return null;
        }

        public static Resolution? ToResolution(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null)
            {
                var unit = Resolution.GetUnit(test.Dimension);

                if (unit != Resolution.Unit.None)
                {
                    var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);
                    return new Resolution(value, unit);
                }
            }

            return null;
        }

        public static Time? ToTime(this StringSource str)
        {
            var test = str.ParseUnit();

            if (test != null)
            {
                var unit = Time.GetUnit(test.Dimension);

                if (unit != Time.Unit.None)
                {
                    var value = Single.Parse(test.Value, CultureInfo.InvariantCulture);
                    return new Time(value, unit);
                }
            }

            return null;
        }

        public static Length? ToBorderWidth(this StringSource value)
        {
            var length = value.ToLength();

            if (length == null)
            {
                var ident = value.ParseIdent();

                if (ident != null)
                {
                    if (ident.Is(CssKeywords.Thin))
                    {
                        return Length.Thin;
                    }
                    else if (ident.Is(CssKeywords.Medium))
                    {
                        return Length.Medium;
                    }
                    else if (ident.Is(CssKeywords.Thick))
                    {
                        return Length.Thick;
                    }
                }
            }

            return length;
        }

        public static Color? ToColor(this StringSource str)
        {
            return str.ParseColor();
        }

        private static Boolean IsWeight(Int32 value)
        {
            return value == 100 || value == 200 || value == 300 || value == 400 ||
                   value == 500 || value == 600 || value == 700 || value == 800 ||
                   value == 900;
        }
    }
}