using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal.GoogleDrive
{
    public class QRCodeFetcher : IQRCodeFetcher
    {
        private const string APPLICATION_NAME = "Home Economic System";
        private const string CREDENTIALS_FILE = @"GoogleDrive\credentials.json";
        private const string TOKEN_FILE = "token.json";
        private const string USER_TO_AUTHORIZE = "user";
        private string[] _scopes;

        public QRCodeFetcher()
        {
            _scopes = new[] { DriveService.Scope.Drive };
        }

        /// <summary>
        /// Create credentials to user google drive
        /// </summary>
        /// <returns></returns>
        private UserCredential CreateCredentials()
        {
            UserCredential credential;

            using (var stream =
                new FileStream(CREDENTIALS_FILE, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = TOKEN_FILE;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    USER_TO_AUTHORIZE,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return credential;
        }

        /// <summary>
        /// Create service object to deal with google-drive.
        /// </summary>
        /// <returns></returns>
        private DriveService CreateDriveService()
        {
            UserCredential credential = CreateCredentials();

            // Create Drive API service.
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME,
            });

            return service;
        }

        /// <summary>
        /// Fetch all files details from GoogleDrive.
        /// </summary>
        /// <param name="service">Service to use for the fetching</param>
        /// <param name="fetchCondition">
        /// Determine which files to fetch.
        /// For conditions look here: https://developers.google.com/drive/api/v3/search-files
        /// </param>
        /// <returns>All file details</returns>
        private IEnumerable<FileDetails> GetQRCodeDetails(DriveService service, string fetchCondition)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Fields = "files(id, name, mimeType)";
            //filter 
            listRequest.Q = fetchCondition;
            // List files
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            IEnumerable<FileDetails> filesDetails =
                from file in files
                select new FileDetails { Id = file.Id, Name = file.Name, MimeType = file.MimeType };

            return filesDetails;
        }

        /// <summary>
        /// Download specific file from the GoogleDrive.
        /// </summary>
        /// <param name="service">Service to use</param>
        /// <param name="file">File to download</param>
        /// <returns>Stream of the downloaded file</returns>
        private byte[] DownloadFile(DriveService service, FileDetails file)
        {
            var stream = new System.IO.MemoryStream();
            var downloader = service.Files.Get(file.Id).DownloadWithStatus(stream);
            if (downloader.Status != Google.Apis.Download.DownloadStatus.Failed)
            {
                return stream.ToArray();
            }
            else
            {
                throw new DownloadFileException();
            }
        }

        public IEnumerable<IQRcode> GetQRCode()
        {
            DriveService service = CreateDriveService();
            IEnumerable<FileDetails> filesDetails = GetQRCodeDetails(service, "trashed = false and mimeType contains 'image/'");
            IEnumerable<QRcode> qRcodes =
                from fileDetails in filesDetails
                let fileContent = DownloadFile(service, fileDetails)
                select new QRcode { FileName = fileDetails.Name, ImageStream = fileContent };
            return qRcodes;
        }

        /// <summary>
        /// Get list of file names and return their files details.
        /// </summary>
        /// <param name="filesNames"></param>
        /// <returns></returns>
        private IEnumerable<FileDetails> FetchFilesByNames(DriveService service, IEnumerable<string> filesNames)
        {
            IEnumerable<string> conditions = filesNames.Select(fileName => $"name = '{fileName}'");
            string condition = string.Join(" or ", conditions);
            return GetQRCodeDetails(service, condition);
        }

        public void DeleteQRcode(params string[] qrCodeFilesNamesToDelete)
        {
            var service = CreateDriveService();
            var files = FetchFilesByNames(service, qrCodeFilesNamesToDelete);

            var filesNamesFound = files.Select(file => file.Name);
            List<string> filesNotFound = qrCodeFilesNamesToDelete.Except(filesNamesFound).ToList();
            List<string> filesFailedToDelete = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    service.Files.Delete(file.Id).Execute();
                }
                catch (Google.GoogleApiException e)
                {
                    if (e.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        filesNotFound.Add(file.Name);
                    }
                    else
                    {
                        filesFailedToDelete.Add(file.Name);
                    }
                }
                catch
                {
                    filesFailedToDelete.Add(file.Name);
                }
            }
        }

        private class FileDetails
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string MimeType { get; set; }
        }

    }

}
