using System;
using System.Runtime.InteropServices;

namespace RmlUiNet.Native
{
    internal static class FileInterface
    {
        [DllImport("RmlUi.Native", EntryPoint = "rml_FileInterface_New")]
        public static extern IntPtr Create(
            OnOpen onOpen,
            OnClose onClose,
            OnRead onRead,
            OnSeek onSeek,
            OnTell onTell,
            OnLength onLength,
            OnLoadFile onLoadFile
        );

        internal delegate ulong OnOpen(string path);

        internal delegate void OnClose(ulong file);

        internal delegate ulong OnRead(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            out byte[] buffer,
            ulong size,
            ulong file
        );

        internal delegate bool OnSeek(ulong file, long offset, int origin);

        internal delegate int OnTell(ulong file);

        internal delegate int OnLength(ulong file);

        internal delegate string OnLoadFile(string path);
    }
}
