using System;
using System.IO;

namespace NTFSDataStreams.Exceptions
{
    public class NTFSDataStreamException : IOException
    {
        public string FullPath { get; }
        public NTFSDataStreamException(string message, string fullpathstream)
            : base(message)
        { FullPath = fullpathstream; }
    }
    public class PathNotFound : NTFSDataStreamException
    {    
        public PathNotFound(string fullpathstream)
            : base($"Path \"{fullpathstream}\" not found.", fullpathstream)
        { }
    }
    public class PathIncorrect : NTFSDataStreamException
    {
        public PathIncorrect(string fullpathstream)
            : base($"The path \"{fullpathstream}\" syntax is incorrect.", fullpathstream)
        { }
    }
    public class AccessDenied : NTFSDataStreamException
    {
        public AccessDenied(string fullpathstream)
            : base($"Access to the \"{fullpathstream}\" denied.", fullpathstream)
        { }
    }
    public class SharingVoliation : NTFSDataStreamException
    {
        public SharingVoliation(string fullpathstream)
            : base($"Cannot access the \"{fullpathstream}\" because it is being used by another process.", fullpathstream)
        { }
    }
    public class LockVoliation : NTFSDataStreamException
    {
        public LockVoliation(string fullpathstream)
            : base($"Cannot access the \"{fullpathstream}\" because another process has locked a portion of the file.", fullpathstream)
        { }
    }
    public class AlreadyExist : NTFSDataStreamException
    {
        public AlreadyExist(string fullpathstream)
            : base($"Stream \"{fullpathstream}\" already exists.", fullpathstream)
        { }
    }

}