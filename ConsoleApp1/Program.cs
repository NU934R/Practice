using Newtonsoft.Json;
using System.Text;

public class FileList
{
    public List<string> Names { get; set; }
    public List<string> Paths { get; set; }
}

class Program
{
    static void CreateNewFile()
    {
        Console.Clear();
        Console.WriteLine("Введіть тему:");
        string name = Console.ReadLine();
        Console.WriteLine("Введіть опис:");
        string text = Console.ReadLine();
        DateTime currentTime = DateTime.Now;
        string time = currentTime.ToString();
        var data = new
        {
            Тема = name,
            Опис = text,
            Час = time
        };
        string directoryPath1 = "data";
        string fileName = name + ".json";

        if (!Directory.Exists(directoryPath1))
        {
            Directory.CreateDirectory(directoryPath1);
        }

        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = Path.Combine(directoryPath1, fileName);
        File.WriteAllText(filePath, jsonData);

        Console.WriteLine("Документ успішно створено");
        Console.ReadKey();
        Main();

    }

    static void ShowList()
    {
        Console.Clear();
        int q = 0;
        FileList filelist = Search();
        Console.WriteLine("0. Повернутися");
        foreach (string file in filelist.Names)
        {
            q++;
            Console.WriteLine($"{q}. {file}");
        }
        Console.WriteLine("Введіть номер документа");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            if (index < 0 || index > filelist.Names.Count)
            {
                Console.WriteLine("Неправилно введений номер");
                Console.ReadKey();
                ShowList();
            }
            else
            {
                if (index == 0)
                {
                    Main();
                }
                else
                {
                    ShowFile(index);
                }
            }
        }
        else
        {
            Console.WriteLine("Неправилно введений номер");
            Console.ReadKey();
            ShowList();
        }
    }

    static void Main()
    {
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;
        FileList fileList = new FileList();
        Console.Clear();
        Console.WriteLine("1. Створити новий документ");
        Console.WriteLine("2. Пошук документів");
        Console.WriteLine("3. Редагувати документ");
        Console.WriteLine("4. Налаштування сортування");
        Console.WriteLine("5. Видалити документ");
        Console.WriteLine("6. Вийти");
        Console.WriteLine();
        Console.Write("Введіть ваш вибір: ");
        char number = Console.ReadKey().KeyChar;
        Console.WriteLine();
        switch (number)
        {
            case '1':
                CreateNewFile();
                break;
            case '2':
                ShowList();
                break;
            case '3':
                ShowFilesToEdit();
                break;
            case '4':
                Sorting();
                break;
            case '5':
                ListFilesToDelete();
                break;
            case '6':
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                Console.ReadKey();
                break;
        }
        Main();
    }

    public static FileList Search()
    {
        FileList filelist = new FileList();
        string relativePath = "Data";
        string searchPattern = "*.json";
        List<string> name = new List<string>();
        List<string> pathes = new List<string>();
        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        if (Directory.Exists(directoryPath))
        {

            string[] files = Directory.GetFiles(directoryPath, searchPattern);
            string filePath = Path.Combine("data", "sorting.txt");
            if (File.Exists(filePath))
            {
                string loadedData = File.ReadAllText(filePath);
                if (loadedData == "0")
                {
                    Array.Sort(files);
                }
                else
                {
                    Array.Sort(files);
                    Array.Reverse(files);
                }
            }
            else
            {
                File.WriteAllText(filePath, "0");
            }
            if (files.Length > 0)
            {

                foreach (string file in files)
                {
                    name.Add(Path.GetFileNameWithoutExtension(file));
                    pathes.Add(Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, file));
                }
            }
            else
            {
                Console.WriteLine("Каталог пустий");
                Console.ReadKey();
                Main();
            }
        }
        else
        {
            Console.WriteLine("Помилка");
            Console.ReadKey();
            Main();
        }
        filelist.Names = name;
        filelist.Paths = pathes;
        return filelist;
    }

    public static int ShowFile(int n)
    {
        Console.Clear();
        FileList filelist = Search();
        var loadedData = JsonConvert.DeserializeObject(File.ReadAllText(filelist.Paths[n - 1]));
        Console.WriteLine(loadedData);
        Console.WriteLine();
        Console.WriteLine("Натисніть любу клавішу щоб повернутися");
        Console.ReadKey();
        return 0;
    }

    static void ShowFilesToEdit()
    {
        Console.Clear();
        int q = 0;
        FileList filelist = Search();
        Console.WriteLine("0. Повернутися");
        foreach (string file in filelist.Names)
        {
            q++;
            Console.WriteLine($"{q}. {file}");
        }
        Console.WriteLine("Введіть номер документа");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            if (index < 0 || index > filelist.Names.Count)
            {
                Console.WriteLine("Неправилно введений номер");
                Console.ReadKey();
                ShowFilesToEdit();
            }
            else
            {
                if (index == 0)
                {
                    Main();
                }
                else
                {
                    EditFile(index);
                }
            }
        }
        else
        {
            Console.WriteLine("Неправилно введений номер");
            Console.ReadKey();
            ShowFilesToEdit();
        }
    }
    static int EditFile(int n)
    {
        Console.Clear();
        FileList filelist = new FileList();
        filelist = (Search());
        Console.WriteLine("Введіть нову тему:");
        string name = Console.ReadLine();
        Console.WriteLine("Введіть новий опис:");
        string text = Console.ReadLine();
        DateTime currentTime = DateTime.Now;
        string time = currentTime.ToString();
        var data = new
        {
            Тема = name,
            Опис = text,
            Час = time
        };
        string directoryPath1 = "data";
        string fileName = name + ".json";

        if (!Directory.Exists(directoryPath1))
        {
            Directory.CreateDirectory(directoryPath1);
        }

        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = Path.Combine(filelist.Paths[n - 1]);
        File.WriteAllText(filePath, jsonData);

        Console.WriteLine("Документ успішно редаговано");
        Console.ReadKey();
        Main();
        return 0;
    }

    static void Sorting()
    {
        Console.Clear();
        string filePath = Path.Combine("data", "sorting.txt");

        if (File.Exists(filePath))
        {
            string loadedData = File.ReadAllText(filePath);
            if (loadedData == "0")
            {
                Console.WriteLine("Сортування від А до Я");
            }
            else
            {
                Console.WriteLine("Сортування від Я до А");
            }
        }
        else
        {
            File.WriteAllText(filePath, "0");
        }
        Console.WriteLine();
        Console.WriteLine("Виберіть тип сортування:");
        Console.WriteLine("0. Повернутися назад");
        Console.WriteLine("1. Від А до Я");
        Console.WriteLine("2. Від Я до А");
        Console.WriteLine();
        char number = Console.ReadKey().KeyChar;
        Console.WriteLine();
        switch (number)
        {
            case '0':
                Main();
                break;
            case '1':
                File.WriteAllText(filePath, "0");
                Console.WriteLine();
                Console.WriteLine("Сортування змінено");
                Console.ReadKey();
                break;
            case '2':
                File.WriteAllText(filePath, "1");
                Console.WriteLine();
                Console.WriteLine("Сортування змінено");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Невірний вибір.");
                Console.ReadKey();
                break;
        }
        Main();
    }

    static int DeleteFile(int n)
    {
        FileList filelist = new FileList();
        filelist = (Search());
        string filePath = Path.Combine(filelist.Paths[n - 1]);
        File.Delete(filePath);
        return 0;
    }

    static void ListFilesToDelete()
    {
        Console.Clear();
        int q = 0;
        FileList filelist = Search();
        Console.WriteLine("0. Повернутися");
        foreach (string file in filelist.Names)
        {
            q++;
            Console.WriteLine($"{q}. {file}");
        }
        Console.WriteLine("Введіть номер документа");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            if (index < 0 || index > filelist.Names.Count)
            {
                Console.WriteLine("Неправилно введений номер");
                Console.ReadKey();
                ListFilesToDelete();
            }
            else
            {
                if (index == 0)
                {
                    Main();
                }
                else
                {
                    DeleteFile(index);
                }
            }
        }
        else
        {
            Console.WriteLine("Неправилно введений номер");
            Console.ReadKey();
            ListFilesToDelete();
        }
    }
}