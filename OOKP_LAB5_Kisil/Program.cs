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
        foreach (var permutation in GetPermutations(positions))
        {
            AssignPositions(permutation);
            if (IsValidAssignment())
            {
                DisplayResult();
                return;
            }
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

    private bool IsValidAssignment()
    {
        var roman = employees.First(e => e.Name == "Роман").Position;
        var mykhailo = employees.First(e => e.Name == "Михайло").Position;
        var lev = employees.First(e => e.Name == "Лев").Position;

        // Умова 1: Якщо Роман – касир, то Михайло – начальник відділу
        if (roman == "касир" && mykhailo != "начальник відділу") return false;

        // Умова 2: Якщо Роман – начальник відділу, то Михайло – бухгалтер
        if (roman == "начальник відділу" && mykhailo != "бухгалтер") return false;

        // Умова 3: Якщо Михайло – не касир, то Лев – не начальник відділу
        if (mykhailo != "касир" && lev == "начальник відділу") return false;

        // Умова 4: Якщо Лев – бухгалтер, то Роман – начальник відділу
        if (lev == "бухгалтер" && roman != "начальник відділу") return false;

        return true;
    }

    private void DisplayResult()
    {
        Console.WriteLine("Результати:");
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