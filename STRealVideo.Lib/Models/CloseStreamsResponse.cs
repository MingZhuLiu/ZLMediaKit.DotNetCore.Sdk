using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
    public class CloseStreamsResponse
    {
        public int code { get; set; }

        /// <summary>
        ///  筛选命中的流个数
        /// </summary>
        public int count_hit { get; set; }

        /// <summary>
        /// 被关闭的流个数，可能小于count_hit
        /// </summary>
        public int count_closed { get; set; }
    }
}
