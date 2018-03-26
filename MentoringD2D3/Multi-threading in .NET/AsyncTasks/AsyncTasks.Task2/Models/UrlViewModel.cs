using System;
using System.ComponentModel.DataAnnotations;

namespace AsyncTasks.Task2.Models
{
    public class UrlViewModel
    {
        [Display(Name = "URL")]
        public string Site { get; set; }
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}