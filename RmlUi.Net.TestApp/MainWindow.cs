using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RmlUiNet;

namespace RmlUi.Net.TestApp
{
    public class MainWindow : GameWindow
    {
        private BaseFileInterface m_FileInterface;
        private BaseSystemInterface m_SystemInterface;
        private BaseRenderInterface m_RenderInterface;

        private Context m_MainContext;

        public MainWindow(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { 
                ClientSize = (width, height),
                Title = title 
            })
        {
            m_FileInterface = new BaseFileInterface();
            m_SystemInterface = new BaseSystemInterface();
            m_RenderInterface = new BaseRenderInterface();

            Rml.SetFileInterface(m_FileInterface);
            Rml.SetSystemInterface(m_SystemInterface);
            Rml.SetRenderInterface(m_RenderInterface);

            Rml.Initialise();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);

            // Create RmlUi Stuff
            Rml.LoadFontFace("fonts/Inter-Regular.ttf");
            m_MainContext = Rml.CreateContext("RmlUi.Net Test App::Main", new Vector2i(1366, 768));

            m_MainContext?.LoadDocument("layouts/demodoc.rml");
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            m_MainContext?.Dispose();
            Rml.Shutdown();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            m_MainContext?.Update();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            m_MainContext?.Render();

            SwapBuffers();
        }
    }
}
