using System;
using System.Collections.Generic;

namespace AdventOfCode._2024;

public static class Day_05
{
    [TestCase("47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47", ExpectedResult = 143)]
    [Puzzle(answer: 6612)]
    public static int PartOne(string input)
    {
        List<Tuple<int, int>> PageOrderRules;
        List<List<int>> updates;
        setData(input, out PageOrderRules, out updates);

        return GetRows(PageOrderRules, updates, true)
            .Select(row => row[row.Count >> 1])
            .Sum();
    }

    [TestCase("47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47", ExpectedResult = 123)]
    [Puzzle(answer: 4944)]
    public static int PartTwo(string input)
    {
        List<Tuple<int, int>> PageOrderRules;
        List<List<int>> updates;
        setData(input, out PageOrderRules, out updates);

        return GetRows(PageOrderRules, updates, false)
            .Select(row => CorrectRow(PageOrderRules, row)[row.Count >> 1])
            .Sum();
    }

    public static void setData(string input, out List<Tuple<int, int>> PageOrderRules, out List<List<int>> updates)
    {
        string[] parts = input.Split(["\n\n", "\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);
        PageOrderRules = parts[0].Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(rule =>
            {
                var nums = rule.Split('|', StringSplitOptions.RemoveEmptyEntries);
                return new Tuple<int, int>(int.Parse(nums[0]), int.Parse(nums[1]));
            }).ToList();
        updates = parts[1].Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(str =>
            {
                var split = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
                return split.Select(substr => int.Parse(substr)).ToList();
            }).ToList();
    }

    public static List<List<int>> GetRows(List<Tuple<int, int>> PageOrderRules, List<List<int>> updates, bool isCorrect)
    => updates.Where(row => RowIsCorrect(PageOrderRules, row) == isCorrect).ToList();

    public static bool RowIsCorrect(List<Tuple<int, int>> PageOrderRules, List<int> row)
    {
        for (int index = 0; index < row.Count; index++)
        {
            var rules = PageOrderRules.Where(rule => rule.Item1 == row[index]);
            foreach (var rule in rules)
            {
                int ruleFuturePageIndex = row.IndexOf(rule.Item2);
                if (ruleFuturePageIndex >= 0 && ruleFuturePageIndex < index)
                    return false;
            }
        }

        return true;
    }

    public static List<int> CorrectRow(List<Tuple<int, int>> PageOrderRules, List<int> row)
    {
        for (int index = 0; index < row.Count; index++)
        {
            var minimumForbiddenIndexForNumberArr = PageOrderRules.Where(rule => rule.Item1 == row[index]).Select(pair => row.IndexOf(pair.Item2)).Where(num => num >= 0);
            if (!minimumForbiddenIndexForNumberArr.Any())
                continue;
            var minimumForbiddenIndexForNumber = minimumForbiddenIndexForNumberArr.Min();
            if (index >= minimumForbiddenIndexForNumber)
            {
                int num = row[index];
                row.Remove(num);
                row.Insert(minimumForbiddenIndexForNumber, num);
            }
        }

        return row;
    }
}