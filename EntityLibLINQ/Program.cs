using EntityLibLINQ.Comparers;
using EntityLibLINQ.DbClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ConsoleUI.ConsoleTable.Structural;
using Microsoft.Extensions.Options;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Intrinsics.Arm;

namespace EntityLibLINQ
{
    internal class Program
    {
        public static DbContextOptions<LibraryContext> Options = new();


        public static void InitOptions()
        {
            ConfigurationBuilder buider = new();
            buider.AddJsonFile("appconfig.json");
            IConfigurationRoot config = buider.Build();
            string cs = config.GetConnectionString("SqlServerConnection");
            DbContextOptionsBuilder<LibraryContext> optionsBuilder = new();
            Options = optionsBuilder.UseSqlServer(cs).Options;
        }

        public static void ShowResult(string hint, string result)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{hint}:\t");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{result}");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void ShowMethodInfo(string methodName, string hint)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(methodName);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"  ->  {hint}");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void ShowErrorMessage(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(message);
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void ShowTableName(string name)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("\t\t\t\t\t\t");
            Console.WriteLine(name);
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static int NumberOfTakedBookByStudent(int id)
        {
            int number = -1;
            using (LibraryContext db = new(Options))
            {
                number = db.SCards.ToList()
                                  .Count(sc => sc.IdBook == id);
            }
            return number;
        }

        public static int NumberOfTakedBookByTeachers(int id)
        {
            int number = -1;
            using (LibraryContext db = new(Options))
            {
                number = db.TCards.ToList()
                                  .Count(sc => sc.IdBook == id);
            }
            return number;
        }

        public static float FindPercent(float main, float second) => second * 100 / main;

        static void Main(string[] args)
        {
            InitOptions();
            Console.SetWindowSize(160, 40);


            string incorrectId = "Plese Enter a id (int) where bigger than zero (0) !";

            using (LibraryContext db = new(Options))
            {

                #region Count -> Find Total Students number where Gorup Id equals user writed id

                ShowMethodInfo("Count", "Find Total Students number where Gorup Id equals user writed id");

            startInitId:
                try
                {
                    Console.Clear();
                    Console.Write("Enter Student id: ");

                    int id = Convert.ToInt32(Console.ReadLine());

                    ShowResult($"Students where groups id equasls {id}. ", db.Students.ToList().Count(s => s.IdGroup == id).ToString());
                }
                catch (Exception) { ShowErrorMessage(incorrectId); goto startInitId; }

                #endregion

                #region Sum -> Find total quantity of books where taked student id equal wanted id

                ShowMethodInfo("Sum", "Find total quantity of books where taked student id equal wanted id");

            initStudentID:
                try
                {
                    Console.Clear();
                    Console.Write("Enter Student id: ");

                    int id = Convert.ToInt32(Console.ReadLine());

                    string username = db.Students.ToList().Find(s => s.Id == id).FirstName;
                    float totalQuantity = 0;
                    db.SCards.ToList().FindAll(sc => sc.IdStudent == id).ForEach(sc => totalQuantity += db.Books.ToList().Find(b => b.Id == sc.IdBook).Quantity);

                    ShowResult($"Total quantity of books where {username} taked.", totalQuantity.ToString());
                }
                catch (Exception) { ShowErrorMessage(incorrectId); goto initStudentID; }

                #endregion

                #region Min -> Find the depot of the first Teacher who purchased the most unsuccessful Book of the least favoured Press house by Students

                ShowMethodInfo("Min", "Find the depot of the first Teacher who purchased the most unsuccessful Book of the least favoured Press house by students");

            startMin:
                try
                {

                    string teacherFirstname = db.Teachers.ToList()
                                                         .Find(t => t.Id == db.TCards.ToList()
                                                         .FirstOrDefault(tc => tc.IdBook == db.SCards.ToList()
                                                         .Min(new IdBookComparer()).IdBook).IdTeacher).FirstName;



                    ShowResult("Teacher who purchased the most unsuccessful Book", teacherFirstname);
                }
                catch (Exception ex) { ShowErrorMessage(ex.Message); goto startMin; }

                #endregion

                #region Max -> Find the Faculties of the most literate Student with the greatest number of polpular Books using the Theme in the entered id

                ShowMethodInfo("Max", "Find the Faculties of the most literate Student with the greatest number of polpular Books using the Theme in the entered id");

            startMax:
                try
                {

                    string faculte = db.Faculties.ToList().Find(f => f.Id == db.Groups.ToList().Find(g => g.Id == db.Students.ToList()
                                                       .Find(t => t.Id == db.SCards.ToList()
                                                       .FirstOrDefault(tc => tc.IdBook == db.SCards.ToList()
                                                       .Max(new IdBookComparer()).IdBook).IdStudent).IdGroup).IdFaculty).Name;



                    ShowResult("Faculty of the most literate Student who has the most successful Book", faculte);
                }
                catch (Exception ex) { ShowErrorMessage(ex.Message); goto startMax; }

                #endregion

                #region Average -> Show some stats of the library

                ShowMethodInfo("Average", "Show all stats of the library");

            startAverage:
                try
                {

                    #region Students Stats

                    ShowTableName("Students Stats");

                    int countBooks = db.Books.ToList().Count;
                    float averageBook = db.SCards.ToList().Average(sc => FindPercent(countBooks, NumberOfTakedBookByStudent(sc.IdStudent)));

                    List<Column> columnsStudent = new() {
                    new ("FirstName",20,ConsoleColor.DarkMagenta),
                    new ("LastName",25,ConsoleColor.DarkMagenta),
                    new ("Literacy",15,averageBook,ConsoleColor.DarkMagenta),
                    new ("Average",15,averageBook, ConsoleColor.DarkMagenta),
                    };
                    Table studentStat = new(columnsStudent);


                    db.Students.ToList().ForEach(s =>
                    {
                        studentStat.AddRow(new Row(new() { s.FirstName, s.LastName, FindPercent(countBooks, NumberOfTakedBookByStudent(s.Id)).ToString(), averageBook.ToString() }));
                    });


                    studentStat.Write();

                    #endregion

                    Console.ReadKey();

                    #region Teachers Stats

                    ShowTableName("Teachers Stats");

                    countBooks = db.Books.ToList().Count;
                    averageBook = db.TCards.ToList().Average(sc => FindPercent(countBooks, NumberOfTakedBookByTeachers(sc.IdTeacher)));

                    List<Column> columnsTeachers = new() {
                    new ("FirstName",20,ConsoleColor.DarkMagenta),
                    new ("LastName",25,ConsoleColor.DarkMagenta),
                    new ("Department",40,ConsoleColor.DarkMagenta),
                    new ("Literacy",15,averageBook,ConsoleColor.DarkMagenta),
                    new ("Average",15,averageBook, ConsoleColor.DarkMagenta),
                    };
                    Table teachersStat = new(columnsTeachers);


                    db.Teachers.ToList().ForEach(t =>
                    {
                        teachersStat.AddRow(new Row(new() { t.FirstName, t.LastName, db.Departments.ToList().Find(d => t.IdDep == d.Id).Name, FindPercent(countBooks, NumberOfTakedBookByTeachers(t.Id)).ToString(), averageBook.ToString() }));
                    });


                    teachersStat.Write();

                    #endregion

                    Console.ReadKey();



                }
                catch (Exception ex) { ShowErrorMessage(ex.Message); goto startAverage; }

                #endregion

            }
        }
    }
}