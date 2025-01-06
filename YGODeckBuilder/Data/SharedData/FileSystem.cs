using Microsoft.DotNet.Scaffolding.Shared;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace YGODeckBuilder.SharedData
{
    public class FileSystem : IFileSystem
    {
        public Task AddFileAsync(string outputPath, Stream sourceStream)
        {
            throw new System.NotImplementedException();
        }

        public void CreateDirectory(string path)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new System.NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            throw new System.NotImplementedException();
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void MakeFileWritable(string path)
        {
            throw new System.NotImplementedException();
        }

        public string ReadAllText(string path)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveDirectory(string path, bool removeIfNotEmpty)
        {
            throw new System.NotImplementedException();
        }

        public void WriteAllText(string path, string contents)
        {
            throw new System.NotImplementedException();
        }

        // Implement other methods from the IFileSystem interface if needed
    }
}
