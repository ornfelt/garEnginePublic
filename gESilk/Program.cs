﻿using gESilk.engine.window;

namespace gESilk;

public static class Program
{
    private static Window? _window;
    
    static void Main()
    {
        _window = new(1280, 720, "garEngine2");
        _window.Run();
    }
}