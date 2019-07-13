using System;
using System.Globalization;
using System.Runtime.InteropServices;


public class KeyboardPointer : IDisposable
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

    [DllImport("user32.dll")]
    static extern bool UnloadKeyboardLayout(IntPtr hkl);
    [DllImport("user32.dll")]
    static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

    private readonly IntPtr pointer;
    public KeyboardPointer(int klid)
    {
        pointer = LoadKeyboardLayout(klid.ToString("X8"), 1);
    }
    public KeyboardPointer(CultureInfo culture)
      : this(culture.KeyboardLayoutId) { }
    public void Dispose()
    {
        UnloadKeyboardLayout(pointer);
        GC.SuppressFinalize(this);
    }
    ~KeyboardPointer()
    {
        UnloadKeyboardLayout(pointer);
    }

    public VirtualKeyCode GetVirtualKey(char character)
    {
        return (VirtualKeyCode)GetVirtualKeyValue(character);
    }

    public short GetVirtualKeyValue(char character)
    {
        return VkKeyScanEx(character, pointer);
    }
}