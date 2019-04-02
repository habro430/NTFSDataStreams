using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using NTFSDataStreams.Exceptions;

namespace NTFSDataStreams.Native
{
    enum ErrorCodes : int
    {
        ERROR_SUCCESS = 0,
        ERROR_FILE_NOT_FOUND = 2,
        ERROR_PATH_NOT_FOUND = 3,
        ERROR_ACCESS_DENIED = 5,
        ERROR_INVALID_DRIVE = 15,
        ERROR_NOT_READY = 21,
        ERROR_SHARING_VIOLATION = 32,
        ERROR_LOCK_VIOLATION = 33,
        ERROR_FILE_EXISTS = 80,
        ERROR_INVALID_PARAMETER = 87,
        ERROR_BUFFER_OVERFLOW = 111,
        ERROR_INVALID_NAME = 123,
        ERROR_FILENAME_EXCED_RANGE = 206,
    }

    static class NativeMethods
    {
        public static void Throw(int error, string path)
        {
            switch (error)
            {
                case (int)ErrorCodes.ERROR_SUCCESS: return;
                case (int)ErrorCodes.ERROR_FILE_NOT_FOUND:
                case (int)ErrorCodes.ERROR_PATH_NOT_FOUND:
                case (int)ErrorCodes.ERROR_INVALID_DRIVE:
                case (int)ErrorCodes.ERROR_NOT_READY:
                    throw new PathNotFoundException(path);
                case (int)ErrorCodes.ERROR_ACCESS_DENIED:
                    throw new AccessDeniedException(path);
                case (int)ErrorCodes.ERROR_SHARING_VIOLATION:
                    throw new SharingVoliationException(path);
                case (int)ErrorCodes.ERROR_LOCK_VIOLATION:
                    throw new LockVoliationException(path);
                case (int)ErrorCodes.ERROR_FILE_EXISTS:
                    throw new AlreadyExistException(path);
                case (int)ErrorCodes.ERROR_BUFFER_OVERFLOW:
                case (int)ErrorCodes.ERROR_INVALID_NAME:
                case (int)ErrorCodes.ERROR_FILENAME_EXCED_RANGE:
                    throw new PathIncorrectException(path);
                default:
                    {
                        Marshal.ThrowExceptionForHR(-2147024896 | error);
                        break;
                    }
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeleteFile(string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetFileAttributes(string fileName);

    }
}
