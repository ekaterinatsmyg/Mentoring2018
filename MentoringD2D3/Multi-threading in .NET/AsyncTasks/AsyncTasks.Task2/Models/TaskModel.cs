using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AsyncTasks.Task2.Models
{
    public class TaskModel
    {
        [Display(Name = "URL")]
        public string Site { get; set; }
        public Guid Id { get; set; }
        public WebClient Client { get; set; }
        public TaskModel(UrlViewModel viewModel)
        {
            Site = viewModel.Site;
            Id = viewModel.Id;
        }
    }
}