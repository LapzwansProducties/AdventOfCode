namespace AdventOfCode._2024;

public class Day_03
{
    [TestCase("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", ExpectedResult = 161)]
    [Puzzle(answer: 170778545)]
    public int PartOne(string input)
    {
        return FindString(input, ["mul("]).Sum(pair => EvalFromInput(input, pair.Key));
    }

    [TestCase("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", ExpectedResult = 48)]
    [Puzzle(answer: 82868252)]
    public int PartTwo(string input)
    {
        int total = 0;
        bool eval = true;
        foreach (var pair in FindString(input, ["mul(", "do()", "don't()"]))
        {
            switch (pair.Value)
            {
                case "do()":
                    eval = true;
                    break;
                case "don't()":
                    eval = false;
                    break;
                default:
                    if (eval)
                        total += EvalFromInput(input, pair.Key);
                    break;
            }
        }
        return total;
    }

    public static IEnumerable<KeyValuePair<int, string>> FindString(in string input, in string[] targets)
    {
        var ret = new List<KeyValuePair<int, string>>();
        foreach (var target in targets) {
            int startIindex = input.IndexOf(target[0]);
            while (startIindex > 0)
            {
                if (input.Substring(startIindex, target.Length) == target)
                    ret.Add(new KeyValuePair<int, string>(startIindex, target));
                startIindex = input.IndexOf(target[0], startIindex + 1);
            }
        }
        return ret.OrderBy(pair => pair.Key);
    }

    public static int EvalFromInput(in string input, int startIindex)
    {
        startIindex += 4;
        int endIndex = input.IndexOf(')', startIindex);
        string slice = input[startIindex..endIndex];
        if (slice.IndexOf(',') <= 0 || slice.EndsWith(',') || slice.Count(char.IsAsciiDigit) != slice.Length - 1)
            return 0;

        string[] split = slice.Split(',');
        int a = int.Parse(split[0]);
        int b = int.Parse(split[1]);
        return a * b;
    }
}