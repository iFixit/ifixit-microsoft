using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using iFixit.Domain.Code;
using Windows.Storage.Search;

namespace iFixit.Shared.UI.Services
{
    public class UiStorage : Domain.Interfaces.IStorage
    {
         StorageFolder _baseFolder = ApplicationData.Current.LocalFolder;


        public string BasePath()
        {
            return "ms-appdata:///local/";
        }

        public async Task<bool> FolderExists(string folderName)
        {

            var folders = await _baseFolder.GetFoldersAsync();
            return folders.Any(o => o.Name == folderName.CleanCharacters());
        }

        public async Task RemoveFolder(string folderName)
        {

            var folders = await _baseFolder.GetFoldersAsync();
            if (folders.Any(o => o.Name == folderName.CleanCharacters()))
            {
                var x = folders.SingleOrDefault(o => o.Name == folderName.CleanCharacters());
                await x.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        public async Task<bool> Exists(string fileName)
        {
            var file = await _baseFolder.TryGetItemAsync(fileName.CleanCharacters());
            return file != null;

        }

        public async Task<bool> Exists(string fileName, string folder)
        {
            try
            {
                var roorfolder = await _baseFolder.GetFolderAsync(folder);
                var files = await roorfolder.GetFilesAsync(CommonFileQuery.OrderByName);
                var file = files.FirstOrDefault(x => x.Name == fileName);

                return file != null;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public async Task<bool> Exists(string fileName, TimeSpan expiration)
        {
            try
            {
                var exists = true;

                var file = await _baseFolder.TryGetItemAsync(fileName.CleanCharacters());
                if (file != null)
                {
                    var createdAt = file.DateCreated;
                    var timeDiff = (DateTimeOffset.Now - createdAt);
                    if (timeDiff > expiration)
                        exists = false;

                    return exists;
                }
                else
                    return false;

            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public async Task<string> ReadData(string fileName)
        {
            byte[] data;
            fileName = fileName.CleanCharacters();
            var file = await _baseFolder.GetFileAsync(fileName);
            using (var s = await file.OpenStreamForReadAsync())
            {
                data = new byte[s.Length];
                await s.ReadAsync(data, 0, (int)s.Length);
            }

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public void Save(string storageId, string content)
        {
            WriteData(storageId, content);
        }

        public async Task WriteData(string fileName, string content)
        {
            var data = Encoding.UTF8.GetBytes(content);
            fileName = fileName.CleanCharacters();

            var exists = await Exists(fileName);
            if (exists)
            {
                await Delete(fileName);
            }
            var file = await _baseFolder.CreateFileAsync(fileName);

            using (var s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task WriteBinary(string folderName, string fileName, byte[] content)
        {

            fileName = fileName.CleanCharacters();
            folderName = folderName.CleanCharacters();
            var folders = await _baseFolder.GetFoldersAsync();
            if (folders.All(f => f.Name != folderName))
                await _baseFolder.CreateFolderAsync(folderName);

            var targetFolder = await _baseFolder.GetFolderAsync(folderName);

            var file = await targetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(content, 0, content.Length);
            }
        }

        public async Task Delete(string storageId)
        {
            var file = await _baseFolder.GetFileAsync(storageId.CleanCharacters());
            await file.DeleteAsync();
        }
    }
}
