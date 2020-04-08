using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib.Models
{
    public class RecordResponse
    {
        public int code { get; set; }

        /// <summary>
        ///  成功与否
        /// </summary>
        public bool result { get; set; }

        public string msg { get; set; }
    }
}
