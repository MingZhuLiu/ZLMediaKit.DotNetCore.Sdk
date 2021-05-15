using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{

    /// <summary>
    /// 轨
    /// </summary>
    public class Track
    {
        /// <summary>
        /// 编码流类型
        /// </summary>
        public CodecStreamType codec_id { get; set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        public CodecType codec_type { get; set; }

        public string codec_id_name{get;set;}

        public int? height{get;set;}
        public int? width{get;set;}
        public int? fps{get;set;}

        public int? sample_bit{get;set;}
        public int? sample_rate{get;set;}
        public int? channels{get;set;}


        /// <summary>
        /// 轨道是否准备就绪
        /// </summary>
        public bool ready { get; set; }
    }
}
