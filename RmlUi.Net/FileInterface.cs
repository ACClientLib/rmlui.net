using System;

namespace RmlUiNet
{
    public abstract class FileInterface : RmlBase<FileInterface>
    {
        private Native.FileInterface.OnOpen _onOpen;
        private Native.FileInterface.OnClose _onClose;
        private Native.FileInterface.OnRead _onRead;
        private Native.FileInterface.OnSeek _onSeek;
        private Native.FileInterface.OnTell _onTell;
        private Native.FileInterface.OnLength _onLength;
        private Native.FileInterface.OnLoadFile _onLoadFile;

        public FileInterface() : base(IntPtr.Zero)
        {
            _onOpen = Open;
            _onClose = Close;
            _onRead = Read;
            _onSeek = Seek;
            _onTell = Tell;
            _onLength = Length;
            _onLoadFile = LoadFile;

            NativePtr = Native.FileInterface.Create(
                _onOpen,
                _onClose,
                _onRead,
                _onSeek,
                _onTell,
                _onLength,
                _onLoadFile
            );

            ManuallyRegisterCache(NativePtr, this);
        }

        public abstract ulong Open(string path);
        public abstract void Close(ulong file);
        public abstract ulong Read(out byte[] buffer, ulong size, ulong file);
        public abstract bool Seek(ulong file, long offset, int origin);
        public abstract int Tell(ulong file);
        public abstract int Length(ulong file);
        public abstract string LoadFile(string path);
    }
}
