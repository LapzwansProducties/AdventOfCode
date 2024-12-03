namespace AdventOfCode._2024;

public class Day_01
{
    [Test]
    [Puzzle(answer: 1530215)]
    public int PartOne(string input)
    {
        List<int> list1 = [];
        List<int> list2 = [];
        foreach (var line in input.Split('\n'))
        {
            string[] parts = line.Split("   ");
            list1.Add(int.Parse(parts[0]));
            list2.Add(int.Parse(parts[1]));
        }

        list1.Sort();
        list2.Sort();

        int sum = 0;
        for (int i = 0; i < list1.Count; i++)
        {
            sum += Math.Abs(list1[i] - list2[i]);
        }

        return sum;
    }

    [Test]
    [Puzzle(answer: 26800609)]
    public int PartTwo(string input)
    {
        System.Console.WriteLine(System.IO.Path.GetFullPath(@"..\..\..\"), input);
        List<int> list1 = [];
        List<int> list2 = [];
        foreach (var line in input.Split('\n'))
        {
            string[] parts = line.Split("   ");
            list1.Add(int.Parse(parts[0]));
            list2.Add(int.Parse(parts[1]));
        }

        int score = 0;
        list1.ForEach(number => score += number * list2.FindAll(number2 => number2 == number).Count);

        return score;
    }
}
