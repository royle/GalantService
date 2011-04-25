using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pstndemo
{
    class brichiperr
    {
        //USB设备操作 返回的错误ID
        public const int BCERR_VALIDHANDLE = -9;  //不合法的句柄
        public const int BCERR_NOPLAYHANDLE = -10; //没有空闲播放句柄
        public const int BCERR_OPENFILEFAILED = -11; //打开文件失败
        public const int BCERR_READFILEFAILED = -12; //读取文件数据错误
        public const int BCERR_WAVHEADERFAILED = -13; //解析文件头失败
        public const int BCERR_NOTSUPPORTFORMAT = -14; //语音格式不支持
        public const int BCERR_NORECHANDLE = -15; //没有足够的录音句柄
        public const int BCERR_CREATEFILEFAILED = -16; //创建录音文件失败
        public const int BCERR_NOBUFSIZE = -17; //缓冲不够
        public const int BCERR_PARAMERR = -18; //参数错误
        public const int BCERR_INVALIDTYPE = -19; //不合法的参数类型		
        public const int BCERR_INVALIDCHANNEL = -20; //不合法的通道ID
        public const int BCERR_ISMULTIPLAYING = -21; //正在多文件播放,请先停止播放
        public const int BCERR_ISCONFRECING = -22; //正在会议录音,请先停止录音
        public const int BCERR_INVALIDCONFID = -23; //错误的会议ID号
        public const int BCERR_NOTCREATECONF = -24; //会议模块还未创建
        public const int BCERR_NOTCREATEMULTIPLAY = -25; //没有开始多文件播放
        public const int BCERR_NOTCREATESTRINGPLAY = -26; //没有开始字符播放
        public const int BCERR_ISFLASHING = -27; //正在拍插簧,请先停止
        public const int BCERR_FLASHNOTLINE = -28; //设备没有接通线路不能拍插簧
        public const int BCERR_NOTLOADFAXMODULE = -29; //未启动传真模块
        public const int BCERR_FAXMODULERUNING = -30; //传真正在使用，请先停止
        public const int BCERR_VALIDLICENSE = -31; //错误的license
        public const int BCERR_ISFAXING = -32; //正在传真不能软挂机
        public const int BCERR_CCMSGOVER = -33; //CC消息长度太长
        public const int BCERR_CCCMDOVER = -34; //CC命令长度太长
        public const int BCERR_INVALIDSVR = -35; //服务器错误
        public const int BCERR_INVALIDFUNC=-36; //未找到指定函数模块
        public const int BCERR_INVALIDCMD=-37; //未找到指定命令
        public const int BCERR_UNSUPPORTFUNC=-38; //设备不支持该功能unsupport func
        public const int BCERR_DEVNOTOPEN=-39; //未打开指定设备
        public const int BCERR_INVALIDDEVID=-40; //不合法的ID
        public const int BCERR_INVALIDPWD=-41; //密码错误
        public const int BCERR_READSTOREAGEERR=-42; //读取存储错误
        public const int BCERR_INVALIDPWDLEN=-43; //密码长度太长
        public const int BCERR_NOTFORMAT=-44; //flash还未格式化
        public const int BCERR_FORMATFAILED=-45; //格式化失败
        public const int BCERR_NOTENOUGHSPACE = -46; //写入的FLASH数据太长,存储空间不够
		public const int BCERR_WRITESTOREAGEERR	=-47; //д��洢����
		public const int BCERR_NOTSUPPORTCHECK	=-48; //ͨ����֧����·��⹦��

        public const int ERROR_INVALIDDLL=-198;//不合法的DLL文件
        public const int ERROR_NOTINIT=-199;//还没有初始化任何设备
        public const int BCERR_UNKNOW = -200;//未知错误
       

        //-------------------------------------------------------
        //CC 操作 回调的错误ID
        public const int TMMERR_BASE = 0;

        public const int TMMERR_SUCCESS = 0;
        public const int TMMERR_FAILED = -1;//异常错误
        public const int TMMERR_ERROR = 1;//正常错误
        public const int TMMERR_SERVERDEAD = 2;//服务器没反应
        public const int TMMERR_INVALIDUIN = 3;//不合法的
        public const int TMMERR_INVALIDUSER = 4;//不合法的用户
        public const int TMMERR_INVALIDPASS = 5;//不合法的密码
        public const int TMMERR_DUPLOGON = 6;//重复登陆
        public const int TMMERR_INVALIDCONTACT = 7;//添加的好友CC不存在
        public const int TMMERR_USEROFFLINE = 8;//用户不在线
        public const int TMMERR_INVALIDTYPE = 9;//无效
        public const int TMMERR_EXPIRED = 14;//超时
        public const int TMMERR_INVALIDDLL = 15;//无效
        public const int TMMERR_OVERRUN = 16;//无效
        public const int TMMERR_NODEVICE = 17;//打开设备失败
        public const int TMMERR_DEVICEBUSY = 18;//语音呼叫时设备忙
        public const int TMMERR_NOTLOGON = 19;//未登陆
        public const int TMMERR_ADDSELF = 20;//不能增加自己为好友
        public const int TMMERR_ADDDUP = 21;//增加好友重复
        public const int TMMERR_SESSIONBUSY = 23;//无效
        public const int TMMERR_NOINITIALIZE = 25;//还未初始化
        public const int TMMERR_NOANSWER = 26;//无效
        public const int TMMERR_TIMEOUT = 27;//无效
        public const int TMMERR_LICENCE = 28;//无效
        public const int TMMERR_SENDPACKET = 29;//无效
        public const int TMMERR_EDGEOUT = 30;//无效
        public const int TMMERR_NOTSUPPORT = 31;//无效
        public const int TMMERR_NOGROUP = 32;//无效
        public const int TMMERR_LOWERVER_PEER = 34;//无效
        public const int TMMERR_LOWERVER = 35;//无效
        public const int TMMERR_HASPOINTS = 36;//无效
        public const int TMMERR_NOTENOUGHPOINTS = 37;//无效
        public const int TMMERR_NOMEMBER = 38;//无效
        public const int TMMERR_NOAUTH = 39;//无效
        public const int TMMERR_REGTOOFAST = 40;//注册太快
        public const int TMMERR_REGTOOMANY = 41;//注册太多
        public const int TMMERR_POINTSFULL = 42;//无效
        public const int TMMERR_GROUPOVER = 43;//无效
        public const int TMMERR_SUBGROUPOVER = 44;//无效
        public const int TMMERR_HASMEMBER = 45;//无效
        public const int TMMERR_NOCONFERENCE = 46;//无效
        public const int TMMERR_RECALL = 47;//呼叫转移
        public const int TMMERR_SWITCHVOIP = 48;//修改VOIP服务器地址
        public const int TMMERR_RECFAILED = 49;//设备录音错误

        public const int TMMERR_CANCEL = 101;//自己取消
        public const int TMMERR_CLIENTCANCEL = 102;//对方取消
        public const int TMMERR_REFUSE = 103;//拒绝对方
        public const int TMMERR_CLIENTREFUSE = 104;//对方拒绝
        public const int TMMERR_STOP = 105;//自己停止=已接通;
        public const int TMMERR_CLIENTSTOP = 106;//对方停止=已接通;

        public const int TMMERR_VOIPCALLFAILED = 108;//帐号没钱了
        public const int TMMERR_VOIPCONNECTED = 200;//VOIP网络连通了
        public const int TMMERR_VOIPDISCONNECTED = 201;//跟服务器断开连接，SOCKET 服务器关闭了。
        public const int TMMERR_VOIPACCOUNTFAILED = 202;//余额不够
        public const int TMMERR_VOIPPWDFAILED = 203;//帐号密码错误
        public const int TMMERR_VOIPCONNECTFAILED = 204;//连接VOIP服务器失败
        //通过代理服务器中转了
        public const int TMMERR_STARTPROXYTRANS = 205;
    }
}
