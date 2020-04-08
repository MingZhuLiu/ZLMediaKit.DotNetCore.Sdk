using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
   public class KickSessionsResponse
    {
        public int code { get; set; }

        /// <summary>
        /// 筛选命中客户端个数
        /// </summary>
        public int count_hit { get; set; }
        public string msg { get; set; }
    }
}
