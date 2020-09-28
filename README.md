# ZLMediaKit.DotNetCore.Sdk
一个基于[ZLMediaKit](https://github.com/xiongziliang/ZLMediaKit)的.NetCoreSDK

>## STRealVideo.Control  控制台项目(主要用于测试)
>### 参考代码文件Program.cs

>## STRealVideo.Lib  核心SDK类库(用于实际开发对接)
>### \Common 文件夹存放工具类
>### \Models 文件夹存放实体类
>### ZLClient.cs 基于ZLMediaServer实现的API客户端(所有业务操作都使用它来完成)
>### ZLClient 构造方法中传入api地址(如果api服务器运行在本机,则第二个参数secret可以不传)

## 调用方法参考
     
            ZLClient client = new ZLClient("http://127.0.0.1/", "035c73f7-bb6b-4889-a715-d9eb2d1925cc");
            var snapImageResponse = client.getSnap("rtmp://1.1.1.1/live/cctv", 10, 0); //捕捉截图
            var resThreadsLoad = client.getThreadsLoad();//获取各epoll(或select)线程负载以及延时
            var resWorkThreadsLoad = client.getWorkThreadsLoad();//获取各后台epoll(或select)线程负载以及延时
            var resServerConfigs = client.getServerConfig();//获取服务器配置
            var resSetConfig = client.setServerConfig("api.apiDebug", "1");//将服务器参数api.apiDebug设置为0
            var resRestartServer = client.restartServer();//重启服务器,只有Daemon方式才能重启，否则是直接关闭！
            var resMediaList = client.getMediaList();//获取流列表，可选筛选参数
            var resCloseStream = client.closeStream("rtsp", "127.0.0.1", "live", "test");//关闭流(目前所有类型的流都支持关闭)
            var resCloseStreams = client.closeStreams("rtsp", "127.0.0.1");//关闭流(目前所有类型的流都支持关闭)
            var resSessions = client.getAllSession();//获取所有TcpSession列表(获取所有tcp客户端相关信息)
            var resKillSession = client.kickSession(resSessions.data[0].id);//断开tcp连接，比如说可以断开rtsp、rtmp播放器等
            var resKillSessions = client.kickSessions(null, "127.0.0.1");//断开tcp连接，比如说可以断开rtsp、rtmp播放器等
            var resAddStream = client.addStreamProxy("__defaultVhost__", "live", "0", "rtsp://admin:12356789a@192.168.1.3:554/h264/ch1/main/av_stream", true, true, RTPPullType.TCP);//动态添加rtsp/rtmp拉流代理(只支持H264/H265/aac负载)
            var resDelStream = client.delStreamProxy("xxxxxxxxx");//关闭拉流代理
            var resAddFFmpegSource = client.addFFmpegSource("http://127.0.0.1/live/0/hls.m3u8", "rtmp://127.0.0.1/live/2", 10000);//通过fork FFmpeg进程的方式拉流代理，支持任意协议
            var resDelFFmpegSource = client.delFFmpegSource("xxxxxxxxx");//关闭ffmpeg拉流代理
            var resMediaOnline = client.isMediaOnline("rtsp", "__defaultVhost__", "live", "0");//判断直播流是否在线
            var resMediaInfo = client.getMediaInfo("rtsp", "__defaultVhost__", "live", "0");//获取流相关信息
            var resSsrcInfo = client.getSsrcInfo("XXXXXXX");//获取rtp代理时的某路ssrc rtp信息
            var resMp4RecordFile = client.getMp4RecordFile("__defaultVhost__", "live", "0", "2020-04-08");//搜索文件系统，获取流对应的录像文件列表或日期文件夹列表
            var resStartRecord = client.startRecord(0, "__defaultVhost__", "live", "0");//开始录制hls或MP4
            var resStopRecord = client.stopRecord(0, "__defaultVhost__", "live", "0");//停止录制流
            var resRecording = client.isRecording(0, "__defaultVhost__", "live", "0");//停止录制流


# 致谢
#### 感谢作者[夏楚](https://github.com/xiongziliang)开源提供这么棒的流媒体服务框架