using System;
using System.Threading.Tasks;
using AsyncTasks.Task2.Models;

namespace AsyncTasks.Task2.Interfaces
{
    public interface IDownloadService
    {
        void CreateNew(UrlViewModel urlViewModel);

        Task<string> StartNewLoading(Guid id);

        UrlViewModel CancelLoading(UrlViewModel urlViewModel);
    }
}
