using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<InstructorSpecialization> InstructorSpecializations { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonProgress> LessonProgresses { get; set; }
    public DbSet<ExtraFee> ExtraFees { get; set; }
    public DbSet<LessonInstructor> LessonInstructors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCredential> UserCredentials => Set<UserCredential>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Role)
                .IsRequired()
                .HasConversion<string>();;
            entity.Ignore(i => i.FullName);
            entity.HasIndex(p => p.Pesel)
                .IsUnique();
            entity.Property(p => p.Pesel)
                .IsRequired()
                .HasMaxLength(11);
            entity.Property(p => p.Email)
                .HasMaxLength(50);
            entity.Property(p => p.PhoneNumber)
                .IsRequired();
            entity.HasDiscriminator<string>("UserType")
                .HasValue<Student>("Student")
                .HasValue<Instructor>("Instructor");
        });
        
        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.Property(i => i.InstructorCode)
                .IsRequired()
                .HasMaxLength(7);
            entity.HasIndex(i => i.InstructorCode)
                .IsUnique();
            entity.Property(i => i.BaseSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.Property(i => i.Bonus)
                .HasColumnType("decimal(18,2)");
            entity.Ignore(i => i.TotalSalary);
            entity.Property(i => i.DrivingLicenseNumber)
                .HasMaxLength(20);
            entity.Property(i => i.MedicalCertificateNumber)
                .HasMaxLength(30);
            entity.HasIndex(i => i.DrivingLicenseNumber)
                .IsUnique();
            entity.HasIndex(i => i.MedicalCertificateNumber)
                .IsUnique();
            entity.HasMany(i => i.Certifications)
                .WithOne(s => s.Instructor)
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(i => i.Specializations)
                .WithOne(s => s.Instructor)
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InstructorSpecialization>(entity =>
        {
            entity.ToTable("InstructorSpecializations");
            entity.HasKey(x => new { x.InstructorId, x.Type });
            entity.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>();
        });
        
        modelBuilder.Entity<Certification>(entity =>
        {
            entity.ToTable("Certifications");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .ValueGeneratedOnAdd();
            entity.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(200);
        });
        
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(s => s.DateOfBirth)
                .IsRequired()
                .HasColumnType("date");
            entity.Ignore(i => i.Age);
        });
        
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Addresses");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .ValueGeneratedOnAdd();
            entity.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.District)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.HouseNumber)
                .IsRequired();
        });
        
        modelBuilder.Entity<Car>(entity =>
        {
            entity.ToTable("Cars");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            entity.Property(c => c.Brand)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(c => c.Model)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(c => c.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(20);
            entity.HasIndex(c => c.RegistrationNumber)
                .IsUnique();
        });
        
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("Lessons");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Id)
                .ValueGeneratedOnAdd();
            entity.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(l => l.SequenceNumber)
                .IsRequired();
            entity.Property(l => l.Duration)
                .IsRequired()
                .HasColumnType("time");
            entity.HasDiscriminator<string>("LessonType")
                .HasValue<PracticalLesson>("PracticalLesson")
                .HasValue<TheoreticalLesson>("TheoreticalLesson");
            entity.HasOne(l => l.Course)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<PracticalLesson>(entity =>
        {
            entity.Property(p => p.StartLocationId)
                .IsRequired();
            entity.HasOne(p => p.StartLocation)
                .WithMany()
                .HasForeignKey(p => p.StartLocationId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(p => p.Car)
                .WithMany(c => c.PracticalLessons)
                .HasForeignKey(p => p.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<TheoreticalLesson>(entity =>
        {
            entity.Property(t => t.Topic)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(t => t.RoomNumber)
                .HasMaxLength(10);
            entity.Property(t => t.IsOnline)
                .IsRequired();
        });
        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasIndex(c => c.Name)
                .IsUnique();
            entity.Property(c => c.MinimumAge)
                .IsRequired();
        });
        
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            entity.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(c => c.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("Enrollments");
            entity.HasKey(e => new { e.StudentId, e.CourseId });
            entity.Property(e => e.EnrollmentDate)
                .IsRequired()
                .HasColumnType("date");
            entity.Property(e => e.IsPassed)
                .IsRequired();
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.NoAction);
        });
        
        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.ToTable("LessonProgresses");
            entity.HasKey(lp =>  new { lp.StudentId, lp.LessonId });
            entity.Property(lp => lp.ProgressStatus)
                .HasConversion<string>()
                .IsRequired();
            entity.Property(lp => lp.StartTime)
                .HasColumnType("datetime2");
            entity.Property(lp => lp.Note)
                .HasMaxLength(200);
            entity.Property(lp => lp.EndTime)
                .HasColumnType("datetime2");
            entity.HasOne(lp => lp.Student)
                .WithMany(s => s.LessonProgresses)
                .HasForeignKey(lp => lp.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(lp => lp.Lesson)
                .WithMany(l => l.LessonProgresses)
                .HasForeignKey(lp => lp.LessonId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(lp => lp.Instructor)
                .WithMany(i => i.LessonProgresses)
                .HasForeignKey(lp => lp.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        
        modelBuilder.Entity<ExtraFee>(entity =>
        {
            entity.ToTable("ExtraFees");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id)
                .ValueGeneratedOnAdd();
            entity.Property(f => f.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.Property(f => f.DateOfPayment)
                .HasColumnType("datetime2");
            entity.HasOne(f => f.LessonProgress)
                .WithOne(lp => lp.ExtraFee)
                .HasForeignKey<ExtraFee>(f => new { f.StudentId, f.LessonId })
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<LessonInstructor>(entity =>
        {
            entity.ToTable("LessonInstructors");
            entity.HasKey(x => new { x.LessonId, x.InstructorId });
            entity.Property(x => x.InstructorCode)
                .IsRequired()
                .HasMaxLength(7);
            entity.HasOne(x => x.Lesson)
                .WithMany(l => l.LessonInstructors)
                .HasForeignKey(x => x.LessonId);
            entity.HasOne(x => x.Instructor)
                .WithMany(i => i.LessonInstructors)
                .HasForeignKey(x => x.InstructorId);
            entity.HasIndex(x => new { x.LessonId, x.InstructorCode })
                .IsUnique();
        });
    }
}