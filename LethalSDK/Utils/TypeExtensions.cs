using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LethalSDK.Utils
{
    public static class TypeExtensions
    {
        public static readonly Dictionary<removeType, string> regexes = new Dictionary<removeType, string>
        {
            {removeType.Normal, "[^a-zA-Z0-9 ,.!?_-]"},
            {removeType.Serializable, "[^a-zA-Z0-9 .!_-]"},
            {removeType.Keyword, "[^a-zA-Z0-9._-]"},
            {removeType.Path, "[^a-zA-Z0-9 ,.!_/-]"},
            {removeType.SerializablePath, "[^a-zA-Z0-9 .!_/-]"}
        };
        public static string RemoveNonAlphanumeric(this string input)
        {
            if(input != null)
            {
                return Regex.Replace(input, regexes[removeType.Normal], string.Empty);
            }
            return string.Empty;
        }
        public static string[] RemoveNonAlphanumeric(this string[] input)
        {
            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = Regex.Replace(input[i], regexes[removeType.Normal], string.Empty);
                }
                return input;
            }
            return new string[0];
        }
        public static string RemoveNonAlphanumeric(this string input, removeType removeType = removeType.Normal)
        {
            if (input != null)
            {
                return Regex.Replace(input, regexes[removeType], string.Empty);
            }
            return string.Empty;
        }
        public static string[] RemoveNonAlphanumeric(this string[] input, removeType removeType = removeType.Normal)
        {
            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = Regex.Replace(input[i], regexes[removeType], string.Empty);
                }
                return input;
            }
            return new string[0];
        }
        public static string RemoveNonAlphanumeric(this string input, int removeType = 0)
        {
            if (input != null)
            {
                return Regex.Replace(input, regexes[(removeType)removeType], string.Empty);
            }
            return string.Empty;
        }
        public static string[] RemoveNonAlphanumeric(this string[] input, int removeType = 0)
        {
            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = Regex.Replace(input[i], regexes[(removeType)removeType], string.Empty);
                }
                return input;
            }
            return new string[0];
        }
        public enum removeType
        {
            Normal = 0,
            Serializable = 1,
            Keyword = 2,
            Path = 3,
            SerializablePath = 4
        }
    }
}
