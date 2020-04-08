using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{

    /// <summary>
    /// 会话
    /// </summary>
    public class Session
    {
        /// <summary>
        /// 该tcp链接唯一id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 本机网卡ip
        /// </summary>
        public string local_ip { get; set; }

        /// <summary>
        /// 本机端口号	(这是个rtmp播放器或推流器)
        /// </summary>
        public int local_port { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        public string peer_ip { get; set; }

        /// <summary>
        /// 客户端端口号
        /// </summary>
        public int peer_port { get; set; }


        /// <summary>
        /// 客户端TCPSession typeid
        /// </summary>
        public string typeid { get; set; }
    }
}
