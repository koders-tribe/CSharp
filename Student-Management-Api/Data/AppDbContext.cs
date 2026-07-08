using StudentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}


// Code	                                     Purpose
// using Microsoft.EntityFrameworkCore	Imports EF Core classes
// AppDbContext :                       DbContext	Makes this the application's database context
// DbContextOptions	                    Receives database configuration
// base(options)	                    Passes configuration to EF Core
// DbSet<Student>	                    Maps the Students table
// DbSet<Teacher>	                    Maps the Teachers table
// DbSet<Parent>	                    Maps the Parents table
// OnModelCreating()	                Configures how models map to the database
// HasKey()	                            Sets the primary key
// Property()	                        Configures a column
// IsRequired()	                        Makes the column NOT NULL
// HasMaxLength(100)	                Limits the column length to 100 characters

// 1. AppDbContext constructor was called
//    └─ Received connection options
//    └─ Knows: Connect to localhost SQL Server

// 2. DbSet<Student> Students
//    └─ Tells EF Core: "Students table exists"

// 3. OnModelCreating configured
//    └─ Tells EF Core: "Id is key, Name required, Email required"

// 4. When you call: .ToList()
//    └─ EF Core generates: SELECT * FROM Students
//    └─ DbContext translates to SQL
//    └─ Sends to SQL Server
//    └─ SQL Server returns data
//    └─ DbContext converts to Student objects
//    └─ You get: List<Student>