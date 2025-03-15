using System;

public enum EventType
{
    TryScored,
    TryAssist,
    Passes,
    Tackles,
    Organization,
}

[Flags]
public enum Quarter
{
    None = 0,
    First = 1 << 0,
    Second = 1 << 1,
    Third = 1 << 2,
    Fourth = 1 << 3,
}

public enum DateDataType
{
    Day,
    Month,
    Year
}