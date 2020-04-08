using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace STRealVideo.Lib.Common
{
    public class QxHttpClient
    {
        public HttpClient _httpClient = null;
        private readonly HttpClientHandler handler = null;

        /// <summary>
        /// 默认语言
        /// </summary>
        public Encoding DefaultEncoding = Encoding.GetEncoding("GB2312");


        public delegate void HttpProgress(int progress);
        //public event HttpProgress HttpDownloadProgressEvent;

        public QxHttpClient()
        {
            cookieContainer = new CookieContainer();
            handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true };
            _httpClient = new HttpClient();

        }
        public QxHttpClient(string proxyIp, int proxyPort, string proxyAccount, string proxyPassword)
        {
            cookieContainer = new CookieContainer();

            WebProxy wp = new WebProxy(proxyIp, proxyPort);
            //代理地址
            //设置身份验证凭据 账号 密码
            wp.Credentials = new NetworkCredential(proxyAccount, proxyPassword);

            handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true, Proxy = wp };
            _httpClient = new HttpClient();

        }


        /// <summary>
        /// Content-Type
        /// </summary>
        public enum ContentType
        {
            x_www_form_unlencoded,

            json
        }

        #region Head相关

        public void AddHeader(string key, String value)
        {

            RemoveHeader(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);

        }

        public Dictionary<String, String> Heads
        {
            get
            {
                Dictionary<String, String> head = new Dictionary<string, string>();
                foreach (var item in _httpClient.DefaultRequestHeaders)
                {
                    head.Add(item.Key, String.Join(";", item.Value));
                }
                return head;
            }
        }

        public void RemoveHeader(string key)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(key))
                _httpClient.DefaultRequestHeaders.Remove(key);
        }


        #endregion
        #region Cookie相关

        public CookieContainer cookieContainer = null;
        public Dictionary<String, String> Cookies
        {
            get
            {
                Dictionary<String, String> cookies = new Dictionary<string, string>();
                if (cookieContainer != null)
                {

                    Hashtable table = (Hashtable)cookieContainer.GetType().InvokeMember("m_domainTable",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                        System.Reflection.BindingFlags.Instance, null, cookieContainer, new object[] { });
                    foreach (object pathList in table.Values)
                    {
                        SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                            | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                        foreach (CookieCollection colCookies in lstCookieCol.Values)
                            foreach (Cookie c in colCookies)
                            {

                                if (cookies.ContainsKey(c.Name))
                                    cookies[c.Name] = c.Value;
                                else
                                    cookies.Add(c.Name, c.Value);
                            }
                    }
                }
                return cookies;
            }
        }

        #endregion
        public T Get<T>(string url, bool ignoreHttpErrorrCode = false)
        {
            Object result = null;
            var httpResult = _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).Result;
            if (ignoreHttpErrorrCode || httpResult.StatusCode == HttpStatusCode.OK)
            {
                var buffer = httpResult.Content.ReadAsByteArrayAsync().Result;
                result = CastObj<T>(buffer);
            }
            else
            {
                //return default(T);
            }
            return (T)result;
        }
        public T Post<T>(string url, string data, ContentType contentType, bool ignoreHttpErrorrCode = false)
        {
            Object result = null;
            var ct = contentType == ContentType.json ? "application/json" : "application/x-www-form-unlencoded";
            StringContent content = new StringContent(data, DefaultEncoding, ct);
            HttpResponseMessage httpResult = _httpClient.PostAsync(new Uri(url), content).Result;
            if (ignoreHttpErrorrCode || httpResult.StatusCode == HttpStatusCode.OK)
            {
                var buffer = httpResult.Content.ReadAsByteArrayAsync().Result;
                result = CastObj<T>(buffer);
            }
            else
            {
                //return default(T);
            }
            return (T)result;
        }

        public T Post<T>(string url, QxHttpPara data, ContentType contentType, bool ignoreHttpErrorrCode = false)
        {
            try
            {
                var reqData = string.Empty;
                if (contentType == ContentType.json)
                {
                    reqData = JsonSerializer.Serialize(data._data);
                }
                else

                {
                    reqData = data.ParaStr;
                }

                return Post<T>(url, reqData, contentType, ignoreHttpErrorrCode);

            }
            catch
            {
                return default(T);
            }
        }
        private T CastObj<T>(byte[] buffer)
        {
            if (buffer == null)
                return default(T);
            Object result = null;
            if (typeof(T) == typeof(String))
            {
                result = DefaultEncoding.GetString((byte[])buffer);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                result = buffer;
            }
            else //不是String 不是byte[] ,那肯定是对象,直接转为对象
            {
                var json = DefaultEncoding.GetString((byte[])buffer);
                result = JsonSerializer.Deserialize<T>(json);
            }
            return (T)result;
        }
    }

    public class QxHttpPara
    {
        public Dictionary<String, String> _data = null;
        public QxHttpPara()
        {
            _data = new Dictionary<string, string>();
        }
        public QxHttpPara(string key, string value) : this()
        {
            _data.Add(key, value);
        }
        public QxHttpPara AddPara(string key, string value)
        {
            if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);
            return this;
        }
        public QxHttpPara AddUrlEncodePara(string key, string value)
        {
            if (_data.ContainsKey(key))
                _data[key] = Tools.URLEncode(value);
            else
                _data.Add(key, Tools.URLEncode(value));
            return this;
        }

        public String ParaStr
        {
            get
            {
                StringBuilder result = new StringBuilder();
                if (_data.Count != 0)
                {
                    int index = 0;
                    foreach (var item in _data.Keys)
                    {
                        if (index != 0)
                            result.Append("&");
                        result.Append(item);
                        result.Append("=");
                        result.Append(Tools.URIEncode(_data[item]));
                        index++;
                    }
                }
                return result.ToString();
            }
        }
    }
}
