using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct KeybdInput
{
    public VirtualKeyCode wVk;
    public ScanCode wScan;
    public KeyEventFlags dwFlags;
    public int time;
    public UIntPtr dwExtraInfo;
}