using System;

namespace Business.Exceptions
{
    /// <summary>
    /// Exception that is used in all business logic with default creating
    /// </summary>
    /// <seealso cref="System.Exception" />
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