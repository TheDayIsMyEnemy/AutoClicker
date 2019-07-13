using System;

[Flags]
public enum KeyEventFlags : uint
{
    ExtendedKey = 0x0001,
    KeyUp = 0x0002,
    ScanCode = 0x0008,
    Unicode = 0x0004
}
