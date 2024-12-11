using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode._2024;

public class Day_11
{
    [TestCase("125 17", ExpectedResult = 55312)]
    [Puzzle(answer: 199946)]
    public long PartOne(string input) => Run(input, 25);

    [Test]
    [Puzzle(answer: 237994815702032)]
    public long PartTwo(string input) => Run(input, 75);

    static long Run(string input, int rounds) {
        IEnumerable<string> data = input.TrimEnd('\n').TrimEnd('\r').Split(' ');
        Dictionary<string, long> stonesGrouped = data.GroupBy(num => num).ToDictionary(group => group.Key, group => group.LongCount());
        return StonesRound(stonesGrouped, rounds).Sum(group => group.Value);
    }
    
    static Dictionary<string, long> StonesRound(Dictionary<string, long> stones, int roundsLeft) {
        var newStones = new Dictionary<string, long>();
        foreach (var pair in stones)
        {
            if (pair.Key == "0")
            {
                if (newStones.ContainsKey("1"))
                {
                    newStones["1"] += pair.Value;
                }
                else { 
                    newStones.Add("1", pair.Value);
                }
            }
            else if ((pair.Key.Length & 1) == 0)
            {
                string key1 = TrimStartZero(pair.Key[..(pair.Key.Length >> 1)]);
                string key2 = TrimStartZero(pair.Key[(pair.Key.Length >> 1)..]);
                if (newStones.ContainsKey(key1))
                {
                    newStones[key1] += pair.Value;
                }
                else
                {
                    newStones.Add(key1, pair.Value);
                }

                if (newStones.ContainsKey(key2))
                {
                    newStones[key2] += pair.Value;
                }
                else
                {
                    newStones.Add(key2, pair.Value);
                }
            }
            else
            {
                string key = (long.Parse(pair.Key) * 2024).ToString();
                if (newStones.ContainsKey(key))
                {
                    newStones[key] += 1;
                }
                else
                {
                    newStones.Add(key, pair.Value);
                }

            }
        }

        return roundsLeft > 1 ? StonesRound(newStones, roundsLeft - 1) : newStones;
    }

    static string TrimStartZero(string number)
    {
        string ret = number.TrimStart('0');
        return ret.Length > 0 ? ret : "0";
    }
}