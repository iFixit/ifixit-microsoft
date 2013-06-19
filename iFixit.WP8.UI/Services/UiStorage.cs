using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using iFixit.Domain.Code;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Search;

namespace iFixit.UI.Services
{
    public class UiStorage : Domain.Interfaces.IStorage
    {

        private StorageFolder baseFolder = ApplicationData.Current.LocalFolder;

        private IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

        public async Task WriteData(string fileName, string content)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            fileName = fileName.Replace(@"'", "").Replace("\"", "");
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (Stream s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task WriteBinary(string folderName, string fileName, byte[] content)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            fileName = fileName.Replace(@"'", "");

            var folders = await folder.GetFoldersAsync();
            if (!folders.Any(f => f.Name == folderName))
                await folder.CreateFolderAsync(folderName);

            var targetFolder = await folder.GetFolderAsync(folderName);

            StorageFile file = await targetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (Stream s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(content, 0, content.Length);
            }
        }

        public async Task RemoveFolder(string folderName)
        {
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            var folders = await storage.GetFoldersAsync();
            if (folders.Any(o => o.Name == folderName))
            {
                var x = folders.Where(o => o.Name == folderName).SingleOrDefault();
                await x.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        public async Task<bool> FolderExists(string folderName)
        {
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            var folders = await storage.GetFoldersAsync();
            return folders.Any(o => o.Name == folderName);
        }

        public async Task<bool> Exists(string fileName)
        {
            try
            {
                fileName = fileName.Replace(@"'", "").Replace("\"", "");
                var taskCompletionSource = new TaskCompletionSource<bool>();
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                taskCompletionSource.SetResult(fileStorage.FileExists(fileName));
                //StorageFile file = await baseFolder.GetFileAsync(fileName);
                //return true;
                return await taskCompletionSource.Task;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Exists(string fileName, TimeSpan expiration)
        {
            try
            {
                fileName = fileName.Replace(@"'", "").Replace("\"", "");
                var taskCompletionSource = new TaskCompletionSource<bool>();
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                var Exists = fileStorage.FileExists(fileName);
                if (Exists)
                {
                    var createdAt = fileStorage.GetLastWriteTime(fileName);
                    var TimeDiff = (DateTimeOffset.Now - createdAt);
                    if (TimeDiff > expiration)
                        Exists = false;
                }
                taskCompletionSource.SetResult(Exists);
                //StorageFile file = await baseFolder.GetFileAsync(fileName);
                //return true;
                return await taskCompletionSource.Task;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<string> ReadData(string fileName)
        {
            byte[] data;
            fileName = fileName.Replace(@"'", "").Replace("\"", "");
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
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(StorageId, FileMode.Create, store))
                    {
                        using (StreamWriter stream = new StreamWriter(fileStream))
                        {
                            stream.Write(Content);
                            Debug.WriteLine("wrote file");
                        }
                    }
                }
            }
            catch (IsolatedStorageException exception)
            {
                throw exception;
            }
        }

        private string ReadFile(string filePath)
        {
            string Result = string.Empty;
            if (myIsolatedStorage.FileExists(filePath))
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        Result = reader.ReadToEnd();
                        Debug.WriteLine("read file");
                    }
                }
            }
            else
            {

            }

            return Result;
        }

        public void Delete(string StorageId)
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFile.FileExists(StorageId))
                    storageFile.DeleteFile(StorageId);
            }
        }



        private void testes()
        {


        }
    }
}
