using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AsyncTasks.Logger;
using AsyncTasks.Task2.Interfaces;
using AsyncTasks.Task2.Models;

namespace AsyncTasks.Task2.Services
{
    public class DownloadService : IDownloadService
    {
        /// <summary>
        /// The queue of loadings.
        /// </summary>
        private static ConcurrentDictionary<Guid, TaskModel> downloaderManager = new ConcurrentDictionary<Guid, TaskModel>();

        /// <summary>
        /// Create and put into a queue a new loading task.
        /// </summary>
        /// <param name="urlViewModel">The site that should be loaded.</param>
        public void CreateNew(UrlViewModel urlViewModel)
        {
            urlViewModel.Id = Guid.NewGuid();
            var taskmodel = new TaskModel(urlViewModel)
            {
                Client = new WebClient()
            };
            taskmodel.Client.Encoding = Encoding.UTF8;
            downloaderManager.TryAdd(urlViewModel.Id, taskmodel);
        }

        /// <summary>
        /// Start a new loading of the site's content.
        /// </summary>
        /// <param name="id">The id of the loading task.</param>
        /// <returns>The result of the loading task.</returns>
        public async Task<string> StartNewLoading(Guid id)
        {
            if (downloaderManager.TryRemove(id, out var taskmodel))
            {
                var client = taskmodel.Client;
                var uri = new Uri(taskmodel.Site);
                try
                {
                    return await client.DownloadStringTaskAsync(uri);
                }
                catch (Exception ex)
                {
                    ApplicationLogger.LogMessage(LogMessageType.Error, $"Loading was aborted : {ex.Message} {Environment.NewLine} {ex.StackTrace}");
                }
            }
            return null;
        }

        /// <summary>
        /// Cancels loading of the site's content.
        /// </summary>
        /// <param name="urlViewModel">The loading site model.</param>
        /// <returns>The loading site model.</returns>
        public UrlViewModel CancelLoading(UrlViewModel urlViewModel)
        {
            TaskModel taskmodel;
            downloaderManager.TryGetValue(urlViewModel.Id, out taskmodel);
            taskmodel?.Client?.CancelAsync();
            urlViewModel.Content = "Loading was canceled.";

            return urlViewModel;
        }
    }
}