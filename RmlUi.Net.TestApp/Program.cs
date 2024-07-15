using System;
using RmlUi.Net.TestApp;

internal class Program
{
    private static void Main(string[] args)
    {
        // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
        using (MainWindow window = new MainWindow(1366, 768, "RmlUI.Net Test App"))
        {
            window.Run();
        }
    }
}
