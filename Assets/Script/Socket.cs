using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityWebSocket;

public class Socket : MonoBehaviour{
    // Start is called before the first frame update
    private string wsUrl = "ws://localhost:10100/websocket"; //io game
    public static Socket instance = null;
    public WebSocket socket;
    private void Awake(){
        if (instance != null){
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        if (socket == null){
            init();
        }
    }

    /// <summary>
    /// 关闭链接
    /// </summary>
    public static void Close(){
        instance.socket.CloseAsync();
    }
    public void init(){
        HandleMgr.addHandler(1, 0, HandleMgr.Hello);

        socket = new WebSocket(wsUrl);
        // 注册回调
        socket.OnOpen += OnOpen;
        socket.OnClose += OnClose;
        socket.OnMessage += OnMessage;
        socket.OnError += OnError;
        socket.ConnectAsync();
    }

    public void OnOpen(object o, OpenEventArgs args){
        var loginVerify = new LoginVerify{
            Age = 273676,
            Jwt = "luoyi",
            LoginBizCode = 1
        };
        var myExternalMessage = new MyExternalMessage{
            CmdMerge = CmdMgr.getMergeCmd(3, 1),
            DataContent = loginVerify.ToByteString(),
            ProtocolSwitch = 0,
            CmdCode = 1
        };
        socket.SendAsync(myExternalMessage.ToByteArray());
        Debug.Log("打开链接回调");
    }


    public void OnClose(object o, CloseEventArgs args){
        Debug.Log("关闭链接");
    }

    public void OnMessage(object o, MessageEventArgs args){
        //将字节数组转换为
        IMessage message = new MyExternalMessage();
        var mySelf = (MyExternalMessage)message.Descriptor.Parser.ParseFrom(args.RawData);
        HandleMgr.packageHandler(mySelf.CmdMerge, mySelf.DataContent);
    }

    public void OnError(object o, ErrorEventArgs args){
        Debug.Log("异常出现: " + args.Message);
    }
}