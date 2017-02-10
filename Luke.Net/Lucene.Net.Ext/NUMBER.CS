using System;
using System.Globalization;

namespace Lucene.Net.Util
{
	/// <summary>
	/// A simple class for number conversions.
	/// </summary>
	public class Number
	{
		/// <summary>
		/// Min radix value.
		/// </summary>
		public const int MIN_RADIX = 2;
		/// <summary>
		/// Max radix value.
		/// </summary>
		public const int MAX_RADIX = 36;

		private const string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

		public static string ToString(float f)
		{
			if (((float)(int)f) == f)
			{
				return ((int)f).ToString() + ".0";
			}
			else
			{
				return f.ToString(NumberFormatInfo.InvariantInfo);
			}
		}

		/// <summary>
		/// Converts a number to string in the specified radix.
		/// </summary>
		/// <param name="i">A number to be converted.</param>
		/// <param name="radix">A radix.</param>
		/// <returns>A string representation of the number in the specified redix.</returns>
		public static string ToString(long i, int radix)
		{
			if (radix < MIN_RADIX || radix > MAX_RADIX)
				radix = 10;

			char[] buf = new char[65];
			int charPos = 64;
			bool negative = (i < 0);

			if (!negative) 
			{
				i = -i;
			}

			while (i <= -radix) 
			{
				buf[charPos--] = digits[(int)(-(i % radix))];
				i = i / radix;
			}
			buf[charPos] = digits[(int)(-i)];

			if (negative) 
			{
				buf[--charPos] = '-';
			}

			return new String(buf, charPos, (65 - charPos)); 
		}

		/// <summary>
		/// Parses a number in the specified radix.
		/// </summary>
		/// <param name="s">An input string.</param>
		/// <param name="radix">A radix.</param>
		/// <returns>The parsed number in the specified radix.</returns>
		public static long Parse(string s, int radix)
		{
			if (s == null) 
			{
				throw new ArgumentException("null");
			}

			if (radix < MIN_RADIX) 
			{
				throw new NotSupportedException("radix " + radix +
					" less than Number.MIN_RADIX");
			}
			if (radix > MAX_RADIX) 
			{
				throw new NotSupportedException("radix " + radix +
					" greater than Number.MAX_RADIX");
			}

			long result = 0;
			long mult = 1;

			s = s.ToLower();
			
			for(int i=s.Length-1; i>=0; i--)
			{
				int weight = digits.IndexOf(s[i]);
				if(weight == -1)
					throw new FormatException("Invalid number for the specified radix");

				result += (weight * mult);
				mult *= radix;
			}

			return result;
		}
	}

	public class Date
	{
		static public long GetTime(DateTime dateTime)
		{
			TimeSpan ts = dateTime.Subtract(new DateTime(1970, 1, 1));
			ts = ts.Subtract(TimeZone.CurrentTimeZone.GetUtcOffset(dateTime));
			return ts.Ticks / TimeSpan.TicksPerMillisecond;
		}
	}
}