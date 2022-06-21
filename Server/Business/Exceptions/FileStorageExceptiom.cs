using System;

namespace Business.Exceptions
{

    [Serializable]
    public class FileStorageException : Exception
    {
        public FileStorageException() { }

        public FileStorageException(string name)
            : base(name)
        {

        }

    }
}