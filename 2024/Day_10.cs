namespace AdventOfCode._2024;

public class Day_10
{
    [TestCase("..90..9\n...1.98\n...2..7\n6543456\n765.987\n876....\n987....", ExpectedResult = 4)]
    [TestCase("89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732", ExpectedResult = 36)]
    [Puzzle(answer: 482)]
    public int PartOne(string input)
    {
        return getPathCount(input, true);
    }

    [TestCase("89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732", ExpectedResult = 81)]
    [Puzzle(answer: 1094)]
    public int PartTwo(string input)
    {
        return getPathCount(input, false);
    }

    int getPathCount(string input, bool distinctTops) {
        Grid<int> grid = new Grid<int>(input, false);
        List<Position> zeroes = [];
        grid.ForEach((x, y) =>
        {
            if (grid.values[y][x] == 0)
            {
                zeroes.Add(new Position { x = x, y = y });
            }
            return false;
        });

        int paths = 0;
        foreach (var zeroPos in zeroes)
        {
            List<Position> listCurrent = [zeroPos];
            List<Position> listHigher = [];
            for (int num = 0; num < 9; num++)
            {
                foreach (var pos in listCurrent)
                {
                    listHigher.AddRange(surroundingupperNumbers(grid, pos.x, pos.y));
                }
                listCurrent = listHigher;
                listHigher = [];
            }
            paths += distinctTops ? listCurrent.Distinct().Count() : listCurrent.Count;
        }

        return paths;
    }

    public List<Position> surroundingupperNumbers(Grid<int> grid, int x, int y)
    {
        List<Position> ret = [];
        int num = grid.values[y][x] + 1;
        if (y + 1 <= grid.maxY && grid.values[y + 1][x] == num) ret.Add(new Position { x = x, y = y + 1 });
        if (y - 1 >= 0         && grid.values[y - 1][x] == num) ret.Add(new Position { x = x, y = y - 1 });
        if (x + 1 <= grid.maxX && grid.values[y][x + 1] == num) ret.Add(new Position { x = x + 1, y = y });
        if (x - 1 >= 0         && grid.values[y][x - 1] == num) ret.Add(new Position { x = x - 1, y = y });
        return ret;
    }
}
