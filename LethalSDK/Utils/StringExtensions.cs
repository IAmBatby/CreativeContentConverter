using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LethalSDK
{
    public static class StringExtension
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);

        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        public static string SkipToLetters(this string input)
        {
            return new string(input.SkipWhile(c => !char.IsLetter(c)).ToArray());
        }

        public static string StripSpecialCharacters(this string input)
        {
            string returnString = string.Empty;

            foreach (char charmander in input)
                if ((!ConfigHelper.illegalCharacters.ToCharArray().Contains(charmander) && char.IsLetterOrDigit(charmander)) || charmander.ToString() == " ")
                    returnString += charmander;

            return returnString;
        }
    }
}
