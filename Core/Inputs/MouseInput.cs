using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct MouseInput
{
    public int dx;
    public int dy;
    public int mouseData;
    public MouseEventFlags dwFlags;
    public uint time;
    public UIntPtr dwExtraInfo;
}