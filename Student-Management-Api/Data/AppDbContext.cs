using StudentManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ===== DbSets for all entities =====
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentTeacher> StudentTeachers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Configure Parent table =====
            modelBuilder.Entity<Parent>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            // ===== Configure Teacher table =====
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn(seed: 1, increment: 1);

                // ✅ ADD THIS to fix the warning
                entity.Property(e => e.Salary)
                .HasPrecision(10, 2);  // 10 digits total, 2 after decimal
            });

            // ===== Configure Student table =====
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(seed: 1, increment: 1);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // ===== Configure Student - Parent Relationship (1:Many) =====
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Parent)                          // Student has ONE Parent
                .WithMany(p => p.Students)                      // Parent has MANY Students
                .HasForeignKey(s => s.ParentId)                 // Foreign key is ParentId
                .OnDelete(DeleteBehavior.Restrict);             // Restrict deletion if has students

            // ===== Configure Student - Teacher Relationship (Many:Many) =====
            // Configure the junction table
            modelBuilder.Entity<StudentTeacher>()
                .HasKey(st => new { st.StudentId, st.TeacherId });  // Composite key

            // Student side of relationship
            modelBuilder.Entity<StudentTeacher>()
                .HasOne(st => st.Student)                       // StudentTeacher has ONE Student
                .WithMany(s => s.StudentTeachers)               // Student has MANY StudentTeachers
                .HasForeignKey(st => st.StudentId);             // Foreign key

            // Teacher side of relationship
            modelBuilder.Entity<StudentTeacher>()
                .HasOne(st => st.Teacher)                       // StudentTeacher has ONE Teacher
                .WithMany(t => t.StudentTeachers)               // Teacher has MANY StudentTeachers
                .HasForeignKey(st => st.TeacherId);             // Foreign key

            // ===== Configure User table =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(seed: 1, increment: 1);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            // ===== Configure User - Student Relationship (Optional) =====
            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)                         // User has ONE Student (optional)
                .WithMany()                                      // Student has no back-reference
                .HasForeignKey(u => u.StudentId)                // Foreign key is StudentId
                .OnDelete(DeleteBehavior.SetNull)               // Delete User, set StudentId to NULL
                .IsRequired(false);                              // StudentId is OPTIONAL
        }
    }
}