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
                    listHigher.AddRange(surroundingupperNumbers(grid, pos));
                }
                listCurrent = listHigher;
                listHigher = [];
            }
            if (distinctTops) paths += listCurrent.Distinct().Count();
            else paths += listCurrent.Count;
        }

        return paths;
    }

    public List<Position> surroundingupperNumbers(Grid<int> grid, Position pos)
    {
        List<Position> ret = [];
        int num = grid.values[pos.y][pos.x] + 1;
        if (grid.InBounds(pos.x, pos.y + 1) && grid.values[pos.y + 1][pos.x] == num) ret.Add(new Position { x = pos.x, y = pos.y + 1 });
        if (grid.InBounds(pos.x, pos.y - 1) && grid.values[pos.y - 1][pos.x] == num) ret.Add(new Position { x = pos.x, y = pos.y - 1 });
        if (grid.InBounds(pos.x + 1, pos.y) && grid.values[pos.y][pos.x + 1] == num) ret.Add(new Position { x = pos.x + 1, y = pos.y });
        if (grid.InBounds(pos.x - 1, pos.y) && grid.values[pos.y][pos.x - 1] == num) ret.Add(new Position { x = pos.x - 1, y = pos.y });

        return ret;
    }
}
