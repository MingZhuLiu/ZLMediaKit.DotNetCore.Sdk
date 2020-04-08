using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
    /// <summary>
    /// ssrc rtp信息
    /// </summary>
    public class SsrcInfo
    {
        public int code { get; set; }

        /// <summary>
        /// 是否存在
        /// </summary>
        public bool exist { get; set; }

        /// <summary>
        /// 推流客户端ip
        /// </summary>
        public string peer_ip { get; set; }

        /// <summary>
        /// 客户端端口号
        /// </summary>
        public int peer_port { get; set; }
    }
}
