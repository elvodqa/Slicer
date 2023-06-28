
//using var game = new Slicer.Game1();
//game.Run();

using Slicer.Core;


public class MyClass
{
    [Saveable]
    public string Name { get; set; }

    [Saveable]
    public int Age { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Age: {Age}";
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Saveable.SaveAs("test.txt");
    }
}