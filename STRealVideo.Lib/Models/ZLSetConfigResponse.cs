using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
    /// <summary>
    /// 设置服务器配置返回值
    /// </summary>
    public class ZLSetConfigResponse
    {
        /// <summary>
        ///  配置项变更个数
        /// </summary>
        public int changed { get; set; }

        /// <summary>
        /// 代表成功
        /// </summary>
        public int code { get; set; }
    }
}
