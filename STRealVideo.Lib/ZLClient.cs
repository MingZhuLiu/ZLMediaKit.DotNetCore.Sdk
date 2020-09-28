using STRealVideo.Lib.Common;
using STRealVideo.Lib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace STRealVideo.Lib
{


    public class ZLClient
    {
        private string secret = null;
        private string apiUrl = null;
        private QxHttpClient httpClient = null;

        private QxHttpPara NewPara { get => string.IsNullOrWhiteSpace(secret) ? new QxHttpPara() : new QxHttpPara("secret", secret); }

        /// <summary>
        /// 构造方法  ZLMedia客户端库
        /// </summary>
        /// <param name="apiUrl">服务器API地址</param>
        /// <param name="secret">密钥</param>
        public ZLClient(string apiUrl, string secret = null,Encoding encoding=null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (String.IsNullOrWhiteSpace(secret))
            {
                throw new Exception("Secret is null!");
            }

            else
            {
                if (apiUrl.LastIndexOf("/") == apiUrl.Length - 1)
                {
                    apiUrl = apiUrl.Substring(0, apiUrl.Length - 1);
                }
            }
            this.secret = secret;
            this.apiUrl = apiUrl;
            httpClient = new QxHttpClient();
            if (encoding == null)
                httpClient.DefaultEncoding = Encoding.UTF8;
            else
                httpClient.DefaultEncoding = encoding;
        }


        public QxHttpClient HttpClient => httpClient;

        /// <summary>
        /// 获取各epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public ZLResponse<List<ThreadsLoad>> getThreadsLoad()
        {
            ZLResponse<List<ThreadsLoad>> response = new ZLResponse<List<ThreadsLoad>>();
            var json = httpClient.Post<String>(apiUrl + "/index/api/getThreadsLoad", "", QxHttpClient.ContentType.x_www_form_unlencoded);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<List<ThreadsLoad>>>(json);
            }
            return response;
        }

        /// <summary>
        /// 获取各后台epoll(或select)线程负载以及延时
        /// </summary>
        /// <returns></returns>
        public ZLResponse<List<ThreadsLoad>> getWorkThreadsLoad()
        {
            ZLResponse<List<ThreadsLoad>> response = new ZLResponse<List<ThreadsLoad>>();
            var json = httpClient.Post<String>(apiUrl + "/index/api/getWorkThreadsLoad", "", QxHttpClient.ContentType.x_www_form_unlencoded);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<List<ThreadsLoad>>>(json);
            }
            return response;
        }



        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <returns></returns>
        public ZLResponse<List<Dictionary<String, String>>> getServerConfig()
        {
            ZLResponse<List<Dictionary<String, String>>> response = new ZLResponse<List<Dictionary<String, String>>>();
            var json = httpClient.Post<String>(apiUrl + "/index/api/getServerConfig", NewPara, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<List<Dictionary<String, String>>>>(json);
            }
            return response;
        }

        /// <summary>
        /// 设置服务器配置
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public ZLSetConfigResponse setServerConfig(string key, string value)
        {
            ZLSetConfigResponse response = new ZLSetConfigResponse();
            if (String.IsNullOrWhiteSpace(key))
            {
                response.code = -300;
                return response;
            }
            var json = httpClient.Post<String>(apiUrl + "/index/api/setServerConfig", NewPara.AddPara(key, value), QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.code = -300;
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLSetConfigResponse>(json);
            }
            return response;
        }

        /// <summary>
        /// 重启服务器,只有Daemon方式才能重启，否则是直接关闭！
        /// </summary>
        /// <returns></returns>
        public ZLResponse<int> restartServer()
        {
            ZLResponse<int> response = new ZLResponse<int>();
            var json = httpClient.Post<String>(apiUrl + "/index/api/restartServer", NewPara, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<int>>(json);
            }
            return response;
        }

        /// <summary>
        /// 获取流列表，可选筛选参数
        /// </summary>
        /// <param name="schema">筛选协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">筛选虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">筛选应用名，例如 live</param>
        /// <returns></returns>
        public ZLResponse<List<MediaStream>> getMediaList(string schema = null, string vhost = null, string app = null)
        {
            ZLResponse<List<MediaStream>> response = null;
            var para = NewPara;
            if (!String.IsNullOrWhiteSpace(schema))
                para.AddPara("schema", schema);

            if (!String.IsNullOrWhiteSpace(vhost))
                para.AddPara("vhost", vhost);

            if (!String.IsNullOrWhiteSpace(app))
                para.AddPara("app", app);

            var json = httpClient.Post<String>(apiUrl + "/index/api/getMediaList", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response = new ZLResponse<List<MediaStream>>().Failed("Server returned empty!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<List<MediaStream>>>(json);
            }
            return response;
        }


        /// <summary>
        /// 关闭流(目前所有类型的流都支持关闭)
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 test</param>
        /// <param name="force">是否强制关闭(有人在观看是否还关闭)</param>
        /// <returns></returns>
        public ZLCloseStreamResponse closeStream(string schema, string vhost, string app, string stream, bool? force = false)
        {

            ZLCloseStreamResponse response = new ZLCloseStreamResponse();

            var para = NewPara;
            if (String.IsNullOrWhiteSpace(schema))
                return response.Failed("schema is null!");
            if (String.IsNullOrWhiteSpace(vhost))
                return response.Failed("vhost is null!");
            if (String.IsNullOrWhiteSpace(app))
                return response.Failed("app is null!");
            if (String.IsNullOrWhiteSpace(stream))
                return response.Failed("stream is null!");
            para.AddPara("schema", schema).
                AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream)
            .AddPara("force", force.HasValue && force.Value ? "1" : "0");

            var json = httpClient.Post<String>(apiUrl + "/index/api/close_stream", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLCloseStreamResponse>(json);
            }
            return response;
        }


        public CloseStreamsResponse closeStreams(string schema = null, string vhost = null, string app = null, string stream = null, bool? force = false)
        {

            CloseStreamsResponse response = new CloseStreamsResponse();
            response.code = -300;
            var para = NewPara;
            if (!String.IsNullOrWhiteSpace(schema))
                para.AddPara("schema", schema);
            if (String.IsNullOrWhiteSpace(vhost))
                para.AddPara("vhost", vhost);
            if (String.IsNullOrWhiteSpace(app))
                para.AddPara("app", app);
            if (String.IsNullOrWhiteSpace(stream))
                para.AddPara("schema", schema);
            para.AddPara("force", force.HasValue && force.Value ? "1" : "0");

            var json = httpClient.Post<String>(apiUrl + "/index/api/close_streams", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                return response;
            }
            else
            {
                response = JsonSerializer.Deserialize<CloseStreamsResponse>(json);
            }
            return response;
        }

        /// <summary>
        /// 获取所有TcpSession列表(获取所有tcp客户端相关信息)
        /// </summary>
        /// <param name="local_port">筛选本机端口，例如筛选rtsp链接：554</param>
        /// <param name="peer_ip">筛选客户端ip</param>
        /// <returns></returns>
        public ZLResponse<List<Session>> getAllSession(int? local_port = null, String peer_ip = null)
        {
            ZLResponse<List<Session>> response = new ZLResponse<List<Session>>();
            var para = NewPara;
            if (local_port.HasValue)
            {
                para.AddPara("local_port", local_port.Value.ToString());
            }
            if (!String.IsNullOrWhiteSpace(peer_ip))
            {
                para.AddPara("peer_ip", peer_ip);
            }

            var json = httpClient.Post<String>(apiUrl + "/index/api/getAllSession", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<List<Session>>>(json);
            }
            return response;
        }


        /// <summary>
        /// 重启服务器,只有Daemon方式才能重启，否则是直接关闭！
        /// </summary>
        /// <param name="id">会话Id</param>
        /// <returns></returns>
        public ZLResponse<int> kickSession(string id)
        {
            ZLResponse<int> response = new ZLResponse<int>();
            if (string.IsNullOrWhiteSpace(id))
                return response.Failed("id is null!");
            var json = httpClient.Post<String>(apiUrl + "/index/api/kick_session", NewPara.AddPara("Id", id), QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<int>>(json);
            }
            return response;
        }

        /// <summary>
        /// 断开tcp连接，比如说可以断开rtsp、rtmp播放器等 
        /// </summary>
        /// <param name="local_port">筛选本机端口，例如筛选rtsp链接：554</param>
        /// <param name="peer_ip">筛选客户端ip</param>
        /// <returns></returns>
        public KickSessionsResponse kickSessions(int? local_port = null, String peer_ip = null)
        {
            KickSessionsResponse response = new KickSessionsResponse();
            var para = NewPara;
            if (local_port.HasValue)
            {
                para.AddPara("local_port", local_port.Value.ToString());
            }
            if (!String.IsNullOrWhiteSpace(peer_ip))
            {
                para.AddPara("peer_ip", peer_ip);
            }

            var json = httpClient.Post<String>(apiUrl + "/index/api/kick_sessions", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.code = -300;
                response.msg = "Server return empty Or Connect failed!";
            }
            else
            {
                response = JsonSerializer.Deserialize<KickSessionsResponse>(json);
            }
            return response;
        }


        /// <summary>
        /// 动态添加rtsp/rtmp拉流代理(只支持H264/H265/aac负载)
        /// </summary>
        /// <param name="vhost">添加的流的虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">添加的流的应用名，例如live</param>
        /// <param name="stream">添加的流的id名，例如test</param>
        /// <param name="url">拉流地址，例如rtmp://live.hkstv.hk.lxdns.com/live/hks2</param>
        /// <param name="enable_rtsp">是否转rtsp</param>
        /// <param name="enable_rtmp">是否转rtmp</param>
        /// <param name="enable_hls">是否转hls</param>
        /// <param name="enable_mp4">是否mp4录制</param>
        /// <param name="rtp_type">rtsp拉流时，拉流方式，tcp，udp，组播</param>
        /// <returns></returns>
        public ZLResponse<BodyKey> addStreamProxy(string vhost, string app, string stream, string url, bool? enable_hls = null, bool? enable_mp4 = null, RTPPullType? rtp_type = null)
        {
            ZLResponse<BodyKey> response = new ZLResponse<BodyKey>();

            var para = NewPara;
            if (String.IsNullOrWhiteSpace(vhost))
                return response.Failed("vhost is null!");
            if (String.IsNullOrWhiteSpace(app))
                return response.Failed("app is null!");
            if (String.IsNullOrWhiteSpace(stream))
                return response.Failed("stream is null!");
            para.AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream)
            .AddPara("url", url);
            if (enable_hls.HasValue)
                para.AddPara("enable_hls", enable_hls.Value ? "1" : "0");
            if (enable_mp4.HasValue)
                para.AddPara("enable_mp4", enable_mp4.Value ? "1" : "0");
            if (rtp_type.HasValue)
                para.AddPara("rtp_type", ((int)rtp_type).ToString());

            var json = httpClient.Post<String>(apiUrl + "/index/api/addStreamProxy", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<BodyKey>>(json);
            }
            return response;
        }


        /// <summary>
        /// 关闭拉流代理
        /// </summary>
        /// <param name="key">addStreamProxy接口返回的key</param>
        /// <returns></returns>
        public ZLResponse<BodyFlag> delStreamProxy(string key)
        {
            ZLResponse<BodyFlag> response = new ZLResponse<BodyFlag>();

            var para = NewPara;
            if (String.IsNullOrWhiteSpace(key))
                return response.Failed("key is null!");
            para.AddPara("key", key);

            var json = httpClient.Post<String>(apiUrl + "/index/api/delStreamProxy", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<BodyFlag>>(json);
            }
            return response;
        }

        /// <summary>
        /// 通过fork FFmpeg进程的方式拉流代理，支持任意协议
        /// </summary>
        /// <param name="src_url">FFmpeg拉流地址,支持任意协议或格式(只要FFmpeg支持即可)</param>
        /// <param name="dst_url">FFmpeg rtmp推流地址，一般都是推给自己，例如rtmp://127.0.0.1/live/stream_form_ffmpeg</param>
        /// <param name="timeout_ms">FFmpeg推流成功超时时间</param>
        /// <returns></returns>
        public ZLResponse<BodyKey> addFFmpegSource(string src_url, string dst_url, int timeout_ms)
        {
            ZLResponse<BodyKey> response = new ZLResponse<BodyKey>();
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(src_url))
                return response.Failed("src_url is null!");
            if (String.IsNullOrWhiteSpace(dst_url))
                return response.Failed("dst_url is null!");
            para.AddPara("src_url", src_url);
            para.AddPara("dst_url", dst_url);
            para.AddPara("timeout_ms", timeout_ms.ToString());
            var json = httpClient.Post<String>(apiUrl + "/index/api/addFFmpegSource", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<BodyKey>>(json);
            }
            return response;
        }

        /// <summary>
        /// 关闭ffmpeg拉流代理
        /// </summary>
        /// <param name="key">addFFmpegSource接口返回的key</param>
        /// <returns></returns>
        public ZLResponse<BodyFlag> delFFmpegSource(string key)
        {
            ZLResponse<BodyFlag> response = new ZLResponse<BodyFlag>();

            var para = NewPara;
            if (String.IsNullOrWhiteSpace(key))
                return response.Failed("key is null!");
            para.AddPara("key", key);

            var json = httpClient.Post<String>(apiUrl + "/index/api/delFFmpegSource", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<BodyFlag>>(json);
            }
            return response;
        }


        /// <summary>
        /// 判断直播流是否在线
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="vhost"></param>
        /// <param name="app"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public OnlineResponse isMediaOnline(string schema, string vhost, string app, string stream)
        {
            OnlineResponse response = new OnlineResponse();
            response.code = -300;
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(schema))
                return response;
            if (String.IsNullOrWhiteSpace(vhost))
                return response;
            if (String.IsNullOrWhiteSpace(app))
                return response;
            if (String.IsNullOrWhiteSpace(stream))
                return response;
            para.AddPara("schema", schema).
                AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream);

            var json = httpClient.Post<String>(apiUrl + "/index/api/isMediaOnline", para, QxHttpClient.ContentType.json);
            if (!String.IsNullOrWhiteSpace(json))
            {
                response = JsonSerializer.Deserialize<OnlineResponse>(json);
            }
            return response;
        }



        /// <summary>
        /// 获取流相关信息
        /// </summary>
        /// <param name="schema">协议，例如 rtsp或rtmp</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        public MediaInfo getMediaInfo(string schema, string vhost, string app, string stream)
        {
            MediaInfo response = new MediaInfo();
            response.code = -300;
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(schema))
                return response;
            if (String.IsNullOrWhiteSpace(vhost))
                return response;
            if (String.IsNullOrWhiteSpace(app))
                return response;
            if (String.IsNullOrWhiteSpace(stream))
                return response;
            para.AddPara("schema", schema).
                AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream);

            var json = httpClient.Post<String>(apiUrl + "/index/api/getMediaInfo", para, QxHttpClient.ContentType.json);
            if (!String.IsNullOrWhiteSpace(json))
            {
                response = JsonSerializer.Deserialize<MediaInfo>(json);
            }
            return response;
        }

        /// <summary>
        /// 获取rtp代理时的某路ssrc rtp信息
        /// </summary>
        /// <param name="ssrc">RTP的ssrc，16进制字符串</param>
        /// <returns></returns>
        public SsrcInfo getSsrcInfo(string ssrc)
        {
            SsrcInfo response = new SsrcInfo();

            var para = NewPara;
            response.code = -300;
            if (String.IsNullOrWhiteSpace(ssrc))
                return response;
            para.AddPara("ssrc", ssrc);

            var json = httpClient.Post<String>(apiUrl + "/index/api/getSsrcInfo", para, QxHttpClient.ContentType.json);
            if (!String.IsNullOrWhiteSpace(json))
            {
                response = JsonSerializer.Deserialize<SsrcInfo>(json);
            }
            return response;
        }



        /// <summary>
        /// 搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
        /// </summary>
        /// <param name="vhost">流的虚拟主机名</param>
        /// <param name="app">流的应用名</param>
        /// <param name="stream">流的ID</param>
        /// <param name="period">流的录像日期，格式为2020-02-01,如果不是完整的日期，那么是搜索录像文件夹列表，否则搜索对应日期下的mp4文件列表</param>
        /// <returns></returns>
        public ZLResponse<Mp4RecordFile> getMp4RecordFile(string vhost, string app, string stream, string period)
        {
            ZLResponse<Mp4RecordFile> response = new ZLResponse<Mp4RecordFile>();
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(vhost))
                return response.Failed("vhost is null!");
            if (String.IsNullOrWhiteSpace(app))
                return response.Failed("app is null!");
            if (String.IsNullOrWhiteSpace(stream))
                return response.Failed("stream is null!");
            if (String.IsNullOrWhiteSpace(period))
                return response.Failed("period is null!");
            para.AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream)
            .AddPara("period", period);

            var json = httpClient.Post<String>(apiUrl + "/index/api/getMp4RecordFile", para, QxHttpClient.ContentType.json);
            if (String.IsNullOrWhiteSpace(json))
            {
                response.Failed("Server return empty Or Connect failed!");
            }
            else
            {
                response = JsonSerializer.Deserialize<ZLResponse<Mp4RecordFile>>(json);
            }
            return response;
        }

        /// <summary>
        /// 开始录制hls或MP4
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <param name="customized_path">录像保存目录</param>
        /// <returns></returns>
        public RecordResponse startRecord(int type, string vhost, string app, string stream, string customized_path = null)
        {
            RecordResponse response = new RecordResponse();
            response.code = -300;
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(vhost))
                return response;
            if (String.IsNullOrWhiteSpace(app))
                return response;
            if (String.IsNullOrWhiteSpace(stream))
                return response;
            para.AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream)
            .AddPara("type", type.ToString());
            if (!String.IsNullOrWhiteSpace(customized_path))
                para.AddPara("customized_path", customized_path);

            var json = httpClient.Post<String>(apiUrl + "/index/api/startRecord", para, QxHttpClient.ContentType.json);
            if (!String.IsNullOrWhiteSpace(json))
            {
                response = JsonSerializer.Deserialize<RecordResponse>(json);
            }
            return response;
        }

        /// <summary>
        /// 停止录制流
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        public RecordResponse stopRecord(int type, string vhost, string app, string stream)
        {
            RecordResponse response = new RecordResponse();
            response.code = -300;
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(vhost))
                return response;
            if (String.IsNullOrWhiteSpace(app))
                return response;
            if (String.IsNullOrWhiteSpace(stream))
                return response;
            para.AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream)
            .AddPara("type", type.ToString());

            var json = httpClient.Post<String>(apiUrl + "/index/api/stopRecord", para, QxHttpClient.ContentType.json);
            if (!String.IsNullOrWhiteSpace(json))
            {
                response = JsonSerializer.Deserialize<RecordResponse>(json);
            }
            return response;
        }

        /// <summary>
        /// 停止录制流
        /// </summary>
        /// <param name="type">0为hls，1为mp4</param>
        /// <param name="vhost">虚拟主机，例如__defaultVhost__</param>
        /// <param name="app">应用名，例如 live</param>
        /// <param name="stream">流id，例如 obs</param>
        /// <returns></returns>
        public RecordingResponse isRecording(int type, string vhost, string app, string stream)
        {
            RecordingResponse response = new RecordingResponse();
            response.code = -300;
            var para = NewPara;
            if (String.IsNullOrWhiteSpace(vhost))
                return response;
            if (String.IsNullOrWhiteSpace(app))
                return response;
            if (String.IsNullOrWhiteSpace(stream))
                return response;
            para.AddPara("vhost", vhost)
            .AddPara("app", app)
            .AddPara("stream", stream)
            .AddPara("type", type.ToString());

            var json = httpClient.Post<String>(apiUrl + "/index/api/isRecording", para, QxHttpClient.ContentType.json);
            if (!String.IsNullOrWhiteSpace(json))
            {
                response = JsonSerializer.Deserialize<RecordingResponse>(json);
            }
            return response;
        }


        public ZLResponse<byte[]> getSnap(string url,int timeout_sec,int expire_sec)
        {
            ZLResponse<byte[]> response = new ZLResponse<byte[]>();

            var para = NewPara;
            if (String.IsNullOrWhiteSpace(url))
                return response.Failed("url is null!");
            para.AddPara("url", url);
            para.AddPara("timeout_sec", timeout_sec+"");
            para.AddPara("expire_sec", expire_sec+"");

            var data = httpClient.Post<byte[]>(apiUrl + "/index/api/getSnap", para, QxHttpClient.ContentType.json);
            if(data==null||data.Length==0)
            {
                response.code = -300;
                response.Failed("Get SNAP Failed,Server Return NULL!");

            }
            else
            {
                response.code = 0;
                response.data = data;
            }
            
            return response;
        }

    }
}
