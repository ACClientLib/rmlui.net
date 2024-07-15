using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RmlUiNet;

namespace RmlUi.Net.TestApp
{
    public class BaseSystemInterface : SystemInterface
    {
        private Stopwatch _timer;

        public BaseSystemInterface()
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        public override double ElapsedTime => _timer.ElapsedMilliseconds;

        public override bool LogMessage(LogType type, string message)
        {
            Console.WriteLine($"[{type}] {message}");
            return true;
        }
    }
}
