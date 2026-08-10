namespace EYRectangles;

// Prints a report for the scenarios drawn in the exercise appendices, plus a few edge cases.
public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Rectangles - relationship report");

        Section("Appendix 1 - Intersection");
        Report("Intersection (one rectangle passes through the other)", new Rectangle(0, 0, 10, 6), new Rectangle(3, -2, 4, 10));
        Report("Intersection (the corners overlap)", new Rectangle(0, 0, 6, 4), new Rectangle(4, 2, 6, 4));
        Report("No intersection", new Rectangle(0, 0, 4, 4), new Rectangle(10, 10, 3, 3));

        Section("Appendix 2 - Containment");
        Report("Containment", new Rectangle(0, 0, 10, 10), new Rectangle(2, 2, 4, 4));
        Report("No containment", new Rectangle(0, 0, 10, 8), new Rectangle(14, 3, 4, 2));
        Report("Intersection - no containment", new Rectangle(0, 0, 6, 6), new Rectangle(4, 2, 5, 2));

        Section("Appendix 3 - Adjacency");
        Report("Adjacent (proper)", new Rectangle(0, 0, 4, 4), new Rectangle(4, 0, 4, 4));
        Report("Adjacent (sub-line)", new Rectangle(0, 0, 4, 6), new Rectangle(4, 1, 3, 3));
        Report("Adjacent (partial)", new Rectangle(0, 0, 4, 4), new Rectangle(4, 2, 4, 4));
        Report("Not adjacent", new Rectangle(0, 0, 4, 4), new Rectangle(6, 0, 4, 4));

        Section("Edge cases");
        Report("Corner contact only (touching, but not adjacent)", new Rectangle(0, 0, 4, 4), new Rectangle(4, 4, 4, 4));
        Report("Identical rectangles (each contains the other)", new Rectangle(0, 0, 4, 4), new Rectangle(0, 0, 4, 4));
        Report("Negative coordinates (proper adjacency)", new Rectangle(-8, -6, 4, 4), new Rectangle(-4, -6, 4, 4));
    }

    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(title);
        Console.WriteLine(new string('=', title.Length));
    }

    // Runs all three algorithms on one pair of rectangles and prints the outcome.
    private static void Report(string title, Rectangle first, Rectangle second)
    {
        var firstContainsSecond = RectangleAnalyzer.Contains(first, second);
        var secondContainsFirst = RectangleAnalyzer.Contains(second, first);
        var adjacency = RectangleAnalyzer.GetAdjacency(first, second);
        var points = RectangleAnalyzer.GetIntersectionPoints(first, second);

        var containment = (firstContainsSecond, secondContainsFirst) switch
        {
          (true, true)  => "each contains the other - rectangles are the same on same coordinates",
          (true, false) => "first contains second",
          (false, true) => "second contains first",
          _             => "none"
        };

        var intersection = points.Count == 0
            ? "none"
            : $"{points.Count} point{(points.Count == 1 ? "" : "s")} -> {string.Join(", ", points)}";

        Console.WriteLine();
        Console.WriteLine($"  {title}");
        Console.WriteLine($"    first        : {first}");
        Console.WriteLine($"    second       : {second}");
        Console.WriteLine($"    containment  : {containment}");
        Console.WriteLine($"    adjacency    : {adjacency}");
        Console.WriteLine($"    intersection : {intersection}");
    }
}
