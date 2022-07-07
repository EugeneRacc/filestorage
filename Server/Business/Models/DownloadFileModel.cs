using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    /// <summary>
    /// Model with all info for creating FileContentResult
    /// </summary>
    public class DownloadFileModel
    {
        public string Extension { get; set; }
        public byte[] Memory { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }

        public DownloadFileModel (string ext, byte[] memory, string fileName)
        {
            Extension = GetMimeType()[ext];
            Memory = memory;
            FileName = fileName;
            Type = ext;
        }
        /// <summary>
        /// Gets the type of the MIME for downloading file.
        /// </summary>
        /// <returns>Dictionary with all popular MIMEs</returns>
        private Dictionary<string, string> GetMimeType()
        {
            return new Dictionary<string, string>
            {
                { "txt", "text/plain" },
                { "pdf", "application/pdf" },
                { "doc", "application/vnd.ms-word" },
                { "docx", "application/vnd.ms-word" },
                { "xls", "application/vnd.ms-excel" },
                { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { "png", "image/png" },
                { "jpg", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "gif", "image/gif" },
                { "csv", "image/csv" },
                { "xml", "application/xml" },
            };
        }
    }
}
