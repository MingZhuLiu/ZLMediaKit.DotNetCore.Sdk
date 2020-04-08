using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace STRealVideo.Lib.Common
{
    public static class Tools
    {

        /// <summary>
        /// 从此实例检索子数组
        /// </summary>
        /// <param name="source">要检索的数组</param>
        /// <param name="startIndex">起始索引号</param>
        /// <param name="length">检索最大长度</param>
        /// <returns>与此实例中在 startIndex 处开头、长度为 length 的子数组等效的一个数组</returns>
        public static Array SubArray(this Array source, Int32 startIndex, Int32 length)
        {
            if (startIndex < 0 || startIndex > source.Length || length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Array Destination;
            if (startIndex + length <= source.Length)
            {
                Destination = Array.CreateInstance(source.GetType(), length);
                Array.Copy(source, startIndex, Destination, 0, length);
            }
            else
            {
                Destination = Array.CreateInstance(source.GetType(), source.Length - startIndex);
                Array.Copy(source, startIndex, Destination, 0, source.Length - startIndex);
            }

            return Destination;
        }

        public static string GetDescription(Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return string.Empty;
        }

        // using System.Security.Cryptography;
        public static string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串
            return sBuilder.ToString();
        }




        public static String URLEncode(String input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return string.Empty;
            return System.Web.HttpUtility.UrlEncode(input, Encoding.UTF8);
        }

        public static String URIEncode(String input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return string.Empty;
            return Uri.EscapeDataString(input);
        }


        public static String URLDecode(String input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return string.Empty;
            return System.Web.HttpUtility.UrlDecode(input, Encoding.UTF8);
        }

        public static String URLDecode(String input, Encoding encoding)
        {
            if (String.IsNullOrWhiteSpace(input))
                return string.Empty;
            return System.Web.HttpUtility.UrlDecode(input, encoding);
        }

        public static String HTMLEncode(String input)
        {
            return System.Web.HttpUtility.HtmlEncode(input);
        }

        public static String HTMLDecode(String input)
        {
            return System.Web.HttpUtility.HtmlDecode(input);
        }

        public static String TimeConvert2NationSystem(String inputTime)
        {
            DateTime dt = DateTime.Parse(inputTime);
            //dt.AddHours(8);
            String result = "";
            result += dt.DayOfWeek.ToString().Substring(0, 3);
            result += (" " + dt.ToString("MMM", CultureInfo.CreateSpecificCulture("en-GB")));
            result += (" " + dt.Day);
            result += (" " + dt.Year);
            result += (" " + dt.ToString("HH:mm:ss"));
            result += (" GMT+0800 (中国标准时间)");
            return result;
        }

      

        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<String, String> GetQueryString(string queryString)
        {
            return GetQueryString(queryString, Encoding.UTF8, true);
        }


        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<String, String> GetQueryString(string queryString, Encoding encoding)
        {
            return GetQueryString(queryString, encoding, true);
        }
        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        private static Dictionary<String, String> GetQueryString(string queryString, Encoding encoding, bool isEncoded)
        {
            if (queryString.Contains("?"))
                queryString = queryString.Substring(queryString.IndexOf("?"));
            queryString = queryString.Replace("?", "");
            queryString = queryString.Replace("&amp;", "&");
            Dictionary<String, String> result = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        if (result.ContainsKey(URLDecode(key, encoding)))
                            result[URLDecode(key, encoding)] = URLDecode(value, encoding);
                        else
                            result.Add(URLDecode(key, encoding), URLDecode(value, encoding));
                    }
                    else
                    {
                        if (result.ContainsKey(key))
                            result[key] = value;
                        else
                            result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        if (result.ContainsKey(key))
                            result[key] = string.Empty;
                        else
                            result.Add(key, string.Empty);
                    }
                }
            }
            return result;
        }

        public static string ConvertTimeSpan2Str(TimeSpan ts)
        {
            string str = "";
            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString() + "小时 " + ts.Minutes.ToString() + "分钟 " + ts.Seconds + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString() + "分钟 " + ts.Seconds + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = ts.Seconds + "秒";
            }
            return str;
        }





    }
}
