using System;
using System.Collections.Generic;
using System.Linq;

// Інтерфейс для введення даних
interface IInputProvider
{
    string GetInput(string prompt);
}

// Консольний провайдер введення
class ConsoleInputProvider : IInputProvider
{
    public string GetInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }
}

// Модель працівника
class Employee
{
    public string Name { get; }
    public string Position { get; set; }

    public Employee(string name)
    {
        Name = name;
    }
}

// Сервіс для вирішення логічної задачі
class LogicSolver
{
    private readonly List<Employee> employees;
    private readonly List<string> positions = new List<string> { "бухгалтер", "касир", "начальник відділу" };

    public LogicSolver()
    {
        employees = new List<Employee>
        {
            new Employee("Лев"),
            new Employee("Михайло"),
            new Employee("Роман")
        };
    }

    public void Solve()
    {
        Console.WriteLine("Починаємо вирішення задачі...");
        Console.WriteLine("Крок 1: У нас є три людини (Лев, Михайло, Роман) і три посади (бухгалтер, касир, начальник відділу).");
        Console.WriteLine("Крок 2: Перебираємо всі можливі комбінації посад і перевіряємо умови.");

        int attempt = 1;
        foreach (var permutation in GetPermutations(positions))
        {
            Console.WriteLine($"\nСпроба #{attempt}:");
            AssignPositions(permutation);
            DisplayCurrentAssignment();
            if (CheckConditions())
            {
                Console.WriteLine("Крок 3: Усі умови виконані. Знайдено правильне рішення!");
                Console.WriteLine("\nОстаточний результат:");
                DisplayResult();
                return;
            }
            attempt++;
        }
        Console.WriteLine("Рішення не знайдено.");
    }

    private void AssignPositions(IList<string> permutation)
    {
        for (int i = 0; i < employees.Count; i++)
        {
            employees[i].Position = permutation[i];
        }
    }

    private void DisplayCurrentAssignment()
    {
        Console.WriteLine("Поточне призначення:");
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.Name} – {employee.Position}");
        }
    }

    private bool CheckConditions()
    {
        var roman = employees.First(e => e.Name == "Роман").Position;
        var mykhailo = employees.First(e => e.Name == "Михайло").Position;
        var lev = employees.First(e => e.Name == "Лев").Position;

        // Умова 1
        Console.WriteLine("Перевірка умови 1: Якщо Роман – касир, то Михайло – начальник відділу.");
        if (roman == "касир" && mykhailo != "начальник відділу")
        {
            Console.WriteLine("Умова 1 не виконана.");
            return false;
        }
        Console.WriteLine("Умова 1 виконана або не застосовується.");

        // Умова 2
        Console.WriteLine("Перевірка умови 2: Якщо Роман – начальник відділу, то Михайло – бухгалтер.");
        if (roman == "начальник відділу" && mykhailo != "бухгалтер")
        {
            Console.WriteLine("Умова 2 не виконана.");
            return false;
        }
        Console.WriteLine("Умова 2 виконана або не застосовується.");

        // Умова 3
        Console.WriteLine("Перевірка умови 3: Якщо Михайло – не касир, то Лев – не начальник відділу.");
        if (mykhailo != "касир" && lev == "начальник відділу")
        {
            Console.WriteLine("Умова 3 не виконана.");
            return false;
        }
        Console.WriteLine("Умова 3 виконана або не застосовується.");

        // Умова 4
        Console.WriteLine("Перевірка умови 4: Якщо Лев – бухгалтер, то Роман – начальник відділу.");
        if (lev == "бухгалтер" && roman != "начальник відділу")
        {
            Console.WriteLine("Умова 4 не виконана.");
            return false;
        }
        Console.WriteLine("Умова 4 виконана або не застосовується.");

        return true;
    }

    private void DisplayResult()
    {
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.Name} – {employee.Position}");
        }
    }

    private IEnumerable<IList<string>> GetPermutations(IList<string> list)
    {
        if (list.Count == 1)
        {
            yield return new List<string> { list[0] };
            yield break;
        }

        foreach (var item in list)
        {
            var subList = list.Where(x => x != item).ToList();
            foreach (var subPerm in GetPermutations(subList))
            {
                var permutation = new List<string> { item };
                permutation.AddRange(subPerm);
                yield return permutation;
            }
        }
    }
}

// Головний клас програми
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Для коректного відображення української

        IInputProvider inputProvider = new ConsoleInputProvider();
        string response = inputProvider.GetInput("Розв’язати задачу? (+/ні)");

        if (response == "+")
        {
            LogicSolver solver = new LogicSolver();
            solver.Solve();
        }
        else
        {
            Console.WriteLine("Програма завершена.");
        }
    }
}