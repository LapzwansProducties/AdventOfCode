namespace AdventOfCode._2024;

public class Day_09
{
    [TestCase("2333133121414131402", ExpectedResult = 1928)]
    [Puzzle(answer: 6241633730082)]
    public long PartOne(string input)
    {
        var diskLayout = CreateDisklayout(input);

        // Compression
        while (diskLayout.Any(pair => pair.Item1 == -1)) {
            Tuple<int, int> lastItem = diskLayout[diskLayout.Count-1];
            diskLayout.RemoveAt(diskLayout.Count - 1);
            // remove ending empty space
            if (lastItem.Item1 == -1)
                continue;

            while (lastItem.Item2 > 0) {
                int freespaceIndex = FindIndexFirstFreespace(diskLayout);
                if (freespaceIndex == -1)
                    break;

                // if file.length < freespace.length -> prepend and resize freezpace
                // if file.length == freespace.length -> freespace.id = file.id
                // if file.length > freespace.length -> freespace.id = file.id, continue above two steps for next freespace
                int freespaceSize = diskLayout[freespaceIndex].Item2;
                if (freespaceSize > lastItem.Item2)
                {
                    diskLayout.Insert(freespaceIndex++, lastItem);
                    diskLayout[freespaceIndex] = new Tuple<int, int>(-1, diskLayout[freespaceIndex].Item2 - lastItem.Item2);
                    lastItem = new Tuple<int, int>(lastItem.Item1, 0);
                }
                else if (freespaceSize == lastItem.Item2)
                {
                    diskLayout[freespaceIndex] = lastItem;
                    lastItem = new Tuple<int, int>(lastItem.Item1, 0);
                }
                else {
                    diskLayout[freespaceIndex] = new Tuple<int, int>(lastItem.Item1, freespaceSize);
                    lastItem = new Tuple<int, int>(lastItem.Item1, lastItem.Item2 - freespaceSize);
                }
            }
            if (lastItem.Item2 > 0)
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
        // Compression
        for (int i = diskLayout.Max(pair => pair.Item1); i > 0; i--)
        {
            Tuple<int, int> lastItem = diskLayout.Find(pair => pair.Item1 == i)!;
            int lastItemIndex = diskLayout.FindIndex(pair => pair.Item1 == i)!;

            int freespaceIndex = FindIndexFirstFreespace(diskLayout, lastItem.Item2, i);
            if (freespaceIndex == -1)
                continue;

            // if file.length < freespace.length -> prepend and resize freezpace
            // if file.length == freespace.length -> freespace.id = file.id
            int freespaceSize = diskLayout[freespaceIndex].Item2;
            if (freespaceSize > lastItem.Item2)
            {
                diskLayout.Remove(lastItem);
                diskLayout.Insert(lastItemIndex, new Tuple<int, int>(-1, lastItem.Item2));
                diskLayout.Insert(freespaceIndex++, lastItem);
                diskLayout[freespaceIndex] = new Tuple<int, int>(-1, diskLayout[freespaceIndex].Item2 - lastItem.Item2);
            }
            else if (freespaceSize == lastItem.Item2)
            {
                diskLayout.Remove(lastItem);
                diskLayout.Insert(lastItemIndex, new Tuple<int, int>(-1, lastItem.Item2));
                diskLayout[freespaceIndex] = lastItem;
            }

            diskLayout = CombineEmpySpace(diskLayout);
        }

        return CalculateChecksum(diskLayout);
    }


    public static List<Tuple<int, int>> CreateDisklayout(string input) {
        string diskMap = input.TrimEnd('\n').TrimEnd('\r');
        var diskLayout = new List<Tuple<int, int>>(); // id, length

        // Split disklayout
        int id = 0;
        bool isFile = true;
        foreach (char c in diskMap)
        {
            diskLayout.Add(isFile ? new Tuple<int, int>(id++, c - '0') : new Tuple<int, int>(-1, c - '0'));
            isFile = !isFile;
        }
        return diskLayout;
    }

    public static int FindIndexFirstFreespace(List<Tuple<int, int>> diskLayout) {
        for (int i = 0; i < diskLayout.Count; i++)
        {
            if (diskLayout[i].Item1 == -1)
                return i;
        }
        return -1;
    }

    public static int FindIndexFirstFreespace(List<Tuple<int, int>> diskLayout, int length, int currentId)
    {
        for (int i = 0; i < diskLayout.Count; i++)
        {
            if (diskLayout[i].Item1 == currentId)
                break;
            if (diskLayout[i].Item1 == -1 && diskLayout[i].Item2 >= length)
                return i;
        }
        return -1;
    }

    public static long CalculateChecksum(List<Tuple<int, int>> diskLayout) {
        long checkSum = 0;
        long blockIndex = 0;
        foreach (var block in diskLayout)
        {
            if (block.Item1 == -1) {
                blockIndex += block.Item2;
                continue;
            }
            for (int i = 0; i < block.Item2; i++)
            {
                checkSum += block.Item1 * blockIndex++;
            }
        }

        return checkSum;
    }

    public static List<Tuple<int, int>> CombineEmpySpace(List<Tuple<int, int>> diskLayout)
    {
        int maxIndex = diskLayout.Count - 1;
        for (int i = 0; i < maxIndex; i++)
        {
            if (diskLayout[i].Item1 == -1 && diskLayout[i + 1].Item1 == -1)
            {
                diskLayout[i] = new Tuple<int, int>(-1, diskLayout[i].Item2 + diskLayout[i + 1].Item2);
                diskLayout.RemoveAt(i + 1);
                --maxIndex;
            }

        }

        return diskLayout;
    }
}
