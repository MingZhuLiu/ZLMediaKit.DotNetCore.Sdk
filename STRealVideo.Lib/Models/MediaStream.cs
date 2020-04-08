using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{

    /// <summary>
    /// 媒体流
    /// </summary>
    public class MediaStream
    {

        /// <summary>
        /// 应用名
        /// </summary>
        public string app { get; set; }

        /// <summary>
        /// 本协议观看人数
        /// </summary>
        public int readerCount { get; set; }

        /// <summary>
        /// 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv
        /// </summary>
        public int totalReaderCount { get; set; }

        /// <summary>
        /// 协议
        /// </summary>
        public string schema { get; set; }


        /// <summary>
        /// 流id
        /// </summary>
        public string stream { get; set; }


        /// <summary>
        /// 虚拟主机名
        /// </summary>
        public string vhost { get; set; }

        /// <summary>
        /// 轨道列表
        /// </summary>
        public List<Track> tracks { get; set; }
    }
}
