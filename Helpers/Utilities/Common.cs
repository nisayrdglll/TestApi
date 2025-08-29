using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Helpers.Utilities
{
	public class Common
	{
		public string DateTimeToString(DateTime? datetimestr)
		{
			return datetimestr.GetValueOrDefault().ToString("g");
		}
		public string GetTodayDateString()
		{
			return DateTime.UtcNow.ToLocalTime().Year.ToString() + "." + DateTime.UtcNow.ToLocalTime().Month.ToString() + "." + DateTime.UtcNow.ToLocalTime().Day.ToString() + " " + DateTime.UtcNow.ToLocalTime().Hour.ToString() + ":" + DateTime.UtcNow.ToLocalTime().Minute.ToString();
		}
		public string GenerateUniqueCode(string Prefix, int id)
		{
			int padLength = 11 - id.ToString().Length;
			return Prefix.PadRight(padLength, '0') + id;
		}
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		public static byte[] ConvertFromBase64String(string input)
		{
			string working = input.Replace('-', '+').Replace('_', '/');
			while (working.Length % 3 != 0)
			{
				working += '=';
			}
			try
			{
				return Convert.FromBase64String(working);
			}
			catch (Exception)
			{

				try
				{
					return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/'));
				}
				catch (Exception) { }
				try
				{
					return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/') + "=");
				}
				catch (Exception) { }
				try
				{
					return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/') + "==");
				}
				catch (Exception) { }

				return null;
			}
		}
		public string ReplaceStringByIndex(string original, string replaceWith, int replaceIndex)
		{
			if (!string.IsNullOrEmpty(original))
			{
				if (original.Length >= (replaceIndex + replaceWith.Length))
				{
					StringBuilder rev = new StringBuilder(original);
					rev.Remove(replaceIndex, replaceWith.Length);
					rev.Insert(replaceIndex, replaceWith);
					return rev.ToString();
				}
				else
				{
					return "*******";
				}
			}
			else
			{
				return "";
			}
		}

		public string StripHtml(string Txt)
		{
			return Regex.Replace(Txt, "<(.|\\n)*?>", string.Empty);
		}

		public string ClearHtmlText(string htmlText)
		{
			htmlText = htmlText.Replace("nbsp;", "");
			htmlText = htmlText.Replace("&amp;", "");
			htmlText = htmlText.Replace("&amp;", "");
			htmlText = htmlText.Replace("&quot;", "");
			htmlText = System.Net.WebUtility.HtmlDecode(htmlText);
			htmlText = htmlText.Replace("'", "");
			htmlText = htmlText.Replace("\"", "");
			return Regex.Replace(htmlText, "<.*?>", string.Empty);
		}

		public static bool TrySetProperty(object obj, string property, object value)
		{
			var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
			if (prop != null && prop.CanWrite)
			{
				prop.SetValue(obj, value, null);
				return true;
			}
			return false;
		}
	}
}
