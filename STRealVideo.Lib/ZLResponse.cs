using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib
{

    /// <summary>
    /// ZLServer返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ZLResponse<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }


        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }

        public ZLResponse<T> Failed(string msg)
        {
            this.code = -300;
            this.msg = msg;
            this.data = default(T);
            return this;
        }
    }
}
