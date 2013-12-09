using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using iFixit.Domain.Code;
using Windows.Storage.Search;

namespace iFixit.UI.Services
{
    public class UiStorage : Domain.Interfaces.IStorage
    {

        public string BasePath()
        {
            return "isostore:/";
        }
       
        private StorageFolder baseFolder = ApplicationData.Current.LocalFolder;

        private IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

        public async Task WriteData(string fileName, string content)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
          
            fileName = fileName.CleanCharacters();
            
            StorageFile file = await baseFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (Stream s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task WriteBinary(string folderName, string fileName, byte[] content)
        {
            

            fileName = fileName.Replace(@"'", "");

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

        public async Task RemoveFolder(string folderName)
        {
            folderName = folderName.CleanCharacters();
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
         
            var folders = await baseFolder.GetFoldersAsync();
            return folders.Any(o => o.Name == folderName.CleanCharacters());
        }

        public async Task<bool> Exists(string fileName)
        {
            try
            {
                fileName = fileName.CleanCharacters();
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

                var file = await baseFolder.GetFileAsync(fileName.CleanCharacters());
                var createdAt = file.DateCreated;
                var TimeDiff = (DateTimeOffset.Now - createdAt);
                if (TimeDiff > expiration)
                    Exists = false;

                return Exists;
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
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(StorageId.CleanCharacters(), FileMode.Create, store))
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
            if (myIsolatedStorage.FileExists(filePath.CleanCharacters()))
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(filePath.CleanCharacters(), FileMode.Open, FileAccess.Read))
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

        public async Task Delete(string StorageId)
        {
            var file = await baseFolder.GetFileAsync(StorageId.CleanCharacters());
            await file.DeleteAsync();
        }


    }
}
