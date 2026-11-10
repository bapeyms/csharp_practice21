namespace csharp_practice21;

// Питання 1: Пояснити, що відбувається з об'єктами після кожної ітерації.
// Відповідь:
// 1) створюється новий об'єкт у керуємій купі
// 2) викликається конструктор, що створює масив byte[] розміром 1мб
// 3) поле _buffer збегірає посилання на цей масив
// 4) виконуєтьс метод Process()
// 5) цикл завершує свою роботу через 1000 ітерацій
// Після кожної ітерації створенні об'єкти та масиви їхні стають кандидатами на видалення GC,
// але фактичне звільнення пам'яті відбувається не гарантовано одразу 
 
// Питання 2: Чому пам'ять не обов'язково звільняється одразу після завершення ітерації?
// Відповідь: Тому що GC запускається тоді, коли виникає потреба в очищенні пам'яті
// Якби збирач запускався б після кожної ітерації, то це б створювало додаткове навантаження та сповільнювало б програму
  
// Питання 3: Які покоління GC існують у .NET?
// Відповідь: Нульове, перше, друге

// Питання 4: До якого покоління потраплятимуть створені об'єкти?
// Відповідь: До нульового

public class DataProcessor
{
    private byte[] _buffer;
 
    public DataProcessor(int size)
    {
        _buffer = new byte[size];
    }
 
    public void Process()
    {
        // Імітація обробки даних
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 1
        // for (int i = 0; i < 1000; i++)
        // {
        //     var processor = new DataProcessor(1024 * 1024);
        //     processor.Process();
        // }
        //
        // Console.WriteLine($"Memory: {GC.GetTotalMemory(false)}");
        
        // 2
        DataProcessor pr1 = new DataProcessor(1024);
        DataProcessor pr2 = new DataProcessor(1024);
        DataProcessor pr3 = new DataProcessor(1024 *  1024);
        
        // показує покоління об'єкта
        Console.WriteLine("1) Generations: ");
        Console.WriteLine($"- Object 1: {GC.GetGeneration(pr1)}");
        Console.WriteLine($"- Object 2: {GC.GetGeneration(pr2)}");
        Console.WriteLine($"- Object 3: {GC.GetGeneration(pr3)}");
        
        // повертає приблизний обсяг зайнятої пам'яті в купі, що керується
        Console.WriteLine($"2) Get Total Memory Before GC: {GC.GetTotalMemory(false)} bytes");

        // показує к-сть виконаних очищеннь конкретного покоління
        Console.WriteLine("3) Collection Count: ");
        for (int i = 0; i <= 2; i++)
        {
            Console.WriteLine($"Generation {i} collections: {GC.CollectionCount(i)} ");
        }

        pr1 = null;
        pr2 = null;
        pr3 = null;
        // примусово запускає збір сміття
        GC.Collect();
        Console.WriteLine($"4) Get Total Memory After GC: {GC.GetTotalMemory(false)} bytes");
        
        // 3
        const int objectCount = 10_000;
        const int objectSize = 100 * 1024;
        
        int gen0Before = GC.CollectionCount(0);
        int gen1Before = GC.CollectionCount(1);
        int gen2Before = GC.CollectionCount(2);
        
        DataProcessor[] pr = new DataProcessor[objectCount];
        for (int i = 0; i < objectCount; i++)
            pr[i] = new DataProcessor(objectSize);
        
        long memoryBefore = GC.GetTotalMemory(false);
        Console.WriteLine("- Before GC: ");
        Console.WriteLine($"Object created: {objectCount}");
        Console.WriteLine($"Memory: {memoryBefore} bytes");
        
        Console.WriteLine("Collection Count: ");
        for (int i = 0; i <= 2; i++)
            Console.WriteLine($"Generation {i} collections: {GC.CollectionCount(i)} ");
        pr = null;
        GC.Collect();
        
        long memoryAfter = GC.GetTotalMemory(false);
        Console.WriteLine("- After GC: ");
        Console.WriteLine($"Memory: {memoryAfter} bytes");
        Console.WriteLine("Collection Count: ");
        for (int i = 0; i <= 2; i++)
            Console.WriteLine($"Generation {i} collections: {GC.CollectionCount(i)} ");
        
        Console.WriteLine($"Generation 0: {GC.CollectionCount(0) - gen0Before }");
        Console.WriteLine($"Generation 1: {GC.CollectionCount(1) - gen1Before }");
        Console.WriteLine($"Generation 2: {GC.CollectionCount(2) - gen2Before }");
        Console.WriteLine($"Freed memory: {(memoryBefore - memoryAfter)} bytes");
        
    }
}