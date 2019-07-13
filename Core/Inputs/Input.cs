using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Input
{
    public Input(InputType type, InputUnion data)
    {
        Type = type;
        Data = data;
    }   

    public InputType Type;
    public InputUnion Data;
    public static int Size => Marshal.SizeOf<Input>();

}