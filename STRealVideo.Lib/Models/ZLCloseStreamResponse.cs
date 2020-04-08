using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{

    /// <summary>
    /// 关闭流返回值
    /// </summary>
    public class ZLCloseStreamResponse
    {
        public int code { get; set; }

        /// <summary>
        /// 0:成功，-1:关闭失败，-2:该流不存在
        /// </summary>
        public int result { get; set; }

        public string msg { get; set; }

        public ZLCloseStreamResponse Failed(string msg)
        {
            this.code = -300;
            this.msg = msg;
            this.result = -1;
            return this;
        }
    }
}
