using System;
using System.IO;

namespace NTFSDataStreams.Exceptions
{
    public class NTFSDataStreamException : IOException
    {
        public string FullPath { get; }
        public NTFSDataStreamException(string message, string pathstream)
            : base(message)
        { FullPath = pathstream; }
    }
    public class PathNotFoundException : NTFSDataStreamException
    {    
        public PathNotFoundException(string pathstream)
            : base($"Path \"{pathstream}\" not found.", pathstream)
        { }
    }
    public class PathIncorrectException : NTFSDataStreamException
    {
        public PathIncorrectException(string pathstream)
            : base($"The path \"{pathstream}\" syntax is incorrect.", pathstream)
        { }
    }
    public class AccessDeniedException : NTFSDataStreamException
    {
        public AccessDeniedException(string pathstream)
            : base($"Access to the \"{pathstream}\" denied.", pathstream)
        { }
    }
    public class SharingVoliationException : NTFSDataStreamException
    {
        public SharingVoliationException(string pathstream)
            : base($"Cannot access the \"{pathstream}\" because it is being used by another process.", pathstream)
        { }
    }
    public class LockVoliationException : NTFSDataStreamException
    {
        public LockVoliationException(string pathstream)
            : base($"Cannot access the \"{pathstream}\" because another process has locked a portion of the file.", pathstream)
        { }
    }
    public class AlreadyExistException : NTFSDataStreamException
    {
        public AlreadyExistException(string pathstream)
            : base($"Stream \"{pathstream}\" already exists.", pathstream)
        { }
    }
    public class ReadNotSupportedException : NTFSDataStreamException
    {
        public ReadNotSupportedException(string pathstream)
            : base($"Reading for stream \"{pathstream}\" not supported.", pathstream)
        { }
    }
    public class WriteNotSupportedException : NTFSDataStreamException
    {
        public WriteNotSupportedException(string pathstream)
            : base($"Writing for stream \"{pathstream}\" not supported.", pathstream)
        { }
    }
}