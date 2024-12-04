using System.Linq;

namespace AdventOfCode._2024;

public class Day_04
{
    [TestCase("MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX", ExpectedResult = 18)]
    [Puzzle(answer: 2599)]
    public int PartOne(string input)
    {
        int ret = 0;
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        int width = lines[0].Length;
        int height = lines.Count;
        for (int y = 0; y <= height - 3; y++)
        {
            for (int x = 0; x <= width - 3; x++)
            {
                string dia1 = String.Empty + lines[y][x] + lines[y][x + 1] + lines[y][x + 2] + lines[y][x + 3];
                string dia2 = String.Empty + lines[y][x] + lines[y + 1][x + 1] + lines[y + 2][x + 2] + lines[y + 3][x + 3];
                string dia3 = String.Empty + lines[y][x] + lines[y + 1][x] + lines[y + 2][x] + lines[y + 3][x];
                string dia4 = String.Empty + lines[y+3][x] + lines[y+2][x + 1] + lines[y+1][x + 2] + lines[y][x + 3];

                if (dia1 == "XMAS" || dia1 == "SAMX")
                    ret++;

                if (dia2 == "XMAS" || dia2 == "SAMX")
                    ret++;

                if (dia3 == "XMAS" || dia3 == "SAMX")
                    ret++;

                if (dia4 == "XMAS" || dia4 == "SAMX")
                    ret++;
            }
        }
        return ret;
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
}