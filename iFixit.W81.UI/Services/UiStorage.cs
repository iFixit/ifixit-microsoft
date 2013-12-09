using System;
using System.Collections.Generic;
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
        private StorageFolder baseFolder = ApplicationData.Current.LocalFolder;


        public string BasePath()
        {
            return "ms-appdata:///local/";
        }

        public async Task<bool> FolderExists(string folderName)
        {

            var folders = await baseFolder.GetFoldersAsync();
            return folders.Any(o => o.Name == folderName.CleanCharacters());
        }

        public async Task RemoveFolder(string folderName)
        {

            var folders = await baseFolder.GetFoldersAsync();
            if (folders.Any(o => o.Name == folderName.CleanCharacters()))
            {
                var x = folders.Where(o => o.Name == folderName.CleanCharacters()).SingleOrDefault();
                await x.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        public async Task<bool> Exists(string fileName)
        {
            var file = await baseFolder.TryGetItemAsync(fileName.CleanCharacters());
            return file != null;

        }

        public async Task<bool> Exists(string fileName, string folder)
        {
            try
            {
                var roorfolder = await baseFolder.GetFolderAsync(folder);
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
                bool Exists = true;

                var file = await baseFolder.TryGetItemAsync(fileName.CleanCharacters());
                if (file != null)
                {
                    var createdAt = file.DateCreated;
                    var TimeDiff = (DateTimeOffset.Now - createdAt);
                    if (TimeDiff > expiration)
                        Exists = false;

                    return Exists;
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
            StorageFile file = await baseFolder.GetFileAsync(fileName);
            using (Stream s = await file.OpenStreamForReadAsync())
            {
                data = new byte[s.Length];
                await s.ReadAsync(data, 0, (int)s.Length);
            }

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public void Save(string StorageId, string Content)
        {
            WriteData(StorageId, Content);
        }

        public async Task WriteData(string fileName, string content)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            fileName = fileName.CleanCharacters();

            bool exists = await Exists(fileName);
            if (exists)
            {
                await Delete(fileName);
            }
            StorageFile file = await baseFolder.CreateFileAsync(fileName);

            using (Stream s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task WriteBinary(string folderName, string fileName, byte[] content)
        {

            fileName = fileName.CleanCharacters();
            folderName = folderName.CleanCharacters();
            var folders = await baseFolder.GetFoldersAsync();
            if (!folders.Any(f => f.Name == folderName))
                await baseFolder.CreateFolderAsync(folderName);

            var targetFolder = await baseFolder.GetFolderAsync(folderName);

            StorageFile file = await targetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (Stream s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(content, 0, content.Length);
            }
        }

        public async Task Delete(string StorageId)
        {
            var file = await baseFolder.GetFileAsync(StorageId.CleanCharacters());
            await file.DeleteAsync();
        }
    }
}
