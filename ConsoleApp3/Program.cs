using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 4;
        int K = 17;

        while (true)
        {
            Console.WriteLine("\n=== ЛАБОРАТОРНА РОБОТА 3 (N=4, K=17) ===");
            Console.WriteLine("1. Завдання 1: Сумарний розмір файлів у папці");
            Console.WriteLine("2. Завдання 2: Рекурсивний підрахунок (> 7) навпіл");
            Console.WriteLine("3. Завдання 3: Розмін 15 грн монетами 1, 2, 5");
            Console.WriteLine("4. Завдання 4: Трасування (стек викликів)");
            Console.WriteLine("5. Завдання 5: Порівняння з ітеративною версією");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть завдання (0-5): ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    RunTask1();
                    break;
                case 2:
                    RunTask2(N, K);
                    break;
                case 3:
                    RunTask3(K);
                    break;
                case 4:
                    RunTask4();
                    break;
                case 5:
                    RunTask5(N, K);
                    break;
                case 0:
                    return;
            }
        }
    }

    // --- ЗАВДАННЯ 1: Обхід каталогів ---
    static void RunTask1()
    {
        Console.WriteLine("\n--- Завдання 1: Сумарний розмір файлів ---");
        Console.Write("Введіть шлях до папки: ");
        string path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            Console.WriteLine("Помилка: Вказана папка не існує!");
            return;
        }

        long totalSize = GetDirectorySize(path);
        Console.WriteLine($"Сумарний розмір усіх файлів: {totalSize} байт");
    }

    public static long GetDirectorySize(string dirPath)
    {
        long size = 0;

        try
        {
            string[] files = Directory.GetFiles(dirPath);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                size += fi.Length;
            }

            string[] subDirs = Directory.GetDirectories(dirPath);
            foreach (string subDir in subDirs)
            {
                size += GetDirectorySize(subDir);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Пропускаємо захищені папки
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        return size;
    }

    // --- ЗАВДАННЯ 2: Розбиття навпіл ---
    static void RunTask2(int seed, int K)
    {
        Console.WriteLine("\n--- Завдання 2: Поділ навпіл ---");
        int threshold = K % 10; // 7
        int[] arr = GenerateArray(16, -20, 20, seed);

        Console.Write("Масив: ");
        PrintArray(arr);

        int maxDepth = 0;
        int count = SolveTask2(arr, 0, arr.Length - 1, 1, ref maxDepth, threshold);

        Console.WriteLine($"Кількість елементів, більших за {threshold}: {count}");
        Console.WriteLine($"Максимальна глибина рекурсії: {maxDepth}");
    }

    public static int SolveTask2(int[] arr, int lo, int hi, int currentDepth, ref int maxDepth, int threshold)
    {
        if (currentDepth > maxDepth) maxDepth = currentDepth;

        if (lo == hi)
        {
            return arr[lo] > threshold ? 1 : 0;
        }

        int mid = lo + (hi - lo) / 2;
        int leftCount = SolveTask2(arr, lo, mid, currentDepth + 1, ref maxDepth, threshold);
        int rightCount = SolveTask2(arr, mid + 1, hi, currentDepth + 1, ref maxDepth, threshold);

        return leftCount + rightCount;
    }

    // --- ЗАВДАННЯ 3: Перебір із поверненням (Backtracking) ---
    static void RunTask3(int K)
    {
        int S = (K % 10) + 8; // 15
        Console.WriteLine($"\n--- Завдання 3: Способи розміняти {S} грн (монети 1, 2, 5) ---");

        int[] coins = new int[] { 1, 2, 5 };
        List<int> currentCombination = new List<int>();
        int totalWays = 0;

        FindChange(S, coins, 0, currentCombination, ref totalWays);
        Console.WriteLine($"Усього варіантів: {totalWays}");
    }

    public static void FindChange(int remainingSum, int[] coins, int startCoinIndex, List<int> current, ref int count)
    {
        if (remainingSum == 0)
        {
            count++;
            Console.WriteLine($"{count}. {string.Join(" + ", current)}");
            return;
        }

        if (remainingSum < 0) return;

        for (int i = startCoinIndex; i < coins.Length; i++)
        {
            current.Add(coins[i]);
            FindChange(remainingSum - coins[i], coins, i, current, ref count);
            current.RemoveAt(current.Count - 1);
        }
    }

    // --- ЗАВДАННЯ 4: Трасування ---
    static void RunTask4()
    {
        Console.WriteLine("\n--- Завдання 4: Трасування (4 елементи) ---");
        int[] arr = new int[] { 10, -5, 8, 12 };
        int threshold = 7;

        int result = SolveTask2WithTrace(arr, 0, arr.Length - 1, 0, threshold);
        Console.WriteLine($"\nРезультат: {result}");
    }

    public static int SolveTask2WithTrace(int[] arr, int lo, int hi, int depth, int threshold)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}enter Count({lo}..{hi})");

        int result;
        if (lo == hi)
        {
            result = arr[lo] > threshold ? 1 : 0;
            Console.WriteLine($"{indent}  enter Count({lo}..{lo}) -> {result}");
            Console.WriteLine($"{indent}exit  Count({lo}..{hi}) -> {result}");
            return result;
        }

        int mid = lo + (hi - lo) / 2;
        int left = SolveTask2WithTrace(arr, lo, mid, depth + 1, threshold);
        int right = SolveTask2WithTrace(arr, mid + 1, hi, depth + 1, threshold);

        result = left + right;
        Console.WriteLine($"{indent}exit  Count({lo}..{hi}) -> {result}");
        return result;
    }

    // --- ЗАВДАННЯ 5: Ітеративна версія ---
    static void RunTask5(int seed, int K)
    {
        Console.WriteLine("\n--- Завдання 5: Порівняння з циклом ---");
        int threshold = K % 10;
        int[] arr = GenerateArray(16, -20, 20, seed);

        int maxDepth = 0;
        int recResult = SolveTask2(arr, 0, arr.Length - 1, 1, ref maxDepth, threshold);
        int iterResult = SolveTask2Iterative(arr, threshold);

        Console.WriteLine($"Рекурсивно: {recResult}");
        Console.WriteLine($"Циклом:     {iterResult}");
    }

    public static int SolveTask2Iterative(int[] arr, int threshold)
    {
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] > threshold) count++;
        return count;
    }

    static int[] GenerateArray(int size, int min, int max, int seed)
    {
        Random rnd = new Random(seed);
        int[] arr = new int[size];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = rnd.Next(min, max + 1);
        return arr;
    }

    static void PrintArray(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
            Console.Write(arr[i] + " ");
        Console.WriteLine();
    }
