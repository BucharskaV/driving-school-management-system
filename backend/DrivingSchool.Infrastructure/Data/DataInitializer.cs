using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Authentification;

namespace DrivingSchool.Infrastructure.Data;

public static class DataInitializer
{
    //booked setting for checking alternative flows of "Book Lesson" use case :
    //21.12.2026 14.30(utc)->16.30 booked instructor FirstnameT9 LastnameT9 - TR00009
    //22.12.2026 14.30(utc)->16.30 booked room for offline theoretical lesson Room A1 
    //24.12.2026 14.30(utc)->16.30 booked car for practical lesson 
    //
    //test credentials
    //Student   klara@gmail.com/Student123!
    //Student   anna@gmail.com/Student123!
    //Instructor    instructor1@drivingschool.test/Instructor123!
    //Admin      admin@drivingschool.test/Admin123!
    public static void Initialize(ApplicationDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var addresses = new List<Address>();
        for (var i = 1; i <= 15; i++)
        {
            addresses.Add(new Address($"City{i}", $"District{i}", $"Street{i}", i));
        }
        dbContext.Addresses.AddRange(addresses);

        var categories = new List<Category>();
        for (var i = 1; i <= 15; i++)
        {
            categories.Add(new Category($"Category{i}", 18));
        }
        dbContext.Categories.AddRange(categories);

        var students = new List<Student>
        {
            new Student("Klara", "Nowacka", "12345678901", Role.Student,"700900800", "klara@gmail.com", DateTime.UtcNow.AddYears(-20)),
            new Student("Anna", "Ivanova", "12345678902", Role.Student, "123456789", "anna@gmail.com", DateTime.UtcNow.AddYears(-22))
        };
        dbContext.Users.AddRange(students);

        var certifications = new List<Certification>();
        var instructors = new List<Instructor>();
        for (var i = 1; i <= 5; i++)
        {
            instructors.Add(new Instructor(
                new List<InstructorType> { InstructorType.PracticalInstructor },
                $"FirstnameP{i}",
                $"LastnameP{i}",
                Role.Instructor,
                (90000000000 + i).ToString(),
                (123000000 + i).ToString(),
                null,
                $"PR{i:00000}",
                5000 + i * 100,
                500,
                $"DL{i:0000000}",
                $"{i:000000}"
            ));
        }
        for (var i = 6; i <= 10; i++)
        {
            var instructor = new Instructor(
                new List<InstructorType> { InstructorType.TheoreticalInstructor },
                $"FirstnameT{i}",
                $"LastnameT{i}",
                Role.Instructor,
                (91000000000 + i).ToString(),
                (123400000 + i).ToString(),
                null,
                $"TR{i:00000}",
                5000 + i * 100,
                500
            );
            instructor.AddCertification($"Certification {i}");
            instructors.Add(instructor);
        }
        for (var i = 11; i <= 15; i++)
        {
            var types = new List<InstructorType>
            {
                InstructorType.PracticalInstructor,
                InstructorType.TheoreticalInstructor
            };
            instructors.Add(new Instructor(
                types,
                $"Both{i}",
                $"Last{i}",
                Role.Instructor,
                (91200000000 + i).ToString(),
                (123450000 + i).ToString(),
                null,
                $"WW{i:00000}",
                6000 + i * 100,
                700,
                $"DL{i:0000000}",
                $"{i:000000}"
            ));
        }
        dbContext.Certifications.AddRange(certifications);
        dbContext.Users.AddRange(instructors);
        dbContext.SaveChanges();

        var cars = new List<Car>();
        for (var i = 1; i <= 15; i++)
        {
            cars.Add(new Car($"Brand{i}", $"Model{i}", $"REG{i:00000}"));
        }
        dbContext.Cars.AddRange(cars);
        dbContext.SaveChanges();

        var courses = new List<Course>();
        for (var i = 1; i <= 15; i++)
        {
            courses.Add(new Course(categories[i - 1], $"Course{i}", 100 + i));
        }
        dbContext.Courses.AddRange(courses);
        dbContext.SaveChanges();
        Console.WriteLine(dbContext.Categories.FirstOrDefault(c => c.Id == 1)?.Courses);

        var lessons = new List<Lesson>();
        foreach (var course in courses)
        {
            for (var i = 1; i <= 15; i++)
            {
                if (i % 2 == 0)
                {
                    lessons.Add(new PracticalLesson(
                        course,
                        cars[0],
                        $"PracticalLesson_{course.Title}_{i}",
                        i,
                        TimeSpan.FromHours(1),
                        addresses[(i - 1) % addresses.Count]));
                }
                else
                {
                    lessons.Add(new TheoreticalLesson(
                        course,
                        $"TheoryLesson_{course.Title}_{i}",
                        i,
                        TimeSpan.FromHours(1),
                        $"Topic{i}",
                        false,
                        "A1"
                    ));
                }
            }
        }
        dbContext.Lessons.AddRange(lessons);
        dbContext.SaveChanges();

        var enrollments = new List<Enrollment>();
        for (var i = 0; i < 15; i++)
        {
            if (i % 2 == 0)
            {
                enrollments.Add(new Enrollment(students[0], courses[i]));
            }
            else
            {
                enrollments.Add(new Enrollment(students[1], courses[i]));
            }
        }
        dbContext.Enrollments.AddRange(enrollments);
        dbContext.SaveChanges();
        
        var lessonInstructors = new List<LessonInstructor>();
        foreach (var lesson in lessons)
        {
            List<Instructor> eligibleInstructors;

            if (lesson is PracticalLesson)
            {
                eligibleInstructors = instructors
                    .Where(i => i.Specializations
                        .Any(s => s.Type == InstructorType.PracticalInstructor))
                    .ToList();
            }
            else if (lesson is TheoreticalLesson)
            {
                eligibleInstructors = instructors
                    .Where(i => i.Specializations
                        .Any(s => s.Type == InstructorType.TheoreticalInstructor))
                    .ToList();
            }
            else
            {
                continue;
            }

            for (int j = 0; j < Math.Min(4, eligibleInstructors.Count); j++)
            {
                var instructor = eligibleInstructors[(lesson.SequenceNumber + j) % eligibleInstructors.Count];
                lessonInstructors.Add(new LessonInstructor(lesson, instructor));
            }
        }

        dbContext.LessonInstructors.AddRange(lessonInstructors);
        dbContext.SaveChanges();

        var lessonProgresses = new List<LessonProgress>();
        foreach (var student in students)
        {
            foreach (var enrollment in student.Enrollments)
            {
                var courseLessons = enrollment.Course.Lessons
                    .OrderBy(l => l.SequenceNumber)
                    .ToList();

                foreach (var lesson in courseLessons)
                {
                    var status = lesson.SequenceNumber switch
                    {
                        1 or 2 => ProgressStatus.Completed,
                        3 => ProgressStatus.Available,
                        _ => ProgressStatus.Locked
                    };

                    lessonProgresses.Add(new LessonProgress(student, lesson, status));
                }
            }
        }
        
        var bookingTeacherLesson = lessons.OfType<TheoreticalLesson>().FirstOrDefault(l => l.Id == 107);
        if (bookingTeacherLesson != null) bookingTeacherLesson.RoomNumber = "A2";
        var bookingTeacher = lessonProgresses.FirstOrDefault(b => b.Student == students[0] && b.LessonId == 107);
        if (bookingTeacher != null)
        {
            var instructor = bookingTeacher.Lesson.LessonInstructors.First().Instructor;
            bookingTeacher.ProgressStatus = ProgressStatus.Booked; 
            bookingTeacher.StartTime = new DateTime(2026, 12, 21, 14, 30, 0, DateTimeKind.Utc);
            bookingTeacher.Note = "Note from instructor";
            bookingTeacher.Instructor = instructor;
            bookingTeacher.InstructorId = instructor.Id;
        }

        var fee = new ExtraFee(bookingTeacher, 50);

        dbContext.LessonProgresses.AddRange(lessonProgresses);
        if (fee != null) dbContext.ExtraFees.Add(fee);
        dbContext.SaveChanges();
        
        dbContext.SaveChanges();
        
        var bookingRoom = lessonProgresses.FirstOrDefault(b => b.Student == students[0] && b.LessonId == 123);
        if (bookingRoom != null)
        {
            var instructor = bookingRoom.Lesson.LessonInstructors.First().Instructor;
            bookingRoom.ProgressStatus = ProgressStatus.Booked; 
            bookingRoom.StartTime = new DateTime(2026, 12, 22, 14, 30, 0, DateTimeKind.Utc);
            bookingRoom.Note = "Note from instructor";
            bookingRoom.Instructor = instructor;
            bookingRoom.InstructorId = instructor.Id;
        }
        
        var bookingCar = lessonProgresses.FirstOrDefault(b => b.Student == students[0] && b.LessonId == 29);
        if (bookingCar != null)
        {
            var instructor = bookingCar.Lesson.LessonInstructors.First().Instructor;
            bookingCar.ProgressStatus = ProgressStatus.Booked; 
            bookingCar.StartTime = new DateTime(2026, 12, 24, 14, 30, 0, DateTimeKind.Utc);
            bookingCar.Note = "Note from instructor";
            bookingCar.Instructor = instructor;
            bookingCar.InstructorId = instructor.Id;
        }
        var lp = lessonProgresses.FirstOrDefault(b => b.Student == students[0] && b.LessonId == 139);
        if (lp != null) lp.ProgressStatus = ProgressStatus.Locked;
        var lp1 =  lessonProgresses.FirstOrDefault(b => b.Student == students[1] && b.LessonId == 22);
        if (lp1 != null) lp1.ProgressStatus = ProgressStatus.Available;
        var lp2 =  lessonProgresses.FirstOrDefault(b => b.Student == students[1] && b.LessonId == 131);
        if (lp2 != null) lp2.ProgressStatus = ProgressStatus.Locked;

        dbContext.SaveChanges();
        
        var extraCourses = new List<Course>();
        var courseIndex = dbContext.Courses.Count() + 1;

        foreach (var category in categories)
        {
            for (int i = 1; i <= 10; i++)
            {
                extraCourses.Add(new Course(
                    category,
                    $"ExtraCourse_{category.Name}_{i}",
                    100 + courseIndex++
                ));
            }
        }

        dbContext.Courses.AddRange(extraCourses);
        dbContext.SaveChanges();
        
        var specialInstructor = new Instructor(
            new List<InstructorType>
            {
                InstructorType.PracticalInstructor,
                InstructorType.TheoreticalInstructor
            },
            "Sample",
            "Instructor",
            Role.Instructor,
            "99999999999",
            "999999999",
            null,
            "SP10000",
            8000,
            1000,
            "DL1000000",
            "100000"
        );

        dbContext.Users.Add(specialInstructor);
        
        var admin = new User("Antonio", "Rolling", Role.Admin, "91009999999", "777890123", "admin@drivingschool.test");
        dbContext.Users.Add(admin);
 
        dbContext.SaveChanges();
 
        var passwordHasher = new PasswordHasher();
        var testInstructor = instructors[0]; // FirstnameP1 LastnameP1
        testInstructor.Email = "instructor1@drivingschool.test";
 
        var credentials = new List<UserCredential>
        {
            new(students[0], passwordHasher.Hash("Student123!")),
            new(students[1], passwordHasher.Hash("Student123!")),
            new(testInstructor, passwordHasher.Hash("Instructor123!")),
            new(admin, passwordHasher.Hash("Admin123!"))
        };
        
        dbContext.UserCredentials.AddRange(credentials);
        dbContext.SaveChanges();
        
        var availableProgresses = lessonProgresses
            .Where(lp =>
                lp.ProgressStatus == ProgressStatus.Available &&
                lp.InstructorId == null)
            .OrderByDescending(lp => lp.LessonId)
            .Take(5)
            .ToList();

        for (int i = 0; i < availableProgresses.Count; i++)
        {
            var progress = availableProgresses[i];

            progress.ProgressStatus = ProgressStatus.Booked;
            progress.Instructor = specialInstructor;
            progress.InstructorId = specialInstructor.Id;
            progress.Note = $"Sample booking {i + 1}";
            progress.StartTime = DateTime.UtcNow.Date
                .AddDays(i + 1)
                .AddHours(14)
                .AddMinutes(30);
        }

        dbContext.SaveChanges();
    }
}