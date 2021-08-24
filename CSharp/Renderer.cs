using Bgfx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class Renderer
    {
        private Bgfx.Bgfx _bgfx;
        private int _windowWidth;
        private int _windowHeight;
        private VertexBufferHandle _vb;
        private IndexBufferHandle _ib;
        private ProgramHandle _program;

        public unsafe Renderer()
        {
            _bgfx = new Bgfx.Bgfx();
            _windowWidth = 800;
            _windowHeight = 400;
        }

        public unsafe void Init()
        {
            var tri = new Triangle();

            var vbMem = Create(tri.vertices);
            var ibMem = Create(tri.indices);
            tri.Init(vbMem, ibMem);

            _vb = tri.VertexBuffer;
            _ib = tri.IndexBuffer;

            _program = LoadProgram("v_default", "f_default");
            _bgfx.reset((uint)_windowWidth, (uint)_windowHeight, 0, TextureFormat.RGBA8);
        }

        public unsafe void Render()
        {
            _bgfx.set_view_clear(0, (ushort)(ClearFlags.Color | ClearFlags.Depth), 0x443355FF, 1, 0);
            _bgfx.set_view_rect(0, 0, 0, (ushort)_windowWidth, (ushort)_windowHeight);


            // view transforms
            var viewMatrix = Matrix4x4.CreateLookAt(new Vector3(0.0f, 0.0f, -35.0f), Vector3.Zero, Vector3.UnitY);
            var projMatrix = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 3, (float)_windowWidth / _windowHeight, 0.1f, 100.0f);
            _bgfx.set_view_transform(0, &viewMatrix.M11, &projMatrix.M11);

            _bgfx.touch(0);

            // set pipeline states
            _bgfx.set_vertex_buffer(0, _vb, 0, 3);
            _bgfx.set_index_buffer(_ib, 0, 3);
            _bgfx.set_state((ulong)StateFlags.Default, 0);

            // submit primitives
            _bgfx.submit(0, _program, 0, 0);
            _bgfx.frame(false);
        }

        private ProgramHandle LoadProgram(string vert, string frag)
        {
            var vsh = LoadShader(vert);
            var fsh = LoadShader(frag);

            return _bgfx.create_program(vsh, fsh, true);
        }

        private unsafe ShaderHandle LoadShader(string name)
        {
            var path = $"Shaders\\{name}.bin";
            var bytes = File.ReadAllBytes(path);
            var mem = Create(bytes);
            return _bgfx.create_shader(mem);
        }

        public unsafe Memory* Create(IntPtr data, uint size)
        {
            return _bgfx.copy(data.ToPointer(), size);
        }

        public unsafe Memory* Create<T>(T[] data) where T : struct
        {
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");

            var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var ptr = Create(gcHandle.AddrOfPinnedObject(), (uint)(Marshal.SizeOf<T>() * data.Length));

            gcHandle.Free();
            return ptr;
        }
    }

    public class Triangle
    {
        public VertexBufferHandle VertexBuffer { get; set; }
        public IndexBufferHandle IndexBuffer { get; set; }

        public unsafe void Init(Memory* vbMem, Memory* ibMem)
        {
            var _bgfx = new Bgfx.Bgfx();

            var layout = new VertexLayout();
            _bgfx.vertex_layout_begin(&layout, RendererType.OpenGL);
            _bgfx.vertex_layout_add(&layout, Attrib.Position, 3, AttribType.Float, false, false);
            _bgfx.vertex_layout_end(&layout);

            VertexBuffer = _bgfx.create_vertex_buffer(vbMem, &layout, 0);
            IndexBuffer = _bgfx.create_index_buffer(ibMem, 0);
        }

        public readonly Vector3[] vertices = {
            new Vector3{ X = -0.5f, Y = -0.5f, Z = 0.0f },
            new Vector3{ X = 0.5f, Y = -0.5f, Z = 0.0f },
            new Vector3{ X = 0.0f, Y = 0.5f, Z = 0.0f }
        };

        public readonly short[] indices = {
            0, 1, 2
        };
    }
}
