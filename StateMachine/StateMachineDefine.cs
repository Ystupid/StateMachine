using System;
using System.Collections.Generic;
using System.Text;

public static class StateMachineDefine
{
    public const int TRANSITION_TO_SELF = 1 << 0x1F;
    public const int MAX_CAPACITY = 0X20;
    public const int DEFAULT_VALUE = 0x7FFFFFFF;
}
