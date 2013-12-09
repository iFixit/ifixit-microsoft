using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Interfaces
{
    public interface IStorage
    {
        Task<bool> FolderExists(string folderName);
        Task RemoveFolder(string folderName);
        Task<bool> Exists(string fileName);
        Task<bool> Exists(string fileName, string folder);
        Task<bool> Exists(string fileName, TimeSpan expitation);
        Task<string> ReadData(string fileName);

        void Save(string StorageId, string Content);
        Task WriteData(string fileName, string content);
        Task WriteBinary(string folder, string fileName, byte[] content);
      

        Task Delete(string StorageId);
        string BasePath();
    }
}
