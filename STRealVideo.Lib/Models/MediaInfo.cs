using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
    /// <summary>
    /// 媒体信息
    /// </summary>
    public class MediaInfo
    {
        public int code { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool online { get; set; }

        /// <summary>
        /// 本协议观看人数
        /// </summary>
        public int readerCount { get; set; }

        /// <summary>
        /// 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv
        /// </summary>
        public int totalReaderCount { get; set; }

        /// <summary>
        /// 轨道列表
        /// </summary>
        public List<Track> tracks { get; set; }
    }
}
