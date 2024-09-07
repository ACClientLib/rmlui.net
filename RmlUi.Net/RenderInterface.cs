using System;
using System.Numerics;

namespace RmlUiNet
{
    public abstract class RenderInterface : RmlBase<RenderInterface>
    {
        private Native.RenderInterface.OnRenderGeometry _onRenderGeometry;
        private Native.RenderInterface.OnGenerateTexture _onGenerateTexture;
        private Native.RenderInterface.OnLoadTexture _onLoadTexture;
        private Native.RenderInterface.OnReleaseTexture _onReleaseTexture;
        private Native.RenderInterface.OnEnableScissorRegion _onEnableScissorRegion;
        private Native.RenderInterface.OnSetScissorRegion _onSetScissorRegion;

        public RenderInterface() : base(IntPtr.Zero)
        {
            _onRenderGeometry = RenderGeometry;
            _onGenerateTexture = GenerateTexture;
            _onLoadTexture = LoadTexture;
            _onReleaseTexture = ReleaseTexture;
            _onEnableScissorRegion = EnableScissorRegion;
            _onSetScissorRegion = SetScissorRegion;

            NativePtr = Native.RenderInterface.Create(
                _onRenderGeometry,
                _onGenerateTexture,
                _onLoadTexture,
                _onReleaseTexture,
                _onEnableScissorRegion,
                _onSetScissorRegion
            );

            ManuallyRegisterCache(NativePtr, this);
        }

        public virtual void RenderGeometry(Vertex[] vertices, int vertexCount, int[] indices, int indexCount,
            ulong texture,
            Vector2f translation)
        {
        }

        public virtual bool GenerateTexture(out ulong textureHandle, byte[] source, int sourceSize, Vector2i sourceDimensions)
        {
            textureHandle = 0;

            return false;
        }

        public virtual bool LoadTexture(out ulong textureHandle, out Vector2i textureDimensions, string source)
        {
            textureHandle = 0;
            textureDimensions = new Vector2i(0, 0);

            return false;
        }

        public virtual void ReleaseTexture(IntPtr textureHandle)
        {
        }

        public virtual void EnableScissorRegion(bool enable)
        {
        }

        public virtual void SetScissorRegion(int x, int y, int width, int height)
        {
        }
    }
}
