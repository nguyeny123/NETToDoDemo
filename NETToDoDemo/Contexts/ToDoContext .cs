using Microsoft.EntityFrameworkCore;
using NETToDoDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETToDoDemo.Contexts
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
    }
}
