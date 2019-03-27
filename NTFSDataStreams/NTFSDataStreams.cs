using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using NTFSDataStreams.Exceptions;
using NTFSDataStreams.Native;

namespace NTFSDataStreams
{
    public class NTFSDataStream
    {
        private string fullpathstream;
        public string FullPathStream { get { return fullpathstream; } }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                UpdateFullPath();
            }
        }

        private string streamname;
        public string StreamName
        {
            get { return streamname; }
            set
            {
                streamname = value;
                if (!string.IsNullOrEmpty(value)) UpdateFullPath();
            }
        }

        public NTFSDataStream(string Path, string StreamName)
        {
            this.Path = Path;
            this.StreamName = StreamName;
        }

        public FileStream OpenStream(FileAccess access, FileMode mode, FileShare share)
        {
            if (mode == FileMode.Append) mode = FileMode.OpenOrCreate;

            SafeFileHandle handle = NativeMethods.CreateFile(fullpathstream, access, share, IntPtr.Zero, mode, 0, IntPtr.Zero);
            if (handle.IsInvalid) NativeMethods.Throw(Marshal.GetLastWin32Error(), fullpathstream);

            return new FileStream(handle, access);
        }
        public void Delete()
        {
            if (!NativeMethods.DeleteFile(fullpathstream))
                NativeMethods.Throw(Marshal.GetLastWin32Error(), fullpathstream);
        }

        public bool Exists() => -1 != NativeMethods.GetFileAttributes(fullpathstream);
        public static bool Exists(string Path, string StreamName) => -1 != NativeMethods.GetFileAttributes(BuildStreamPath(Path, StreamName));
        
        private void UpdateFullPath() => fullpathstream = BuildStreamPath(path, streamname);

        private static string BuildStreamPath(string Path, string StreamName)
        {
            if (string.IsNullOrEmpty(StreamName))
                return Path;
            else
                return Path + ':' + StreamName;
        }
    }
}
