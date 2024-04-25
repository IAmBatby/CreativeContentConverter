using LethalLevelLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        public static string FirstToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            char[] chars = input.ToCharArray();
            if (char.IsLetter(chars[0]))
                chars[0] = char.ToUpper(chars[0]);
            return (new string(chars));
        }

        public static string UpperFirstLetters(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            bool upperNextChar = false;
            char[] chars = input.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (!char.IsLetter(chars[i]))
                    upperNextChar = true;
                if (char.IsLetter(chars[i]) && upperNextChar == true)
                {
                    chars[i] = char.ToUpper(chars[i]);
                    upperNextChar = false;
                }
            }

            return (new string(chars));
        }

        public static string ToBold(this string input)
        {
            return new string("<b>" + input + "</b>");
        }

        public static string Colorize(this string input)
        {
            string hexColor = "#" + "FFFFFF";
            return new string("<color=" + hexColor + ">" + input + "</color>");
        }

        public static string Colorize(this string input, Color color)
        {
            string hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
            return new string("<color=" + hexColor + ">" + input + "</color>");
        }
    }
}
