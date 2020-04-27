using AntiPlagiatus.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AntiPlagiatus.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static bool IsInt32(this string text)
        {
            int outInt = 0;
            return Int32.TryParse(text, out outInt);
        }
        public static Status ParseReportStatus(this string status)
        {
            Status enumResult = Status.None;
            Enum.TryParse(status, out enumResult);
            return enumResult;
        }
        public static TextOrigin ParseContentTextOrigin(this string contentOrigin)
        {
            TextOrigin enumResult = TextOrigin.None;
            Enum.TryParse(contentOrigin, out enumResult);
            return enumResult;
        }
        public static IgnoreType ParseIgnoreRuleType(this string ruleType)
        {
            IgnoreType enumResult = IgnoreType.None;
            Enum.TryParse(ruleType, out enumResult);
            return enumResult;
        }
        public static Theme ParseUserTheme(this string theme)
        {
            Theme enumResult = Theme.Dark;
            Enum.TryParse(theme, out enumResult);
            return enumResult;
        }

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
