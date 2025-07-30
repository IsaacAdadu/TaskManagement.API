using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs.Task
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
