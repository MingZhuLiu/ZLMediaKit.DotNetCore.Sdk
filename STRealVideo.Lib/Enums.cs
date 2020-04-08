using System;
using System.Collections.Generic;
using System.Text;

namespace STRealVideo.Lib
{
    /// <summary>
    /// RTP拉流方式
    /// </summary>
   public enum RTPPullType
    {
        /// <summary>
        /// TCP
        /// </summary>
        TCP=0,

        /// <summary>
        /// UDP
        /// </summary>
        UDP=1,

        /// <summary>
        /// 组播
        /// </summary>
        Multicast=2
    }

    /// <summary>
    /// 编码流类型
    /// </summary>
    public enum CodecStreamType
    {
        /// <summary>
        /// H264
        /// </summary>
        H264=0,

        /// <summary>
        /// H265
        /// </summary>
        H265 = 1,

        /// <summary>
        /// AAC
        /// </summary>
        AAC = 2
    }

    /// <summary>
    /// 编码类型
    /// </summary>
    public enum CodecType
    {

        /// <summary>
        /// 视频
        /// </summary>
        Video=0,

        /// <summary>
        /// 音频
        /// </summary>
        Audio=1
    }
}
