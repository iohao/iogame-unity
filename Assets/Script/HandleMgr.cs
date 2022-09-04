
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Google.Protobuf;
using UnityEngine;

public abstract class HandleMgr{
    public static readonly Dictionary< int ,  EventHandler<Execute> > handlers = new();
        
    // 事件参数
    public  class Execute : EventArgs{
        public ByteString data;
        internal Execute(ByteString data){
            this.data = data;
        }
        
    }
    
    // 注册回调
    public static void addHandler(int cmd,int subCmd,  EventHandler<Execute> IHandler){
        handlers.Add(CmdMgr.getMergeCmd(cmd,subCmd),IHandler); 
    }

    // 这里为了方便测试就写在一起了
    public static void Hello(object sender, Execute e){
        var loginVerify = new LoginVerify();
        loginVerify.MergeFrom(e.data);
        Debug.Log(loginVerify);
    }
    
    //消息分发
    public static void packageHandler(int cmd, int subCmd, ByteString data ) {
        EventHandler<Execute> handler = handlers[CmdMgr.getMergeCmd(cmd,subCmd)];
        if (handler != null) handler.Invoke(handler, new Execute(data));
    }

    //消息分发
    public static void packageHandler( int mergeCmd, ByteString data ) {
        EventHandler<Execute> handler = handlers[mergeCmd];
        if (handler != null) handler.Invoke(handler, new Execute(data));
    }
}
