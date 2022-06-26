using Business.Models;
using System.Threading.Tasks;

namespace Business.Interfaces
{

    public interface IFileService : ICrud<FileModel>
    {
        public Task CreateDir(FileModel model);
    }
}