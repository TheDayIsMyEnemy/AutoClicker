using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct HardwareInput
{
    public uint Msg;
    public ushort ParamL;
    public ushort ParamH;
}