using System.Linq;

namespace AdventOfCode._2024;

public class Day_04
{
    [TestCase("MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX", ExpectedResult = 18)]
    [Puzzle(answer: 2599)]
    public int PartOne(string input)
    {
        List<string> linesLeftToRight = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> linesTopToBottom = exploreTopDown(linesLeftToRight);
        List<string> linesLeftTopRightBottom = exploreLeftTopRightBottom(linesLeftToRight);
        List<string> linesLeftBottomRightTop = exploreLeftBottomRightTop(linesLeftToRight);

        IEnumerable<string> all = linesLeftToRight.Concat(linesTopToBottom).Concat(linesLeftTopRightBottom).Concat(linesLeftBottomRightTop);
        all.Sum(XmasCount);

        return all.Sum(XmasCount);
    }

    [TestCase(".M.S......\n..A..MSMS.\n.M.S.MAA..\n..A.ASMSM.\n.M.S.M....\n..........\nS.S.S.S.S.\n.A.A.A.A..\nM.M.M.M.M.\n..........", ExpectedResult = 9)]
    [Puzzle(answer: 1948)]
    public int PartTwo(string input)
    {
        int ret = 0;
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        int width = lines[0].Length;
        int height = lines.Count;
        for (int y = 0; y <= height - 3; y++)
        {
            for (int x = 0; x <= width - 3; x++)
            {
                string dia1 = String.Empty + lines[y][x] + lines[y + 1][x + 1] + lines[y + 2][x + 2];
                string dia2 = String.Empty + lines[y+2][x] + lines[y + 1][x + 1] + lines[y][x + 2];

                if ((dia1 == "MAS" || dia1 == "SAM") && (dia2 == "MAS" || dia2 == "SAM"))
                    ret++;
            }
        }
        return ret;
    }

    public List<string> exploreTopDown(in List<string> lines) {
        List<string> ret = [];
        int width = lines[0].Length;
        int height = lines.Count;
        for (int x = 0; x < width; ++x)
        {
            string line = String.Empty;
            for (int y = 0; y < height; ++y)
            {
                line += lines[y][x];
            }
            ret.Add(line);
        }
        return ret;
    }

    public List<string> exploreLeftTopRightBottom(in List<string> lines)
    {
        List<string> ret = [];
        int width = lines[0].Length;
        int height = lines.Count;
        for (int startY = 0; startY < height; ++startY)
        {
            string line = String.Empty;
            int x = 0, y = startY;
            while(y < height)
            {
                line += lines[y][x];
                ++x;
                ++y;
            }
            ret.Add(line);
        }
        for (int startX = 1; startX < width; ++startX)
        {
            string line = String.Empty;
            int x = startX, y = 0;
            while (x < width)
            {
                line += lines[y][x];
                ++x;
                ++y;
            }
            ret.Add(line);
        }
        return ret;
    }

    public List<string> exploreLeftBottomRightTop(in List<string> lines)
    {
        List<string> ret = [];
        int width = lines[0].Length;
        int height = lines.Count;
        for (int startY = 0; startY < height; ++startY)
        {
            string line = String.Empty;
            int x = 0, y = startY;
            while (y >= 0)
            {
                line += lines[y][x];
                ++x;
                --y;
            }
            ret.Add(line);
        }
        for (int startX = 1; startX < width; ++startX)
        {
            string line = String.Empty;
            int x = startX, y = lines.Count-1;
            while (x < width)
            {
                line += lines[y][x];
                ++x;
                --y;
            }
            ret.Add(line);
        }
        return ret;
    }

    [Test]
    [TestCase("MAS", ExpectedResult = 0)]
    [TestCase("XMAS", ExpectedResult = 1)]
    [TestCase("XMASAMX", ExpectedResult = 2)]
    [TestCase("XXMASAMX", ExpectedResult = 2)]
    public int XmasCount(string data)
    {
        int ret = 0;
        if (data.Length < 4)
            return ret;

        for (int i = 0; i <= data.Length - 4; ++i)
        {
            var substr = data.Substring(i, 4);
            if (substr == "XMAS" || substr == "SAMX")
                ++ret;
        }
        return ret;
    }
}