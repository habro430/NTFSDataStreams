using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using NTFSDataStreams.Native;
using NTFSDataStreams.Exceptions;

namespace NTFSDataStreams
{
    public class NTFSDataStream : Stream
    {
        private string _path;
        public string Path => _path; 

        private string _streamname;
        public string StreamName => _streamname;

        private Stream _stream;

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override bool CanTimeout => _stream.CanTimeout;
        public override int ReadTimeout
        {
            get => _stream.ReadTimeout;
            set => _stream.ReadTimeout = value;
        }
        public override int WriteTimeout
        {
            get => _stream.WriteTimeout;
            set => _stream.WriteTimeout = value;
        }

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public NTFSDataStream(string path, string streamname, FileAccess access, FileMode mode, FileShare share)
        {
            _path = path;
            _streamname = streamname;

            if (mode == FileMode.Append) mode = FileMode.OpenOrCreate;

            string streampath = BuildStreamPath(_path, _streamname);

            SafeFileHandle handle = NativeMethods.CreateFile(streampath, access, share, IntPtr.Zero, mode, 0, IntPtr.Zero);
            if (handle.IsInvalid) NativeMethods.Throw(Marshal.GetLastWin32Error(), streampath);

            _stream = new FileStream(handle, access);
        }

        private static string BuildStreamPath(string Path, string StreamName)
        {
            if (string.IsNullOrEmpty(StreamName))
                return Path;
            else
                return Path + ':' + StreamName;
        }

        public void Delete() => Delete(_path, _streamname);
        public static void Delete(string path, string streamname)
        {
            string streampath = BuildStreamPath(path, streamname);

            if (!NativeMethods.DeleteFile(streampath))
                NativeMethods.Throw(Marshal.GetLastWin32Error(), streampath);
        }

        public bool Exists() => Exists(_path, _streamname);
        public static bool Exists(string path, string streamname) => 
            NativeMethods.GetFileAttributes(BuildStreamPath(path, streamname)) != -1;

        public override void Flush() => _stream.Flush();

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => _stream.SetLength(value);


        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead) throw new ReadNotSupportedException(BuildStreamPath(_path, _streamname));

            return _stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite) throw new WriteNotSupportedException(BuildStreamPath(_path, _streamname));

            _stream.Write(buffer, offset, count);
        }
    }
}
