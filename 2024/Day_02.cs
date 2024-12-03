namespace AdventOfCode._2024;

public class Day_02
{
    [TestCase("7 6 4 2 1;1 2 7 8 9;9 7 6 2 1;1 3 2 4 5;8 6 4 4 1;1 3 6 7 9", ExpectedResult = 2)]
    [Puzzle(answer: 639)]
    public int PartOne(string input)
    {
        var lines = input.Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var ns = lines.Select(l => l.Split(' ').Select(s => int.Parse(s)).ToList()).ToList();
        return ns.Count(report => IsSafe(report));
    }

    [TestCase("7 6 4 2 1;1 2 7 8 9;9 7 6 2 1;1 3 2 4 5;8 6 4 4 1;1 3 6 7 9", ExpectedResult = 4)]
    [Puzzle(answer: 674)]
    public int PartTwo(string input)
    {
        var lines = input.Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var ns = lines.Select(l => l.Split(' ').Select(s => int.Parse(s)).ToList()).ToList();
        return ns.Count(report => IsSafeWithDampener(report));
    }

    static bool IsSafeWithDampener(in List<int> report)
    {
        if (IsSafe(report))
            return true;
        for (int i = 0; i < report.Count; i++)
        {
            int[] holder = new int[report.Count];
            report.CopyTo(holder, 0);
            List<int> dampener = holder.ToList();
            dampener.RemoveAt(i);
            if (IsSafe(dampener))
                return true;
        }
        return false;
    }

    static bool IsSafe(in List<int> report)
    {
        bool isSortedAscending = report.OrderBy(x => x).SequenceEqual(report);
        bool isSortedDescending = report.OrderByDescending(x => x).SequenceEqual(report);
        bool containsNoDoubles = report.DistinctBy(x => x).Count() == report.Count;

        if (!((isSortedAscending || isSortedDescending) && containsNoDoubles))
            return false;

        int i = 0;
        for (; i < report.Count - 1; ++i)
            if (Math.Abs(report[i] - report[i + 1]) > 3)
                return false;
        return true;
    }
}