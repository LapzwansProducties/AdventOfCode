namespace AdventOfCode._2024;

public class Day_07
{
    [TestCase("190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20", ExpectedResult = 3749)]
    [Puzzle(answer: 303766880536)]
    public long PartOne(string input)
    {
        List<Func<long, long, long>> funcs = [
            (long a, long b) => a + b,
            (long a, long b) => a * b,
        ];

        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        return lines.Sum(str => isCorrect(str, funcs));
    }

    [TestCase("190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20", ExpectedResult = 11387)]
    [Puzzle(answer: 337041851384440)]
    public long PartTwo(string input)
    {
        List<Func<long, long, long>> funcs = [
            (long a, long b) => a + b,
            (long a, long b) => a * b,
            (long a, long b) => long.Parse(b.ToString() + a.ToString()),
        ];

        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        return lines.Sum(str => isCorrect(str, funcs));
    }

    public long isCorrect(string input, List<Func<long, long, long>> funcs) {
        long ret = 0;
        var split = input.Split(':', StringSplitOptions.RemoveEmptyEntries);
        long ans = long.Parse(split[0]);
        List<long> nums = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        nums.Reverse();
        return calculatePermutations(nums, funcs).Contains(ans) ? ans : 0;
    }

    public List<long> calculatePermutations(List<long> nums, List<Func<long, long, long>> funcs) {
        List<long> ret = [];

        if (nums.Count > 2)
        {
            List<long> returnedNumbers = calculatePermutations(nums[1..], funcs);
            foreach (var returnedNumber in returnedNumbers)
            {
                foreach (var func in funcs)
                {
                    ret.Add(func(nums[0], returnedNumber));
                }
            }
            return ret;
        }
        else {
            foreach (var func in funcs)
            {
                ret.Add(func(nums[0], nums[1]));
            }
            return ret;
        }
    }
}
