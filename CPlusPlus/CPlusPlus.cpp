#include <iostream>
#include <bgfx/bgfx.h>
#include <GLFW/glfw3.h>
#include <GLFW/glfw3native.h>

void CreateWindow();

int main()
{
    CreateWindow();
    int a;
    std::cin >> a;
}

void CreateWindow() {
    glfwInit();
    auto window = glfwCreateWindow(800, 400, "Benchmark Test", NULL, NULL);
    auto pd = new bgfx::PlatformData();
    pd->nwh = window;

    bgfx::Init init;
    init.type = bgfx::RendererType::OpenGL;
    init.vendorId = 0;
    init.resolution.width = 800;
    init.resolution.height = 400;
    init.resolution.reset = BGFX_RESET_VSYNC;
    bgfx::init(init);
}
