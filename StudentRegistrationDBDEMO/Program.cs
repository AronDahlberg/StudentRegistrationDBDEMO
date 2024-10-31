using Microsoft.EntityFrameworkCore;

namespace StudentRegistrationDBDEMO
{
    internal class Program
    {
        private static bool Running { get; set; } = true;
        private static StudentDbContext DbContext { get; set; } = new();

        static void Main(string[] args)
        {
            while (Running)
            {
                Console.Clear();
                WriteMainMenu();

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.E: Running = false; break;

                    case ConsoleKey.R:
                        try { RegisterNewStudent(); }

                        catch (ArgumentException)
                        { WriteArgumentException(); }

                        break;

                    case ConsoleKey.C:
                        try { EditExistingStudent(); }

                        catch (ArgumentOutOfRangeException)
                        { WriteStudentDoesNotExist(); }
                        catch (ArgumentException)
                        { WriteArgumentException(); }

                        break;

                    case ConsoleKey.L:

                        Console.Clear();
                        WriteAllStudents(DbContext.Students);
                        
                        break;

                    default: continue;
                }

                if (Running)
                {
                    WriteWaitingForInput();

                    Console.ReadKey(); 
                }
            }
        }



        #region MenuTexts

        private static void WriteMainMenu()
        {
            Console.Write(
                "E: Exit\n" +
                "R: Register student\n" +
                "C: Change student data\n" +
                "L: List all students\n");
        }

        private static void WriteAllStudents(DbSet<Student>? students)
        {
            if (students == null || !students.Any())
            {
                Console.Write("No students exist");
                return;
            }

            Console.Write(
                "Students:\n");

            foreach (var student in students)
            {
                Console.Write(
                    $"    Id: '{student.StudentId}'\n" +
                    $"      First name: '{student.StudentFirstName}'\n" +
                    $"      Last name: '{student.StudentLastName}'\n" +
                    $"      City of residence: '{student.StudentCity}'\n");
            }
        }

        private static void WriteWaitingForInput()
        {
            Console.Write("\n" +
                "Press any key to continue\n");
        }

        private static void WriteArgumentException()
        {
            Console.Write("\n" +
                "Input argument is invalid, please try again.");
        }

        private static void WriteStudentDoesNotExist()
        {
            Console.Write("\n" +
                "Student does not exist, please try again");
        }

        private static void WriteFullNameQuery()
        {
            Console.Write(
                "Input full name of student: ");
        }

        private static void WriteNewFirstNameQuery()
        {
            Console.Write(
                "Input new first name of student: ");
        }

        private static void WriteNewLastNameQuery()
        {
            Console.Write(
                "Input new last name of student: ");
        }

        private static void WriteCityQuery()
        {
            Console.Write(
                "Input city of residence: ");
        }

        private static void WriteNewCityQuery()
        {
            Console.Write(
                "Input new city of residence: ");
        }

        private static void WriteIdQuery()
        {
            Console.Write(
                "Input Id of student: ");
        }

        private static void WritePropertyQuery()
        {
            Console.Write(
                "Input name of property to change: ");
        }

        private static void WriteFirstNameChangeSuccesfull(string? previousFirstName, string newFirstName)
        {
            Console.Write(
               $"Changed first name from '{previousFirstName}' to '{newFirstName}'");
        }

        private static void WriteLastNameChangeSuccesfull(string? previousLastName, string newLastName)
        {
            Console.Write(
                $"Changed last name from '{previousLastName}' to '{newLastName}'");
        }

        private static void WriteFullNameChangeSuccesfull(string? previousFirstName, string newFirstName, string? previousLastName, string newLastName)
        {
            Console.Write(
               $"Changed first name from '{previousFirstName}' to '{newFirstName}'\n" +
               $"Changed last name from '{previousLastName}' to '{newLastName}'");
        }

        private static void WriteCityChangeSuccesfull(string? previousCity, string newCity)
        {
            Console.Write(
                $"Changed city of residence from '{previousCity}' to '{newCity}'");
        }

        private static void WriteStudentAddedDisplay(Student student)
        {
            Console.Write(
                student.StudentLastName != string.Empty
                ? $"Student '{student.StudentLastName}, {student.StudentFirstName}' in '{student.StudentCity}' has been registered\n"
                : $"Student '{student.StudentFirstName}' in '{student.StudentCity}' has been registered\n");
        }

