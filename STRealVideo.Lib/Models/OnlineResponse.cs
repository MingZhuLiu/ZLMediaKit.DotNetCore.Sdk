using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
   public class OnlineResponse
    {
        public int code { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool online { get; set; }
    }
}
