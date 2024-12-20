﻿namespace AdventOfCode._2024;

public class Day_09
{
    [TestCase("2333133121414131402", ExpectedResult = 1928)]
    [Puzzle(answer: 6241633730082)]
    public long PartOne(string input)
    {
        var diskLayout = CreateDisklayout(input);

        var emptyItemCount = diskLayout.Count(pair => pair[0] == -1);

        while (emptyItemCount > 0) {
            int[] lastItem = diskLayout[diskLayout.Count-1];
            diskLayout.RemoveAt(diskLayout.Count - 1);
            if (lastItem[0] == -1)
            {
                emptyItemCount--;
                continue;
            }

            while (lastItem[1] > 0) {
                int freespaceIndex = diskLayout.FindIndex(pair => pair[0] == -1);
                if (freespaceIndex == -1)
                    break;

                int freespaceSize = diskLayout[freespaceIndex][1];
                if (freespaceSize > lastItem[1])
                {
                    diskLayout.Insert(freespaceIndex++, lastItem);
                    diskLayout[freespaceIndex] = new int[] { -1, diskLayout[freespaceIndex][1] - lastItem[1] };
                    lastItem = new int[] { lastItem[0], 0 };
                }
                else if (freespaceSize == lastItem[1])
                {
                    diskLayout[freespaceIndex] = lastItem;
                    lastItem = new int[] { lastItem[0], 0 };
                    emptyItemCount--;
                }
                else {
                    diskLayout[freespaceIndex] = new int[] { lastItem[0], freespaceSize };
                    lastItem = new int[] { lastItem[0], lastItem[1] - freespaceSize };
                    emptyItemCount--;
                }
            }
            if (lastItem[1] > 0)
            {
                diskLayout.Add(lastItem);
            }
        }

        return CalculateChecksum(diskLayout);
    }

    [TestCase("2333133121414131402", ExpectedResult = 2858)]
    [Puzzle(answer: 6265268809555)]
    public long PartTwo(string input)
    {
        var diskLayout = CreateDisklayout(input);
        for (int i = diskLayout.Max(pair => pair[0]); i > 0; i--)
        {
            int itemIndex = diskLayout.FindIndex(pair => pair[0] == i)!;
            int[] item = diskLayout[itemIndex];

            int freespaceIndex = FindIndexFirstFreespace(diskLayout, item[1], itemIndex);
            if (freespaceIndex == -1)
                continue;

            int freespaceSize = diskLayout[freespaceIndex][1];
            if (freespaceSize > item[1])
            {
                diskLayout.Remove(item);
                diskLayout.Insert(itemIndex, new int[] { -1, item[1] });
                diskLayout.Insert(freespaceIndex++, item);
                diskLayout[freespaceIndex] = new int[] { -1, diskLayout[freespaceIndex][1] - item[1] };
            }
            else if (freespaceSize == item[1])
            {
                diskLayout.Remove(item);
                diskLayout.Insert(itemIndex, new int[] { -1, item[1] });
                diskLayout[freespaceIndex] = item;
            }
        }

        return CalculateChecksum(diskLayout);
    }


    public static List<int[]> CreateDisklayout(string input) {
        string diskMap = input.TrimEnd('\n').TrimEnd('\r');
        var diskLayout = new List<int[]>(); // id, length

        int id = 0;
        bool isFile = true;
        foreach (char c in diskMap)
        {
            diskLayout.Add(isFile ? new int[] { id++, c - '0' } : new int[] { -1, c - '0' });
            isFile = !isFile;
        }
        return diskLayout;
    }

    public static int FindIndexFirstFreespace(List<int[]> diskLayout, int length, int checkUntill)
    {
        for (int i = 0; i < checkUntill; i++)
        {
            if (diskLayout[i][0] == -1 && diskLayout[i][1] >= length)
                return i;
        }
        return -1;
    }

    public static long CalculateChecksum(List<int[]> diskLayout) {
        long checkSum = 0;
        long blockIndex = 0;
        foreach (var block in diskLayout)
        {
            if (block[0] == -1) {
                blockIndex += block[1];
                continue;
            }
            for (int i = 0; i < block[1]; i++)
            {
                checkSum += block[0] * blockIndex++;
            }
        }

        return checkSum;
    }
}
