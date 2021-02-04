using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.Dal
{
    /// <summary>
    /// Occur when files deletion was failed.
    /// </summary>
    public class DeleteFilesException : Exception
    {
        public IEnumerable<string> FilesNotExists { get; private set; }
        public IEnumerable<string> FailedToDelete { get; private set; }

        public DeleteFilesException(IEnumerable<string> filesNotExists, IEnumerable<string> failedToDelete)
        {
            FilesNotExists = filesNotExists;
            FailedToDelete = failedToDelete;
        }
    }
}
