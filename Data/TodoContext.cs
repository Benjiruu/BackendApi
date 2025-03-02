using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Backend_Api.Models;

namespace Backend_Api.Data
{
        public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
        {
        public required DbSet<Note> Notes { get; set; }
        }
}
