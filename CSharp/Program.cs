using System;

namespace CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            startup.CreateWindow();

            var renderer = new Renderer();
            renderer.Init();

            while (true)
            {
                renderer.Render();
                GLFW.Glfw.PollEvents();
            }
        }
    }
}
