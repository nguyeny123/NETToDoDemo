using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETToDoDemo.Entities
{
    public class ToDo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Range(1, 3)]
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
