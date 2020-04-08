using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
    public class RecordingResponse
    {
        public int code { get; set; }

        /// <summary>
        ///  false:未录制,true:正在录制
        /// </summary>
        public bool status { get; set; }

        public string msg { get; set; }
    }
}
