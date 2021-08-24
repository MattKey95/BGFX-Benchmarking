using Bgfx;
using GLFW;

namespace CSharp
{
    public class Startup
    {
        public unsafe void CreateWindow()
        {
            var bgfx = new Bgfx.Bgfx();
            Glfw.Init();
            var window = new NativeWindow(800, 400, "Pluto");
            var windowHandle = Native.GetWin32Window(window);
            var pd = new PlatformData
            {
                nwh = windowHandle.ToPointer()
            };
            bgfx.set_platform_data(&pd);

            var ini = new Init();
            bgfx.init_ctor(&ini);
            ini.type = RendererType.OpenGL;

            ini.resolution = new Resolution
            {
                width = (uint)400,
                height = (uint)800,
                reset = (uint)ResetFlags.Vsync,
                numBackBuffers = 2
            };
            bgfx.init(&ini);
        }
    }
}
