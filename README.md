## NTFSDataStreams
it is simple class for read and write to alternative NTFS data streams.

NTFSDataStream implements the [`Stream`](https://docs.microsoft.com/en-us/dotnet/api/system.io.stream) class.
```C#
Stream stream = new NTFSDataStream("full or relative path for file or directory", "stream data name", 
  FileAccess.ReadWrite, FileMode.OpenOrCreate, FileShare.None);
```
For read and write to stream, you must use the methods `NTFSDataStream.Read()` and `NTFSDataStream.Write()`.