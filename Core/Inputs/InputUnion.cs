using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct InputUnion
{
    [FieldOffset(0)]
    public HardwareInput Hardware;
    [FieldOffset(0)]
    public KeybdInput Keyboard;
    [FieldOffset(0)]
    public MouseInput Mouse;
}