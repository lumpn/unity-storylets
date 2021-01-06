using System.Diagnostics;

using Debug = UnityEngine.Debug;

public static class Assert
{
    [DebuggerHidden]
    public static void NotNull(object obj)
    {
        Debug.Assert(obj != null);
    }

    [DebuggerHidden]
    public static void Equal(int a, int b)
    {
        Debug.Assert(a == b);
    }

    [DebuggerHidden]
    public static void Greater(int a, int b)
    {
        Debug.Assert(a > b);
    }

    [DebuggerHidden]
    public static void LessOrEqual(int a, int b)
    {
        Debug.Assert(a <= b);
    }
}
