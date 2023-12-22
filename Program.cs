using Skola.NET.Models;

namespace Skola.NET
{
    internal class Program
    {
        static void DisplayResults<T>(IEnumerable<T> results)
        {
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
        static void Main()
        {
            using (var context = new SkolaContext())
            {
                bool continueProgram = true;

                while (continueProgram)// Loop för Menyn
                {
                    Console.Clear();
                    Console.WriteLine("Välj alternativ:");
                    Console.WriteLine("1. Personal");
                    Console.WriteLine("2. Studenter");
                    Console.WriteLine("3. Klasser");
                    Console.WriteLine("4. Betyg satta senaste månaden");
                    Console.WriteLine("5. Kursstatistik");
                    Console.WriteLine("6. Lägg till student");
                    Console.WriteLine("7. Lägg till personal");
                    Console.WriteLine("8. Exit");

                    Console.Write("Ditt val: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            FetchPersonal(context);
                            break;
                        case "2":
                            FetchAllStudents(context);
                            break;
                        case "3":
                            FetchStudentsInClass(context);
                            break;
                        case "4":
                            FetchGradesLastMonth(context);
                            break;
                        case "5":
                            FetchCourseStatistics(context);
                            break;
                        case "6":
                            AddNewStudent(context);
                            break;
                        case "7":
                            AddNewPersonal(context);
                            break;
                        case "8":
                            continueProgram = false;
                            break;
                        default:
                            Console.WriteLine("Ogilyigt val. Försök igen.");
                            break;
                    }
                    if (continueProgram)
                    {
                        Console.WriteLine("\nTryck på valfri tangent för att komma tillbaka till menyn...");
                        Console.ReadKey(true);
                    }
                }
            }
        }

        static void FetchPersonal(SkolaContext context) // Alla alternativ för att hämta personal
        {
            Console.WriteLine("Välj alternativ:");
            Console.WriteLine("1. Alla anställda");
            Console.WriteLine("2. Lärare");
            Console.WriteLine("3. Annan personal");

            Console.Write("Ditt val: ");
            string categoryChoice = Console.ReadLine();

            IQueryable<Personal> personalQuery;

            switch (categoryChoice)
            {
                case "1":
                    personalQuery = context.Personals;
                    break;
                case "2":
                    personalQuery = context.Personals.Where(p => p.Befattning == "Lärare");
                    break;
                case "3":
                    personalQuery = context.Personals.Where(p => p.Befattning != "Lärare");
                    break;
                default:
                    Console.WriteLine("Ogiltigt val. Skickas tillbaka till menyn.");
                    return;
            }

            var personalList = personalQuery.ToList();

            if (personalList.Any())
            {
                Console.WriteLine("Personal Information:");
                foreach (var personal in personalList)
                {
                    Console.WriteLine($"ID: {personal.PersonalId}, Name: {personal.Namn}, Position: {personal.Befattning}");
                }
            }
            else
            {
                Console.WriteLine("Ingen personal hittad.");
            }
        }

        static void FetchAllStudents(SkolaContext context) // Alla alternativ för att hämta elever
        {
            Console.WriteLine("Välj alternativ:");
            Console.WriteLine("1. Sortera efter förnamn (A-Ö)");
            Console.WriteLine("2. Sortera efter förnamn (Ö-A)");
            Console.WriteLine("3. Sortera efter efternamn (A-Ö)");
            Console.WriteLine("4. Sortera efter efternamn (Ö-A)");

            Console.Write("Ditt val: ");
            string sortingChoice = Console.ReadLine();

            IQueryable<Student> studentsQuery;

            switch (sortingChoice)
            {
                case "1":
                    studentsQuery = context.Students.OrderBy(s => s.Namn);
                    break;
                case "2":
                    studentsQuery = context.Students.OrderByDescending(s => s.Namn);
                    break;
                case "3":
                    studentsQuery = context.Students.OrderBy(s => s.Namn);
                    break;
                case "4":
                    studentsQuery = context.Students.OrderByDescending(s => s.Namn);
                    break;
                default:
                    Console.WriteLine("Ogiltigt val. Skickas tillbaka till menyn.");
                    return;
            }

            var studentsList = studentsQuery.ToList();

            if (studentsList.Any())
            {
                Console.WriteLine("Student Information:");
                foreach (var student in studentsList)
                {
                    Console.WriteLine($"ID: {student.StudentId}, Name: {student.Namn}, Class: {student.Fkklass?.Klassnamn}");
                }
            }
            else
            {
                Console.WriteLine("Inga studenter hittade.");
            }
        }

        static void FetchStudentsInClass(SkolaContext context) //Hämta alla elever i en klass
        {
            Console.WriteLine("Klasser:");
            var classes = context.Klasses.ToList();
            foreach (var klass in classes)
            {
                Console.WriteLine($"{klass.KlassId}. {klass.Klassnamn}");
            }

            Console.Write("Skriv in Id av klassen: ");
            if (!int.TryParse(Console.ReadLine(), out int chosenClassId))
            {
                Console.WriteLine("Ogiltigt. Skickas tillbaka till menyn.");
                return;
            }

            var studentsInClass = context.Students
                .Where(s => s.FkklassId == chosenClassId)
                .ToList();

            if (studentsInClass.Any())
            {
                Console.WriteLine($"Studenter i klassen {classes.SingleOrDefault(c => c.KlassId == chosenClassId)?.Klassnamn}:");
                foreach (var student in studentsInClass)
                {
                    Console.WriteLine($"ID: {student.StudentId}, Namn: {student.Namn}");
                }
            }
            else
            {
                Console.WriteLine("Inga studenter i klassen.");
            }
        }

        static void FetchGradesLastMonth(SkolaContext context) // Hämta betyg senaste månaden
        {
            DateTime lastMonthDate = DateTime.Now.AddMonths(-1);

            var gradesLastMonth = context.Betygs
                .Where(b => b.Datum >= lastMonthDate)
                .OrderBy(b => b.Datum)
                .ToList();

            if (gradesLastMonth.Any())
            {
                Console.WriteLine("Betyg satta senaste månaden:");
                foreach (var grade in gradesLastMonth)
                {
                    Console.WriteLine($"Student: {grade.Fkstudent?.Namn}, Kurs: {grade.FkkursNavigation?.Kursnamn}, Betyg: {grade.Betyg1}, Datum: {grade.Datum?.ToShortDateString()}");
                }
            }
            else
            {
                Console.WriteLine("Inga betyg hittade denna månad.");
            }
        }

        static void FetchCourseStatistics(SkolaContext context) // Hämtar all kursstatistik
        {
            var courses = context.Kurs.ToList();

            if (!courses.Any())
            {
                Console.WriteLine("Inga kurser hittade.");
                return;
            }

            Console.WriteLine("Kursstatistik:");

            foreach (var course in courses)
            {
                var gradesForCourse = context.Betygs
                    .Where(b => b.Fkkurs == course.KursId)
                    .ToList();

                if (gradesForCourse.Any())
                {
                    double averageGrade = gradesForCourse.Average(b => GradeToNumeric(b.Betyg1));

                    var highestGrade = gradesForCourse.Max(b => GradeToNumeric(b.Betyg1));
                    var lowestGrade = gradesForCourse.Min(b => GradeToNumeric(b.Betyg1));

                    Console.WriteLine($"Kurs: {course.Kursnamn}");
                    Console.WriteLine($"Snittbetyg: {NumericToGrade(averageGrade)}");
                    Console.WriteLine($"Högsta betyg: {NumericToGrade(highestGrade)}");
                    Console.WriteLine($"Lägsta betyg: {NumericToGrade(lowestGrade)}");
                    Console.WriteLine("-------------------------------");
                }
                else
                {
                    Console.WriteLine($"Inga betyg i kursen: {course.Kursnamn}");
                }
            }
        }

        static double GradeToNumeric(string grade) // Konverterar bokstavsbetyg till numeriska betyg
        {
            switch (grade.ToUpper())
            {
                case "A": return 5.0;
                case "B": return 4.0;
                case "C": return 3.0;
                case "D": return 2.0;
                case "E": return 1.0;
                case "F": return 0.0;
                default: return 0.0;
            }
        }

        static string NumericToGrade(double numericGrade) // Konverterar tillbaka betygen
        {
            if (numericGrade >= 4.5) return "A";
            if (numericGrade >= 3.5) return "B";
            if (numericGrade >= 2.5) return "C";
            if (numericGrade >= 1.5) return "D";
            if (numericGrade >= 0.5) return "E";
            return "F";
        }

        static void AddNewStudent(SkolaContext context) // Lägg till ny student
        {
            Console.WriteLine("Skriv in info om ny student:");

            Console.Write("Student Namn: ");
            string studentName = Console.ReadLine();

            Console.Write("Student klass ID: ");
            if (!int.TryParse(Console.ReadLine(), out int classId))
            {
                Console.WriteLine("Ogiltigt klass ID. Avbryter.");
                return;
            }

            var existingClass = context.Klasses.Find(classId);
            if (existingClass == null)
            {
                Console.WriteLine("Klass inte hittad. Avbryter.");
                return;
            }

            var newStudent = new Student
            {
                Namn = studentName,
                FkklassId = classId
            };

            context.Students.Add(newStudent);

            context.SaveChanges();

            Console.WriteLine($"Ny student '{studentName}' har lagts till i '{existingClass.Klassnamn}'.");
        }

        static void AddNewPersonal(SkolaContext context) // Lägg till personal
        {
            Console.WriteLine("Skriv in info om ny persoanl:");

            Console.Write("Namn: ");
            string name = Console.ReadLine();

            Console.Write("Position: ");
            string position = Console.ReadLine();

            Console.Write("Personal ID: ");
            string personalId = Console.ReadLine();

            var newPersonal = new Personal
            {
                Namn = name,
                Befattning = position,
                Personnummer = personalId
            };

            context.Personals.Add(newPersonal);

            context.SaveChanges();

            Console.WriteLine($"Ny personal '{name}' har lagts till som '{position}'.");
        }
    }
}