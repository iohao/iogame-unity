
public class CmdMgr{
    //获取cmd
    public static int getCmd(int merge) {
        return merge >> 16;
    }
    //获取subCmd
    public static int getSubCmd(int merge) {
        return merge & 0xFFFF;
    }
    //获取mergeCmd
    public static int getMergeCmd(int cmd, int subCmd) {
        return (cmd << 16) + subCmd;
    }
}