        private static void WriteStudentData(Student student)
        {
            Console.Write(
                $"Student data for id '{student.StudentId}':\n" +
                $"    First name: '{student.StudentFirstName}'\n" +
                $"    Last name: '{student.StudentLastName}'\n" +
                $"    City: '{student.StudentCity}'\n");
        }

        #endregion



        #region Menu actions

        private static void RegisterNewStudent()
        {
            string name = GetStudentName();

            string cityName = GetCityName();

            // Find the index of the first space in the name if it exists
            int spaceIndex = name.IndexOf(' ');

            Student student = new()
            {
                StudentFirstName = spaceIndex != -1 ? name[..spaceIndex] : name,
                StudentLastName = spaceIndex != -1 ? name[(spaceIndex + 1)..] : string.Empty,
                StudentCity = cityName
            };

            DbContext.Add(student);
            DbContext.SaveChanges();

            Console.Clear();
            WriteStudentAddedDisplay(student);
        }

        private static void EditExistingStudent()
        {
            Student student = GetStudent();

            Console.Clear();
            WriteStudentData(student);
            WritePropertyQuery();

            switch (Console.ReadLine() ?? "")
            {
                case "First name": EditStudentFirstName(student); break;

                case "Last name": EditStudentLastName(student); break;

                case "City": EditStudentCity(student); break;

                default: throw new ArgumentException("Invalid parameter name");
            }
        }

        private static void EditStudentFirstName(Student student)
        {
            string? previousFirstName = student.StudentFirstName;

            Console.Clear();
            WriteNewFirstNameQuery();

            string firstNameInput = Console.ReadLine() ?? "";

            ArgumentException.ThrowIfNullOrWhiteSpace(firstNameInput, "Invalid name");

            // Find the index of the first space in the name if it exists
            int spaceIndex = firstNameInput.IndexOf(' ');
            
            if (spaceIndex == -1) // No space in the name
            {
                student.StudentFirstName = firstNameInput;
                DbContext.SaveChanges();

                Console.Clear();
                WriteFirstNameChangeSuccesfull(previousFirstName, firstNameInput);
            }
            else // Space in the name
            {
                string? previousLastName = student.StudentLastName;

                string newFirstName = firstNameInput[..spaceIndex];
                string newLastName = $"{firstNameInput[(spaceIndex + 1)..]} {previousLastName}";

                student.StudentFirstName = newFirstName;
                student.StudentLastName = newLastName;
                DbContext.SaveChanges();
                
                Console.Clear();
                WriteFullNameChangeSuccesfull(previousFirstName, newFirstName, previousLastName, newLastName);
            }
        }

        private static void EditStudentLastName(Student student)
        {
            string? previousLastName = student.StudentLastName;

            Console.Clear();
            WriteNewLastNameQuery();

            string lastNameInput = Console.ReadLine() ?? "";

            student.StudentLastName = lastNameInput;
            DbContext.SaveChanges();

            Console.Clear();
            WriteLastNameChangeSuccesfull(previousLastName, lastNameInput);
        }

        private static void EditStudentCity(Student student)
        {
            string? previousCity = student.StudentCity;

            Console.Clear();
            WriteNewCityQuery();

            string cityInput = Console.ReadLine() ?? "";

            ArgumentException.ThrowIfNullOrWhiteSpace(cityInput, "Invalid city name");

            student.StudentCity = cityInput;
            DbContext.SaveChanges();

            Console.Clear();
            WriteCityChangeSuccesfull(previousCity, cityInput);
        }

        #endregion



        #region Program actions

        private static Student GetStudent()
        {
            Console.Clear();
            WriteIdQuery();

            string idInput = Console.ReadLine() ?? "";

            if (!int.TryParse(idInput, out int id))
            {
                throw new ArgumentException("Invalid id");
            }

            if (!(DbContext.Students?.Any(s => s.StudentId == id) ?? false))
            {
                throw new ArgumentOutOfRangeException("Student id does not exist");
            }

            return DbContext.Students.First(s => s.StudentId == id);
        }

        private static string GetStudentName()
        {
            Console.Clear();
            WriteFullNameQuery();

            string nameInput = Console.ReadLine() ?? "";

            ArgumentException.ThrowIfNullOrWhiteSpace(nameInput, "Invalid name");

            return nameInput;
        }

        private static string GetCityName()
        {
            Console.Clear();
            WriteCityQuery();

            string cityInput = Console.ReadLine() ?? "";

            ArgumentException.ThrowIfNullOrWhiteSpace(cityInput, "Invalid city name");

            return cityInput;
        }

        #endregion
    }
}
