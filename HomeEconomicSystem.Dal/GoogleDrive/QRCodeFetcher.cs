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
        private const string CREDENTIALS_FILE = "credentials.json";
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
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME,
            });

            return service;
        }


        public IEnumerable<IQRcode> DeleteQRcode(IEnumerable<IQRcode> qrCodeToDelete)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IQRcode> GetQRCode()
        {
            // TODO: Fetch only images that not in trash
            // drive.files().list().setQ("trashed = false and mimeType='image'").execute();
            // For conditions look here: https://developers.google.com/drive/api/v3/search-files
            throw new NotImplementedException();
        }
    }
}
