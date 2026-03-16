// ==========================================
// CORE C# & .NET CHEATSHEET
// ==========================================

// These 'using' statements bring in namespaces from the .NET base class libraries
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Console.WriteLine("--- 1. VARIABLES & BASIC DATA TYPES ---");
// C# is strongly typed. You must declare the type, or use 'var' to let the compiler infer it.

int age = 25;                  // Integer (Whole number)
double pi = 3.14159;           // Double (Decimal number)
string greeting = "Hello!";    // String (Text - must use double quotes)
char grade = 'A';              // Char (Single character - must use single quotes)
bool isLearning = true;        // Boolean (True or false)
var dynamicType = "Inferred";  // The compiler knows this is a string based on the value

Console.WriteLine($"Age: {age}, Greeting: {greeting}"); // String interpolation uses '$'

// --- 2. BASIC OPERATORS ---
int addition = 5 + 3;          // 8
int modulo = 17 % 3;           // 2 (Remainder)
bool isEqual = (5 == 5);       // true
bool logicCheck = (5 > 3) && (10 < 20); // true (Uses && for 'and', || for 'or', ! for 'not')


Console.WriteLine("\n--- 3. DATA STRUCTURES ---");

// ARRAYS: Fixed size, must specify the type.
string[] fruitsArray = { "apple", "banana", "cherry" };
fruitsArray[1] = "blueberry"; // Modifying

// LISTS: Dynamic size, similar to Python's standard lists.
List<string> fruitsList = new List<string> { "apple", "banana" };
fruitsList.Add("orange"); // Adds to the end

// DICTIONARIES: Key-Value pairs (Like Python dicts).
Dictionary<string, int> studentAges = new Dictionary<string, int>();
studentAges["John"] = 20;
studentAges.Add("Alice", 22);

Console.WriteLine($"Alice is {studentAges["Alice"]} years old.");


Console.WriteLine("\n--- 4. CONTROL FLOW ---");
int temperature = 25;

if (temperature > 30)
{
    Console.WriteLine("It's hot outside.");
}
else if (temperature > 20)
{
    Console.WriteLine("It's a beautiful day."); // This executes
}
else
{
    Console.WriteLine("It's getting cold.");
}


Console.WriteLine("\n--- 5. LOOPS ---");

// FOR LOOPS: Best when you know exactly how many times to loop.
for (int i = 1; i < 4; i++)
{
    Console.WriteLine($"For loop: {i}");
}

// FOREACH LOOPS: The equivalent of Python's 'for item in list'.
foreach (string fruit in fruitsList)
{
    Console.WriteLine($"Foreach loop: {fruit}");
}

// WHILE LOOPS: Run as long as the condition is true.
int counter = 0;
while (counter < 3)
{
    Console.WriteLine($"While loop counter: {counter}");
    counter++; // Same as counter += 1
}


Console.WriteLine("\n--- 6. METHODS (Functions) ---");
// Methods are usually defined inside a class, but can be local functions here.

string GreetUser(string name, string defaultGreeting = "Welcome")
{
    return $"{defaultGreeting}, {name}!";
}

Console.WriteLine(GreetUser("Sarah"));


Console.WriteLine("\n--- 7. EXCEPTION HANDLING ---");
try
{
    int result = 10 / 0;
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Error Caught: {ex.Message}");
}
finally
{
    Console.WriteLine("This 'finally' block always runs.");
}


// ... [All your previous code: Variables, Loops, Methods, etc.] ...

Console.WriteLine("\n--- 8. CLASSES & OBJECTS (OOP) ---");

// FIX 1: We simplified the 'new' expression (IDE0090)
Dog myDog = new("Buddy", 3);
myDog.Bark();

Console.WriteLine("\n--- 9. LINQ (Language Integrated Query) ---");
int[] numbers = { 1, 2, 3, 4, 5 };
var evenSquares = numbers.Where(n => n % 2 == 0).Select(n => n * n).ToList();
Console.WriteLine($"Even Squares: {string.Join(", ", evenSquares)}");

Console.WriteLine("\n--- 10. FILE I/O ---");
string filePath = "test_file.txt";
File.WriteAllText(filePath, "Hello from .NET!\nThis is C#.");
string content = File.ReadAllText(filePath);
Console.WriteLine("File Content:\n" + content);


// ====================================================================
// FIX 2: ALL CLASS DEFINITIONS MUST GO AT THE VERY BOTTOM OF THE FILE (CS8803)
// ====================================================================
class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Dog(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Bark()
    {
        Console.WriteLine($"{Name} says Woof!");
    }
}