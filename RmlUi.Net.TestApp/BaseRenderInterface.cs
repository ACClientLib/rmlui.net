using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using RmlUiNet;

namespace RmlUi.Net.TestApp
{
    public class BaseRenderInterface : RenderInterface
    {
        public override void RenderGeometry(Vertex[] vertices, int vertexCount, int[] indices, int indexCount, ulong texture, Vector2 translation)
        {
            // do nothing
        }
    }
}
