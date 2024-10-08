using System;
using System.Runtime.InteropServices;

namespace RmlUiNet.Native
{
    internal static class Rml
    {
        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_Initialise")]
        public static extern void Initialise();

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_Shutdown")]
        public static extern void Shutdown();

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_SetRenderInterface")]
        public static extern void SetRenderInterface(IntPtr renderInterface);

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_SetSystemInterface")]
        public static extern void SetSystemInterface(IntPtr systemInterface);

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_SetFileInterface")]
        public static extern void SetFileInterface(IntPtr fileInterface);

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_LoadFontFace")]
        public static extern bool LoadFontFace(string fileName, bool fallbackFace, FontWeight weight);

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_CreateEventListener")]
        public static extern IntPtr CreateEventListener(OnProcessEvent onProcessEvent, OnAttachEvent onAttachEvent, OnDetachEvent onDetachEvent);

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_ReleaseEventListener")]
        public static extern void ReleaseEventListener(IntPtr eventListener);

        [DllImport("RmlUiNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rml_RemoveContext")]
        public static extern bool RemoveContext(string name);

        internal delegate void OnProcessEvent(IntPtr eventPtr);
        internal delegate void OnAttachEvent(IntPtr elementPtr, string elementType);
        internal delegate void OnDetachEvent(IntPtr elementPtr, string elementType);
    }
}
