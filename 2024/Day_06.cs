using System.IO;

namespace AdventOfCode._2024;

public class Day_06
{
    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 41)]
    [Puzzle(answer: 4711)]
    public int PartOne(string input)
    {
        Grid<char> grid = new Grid<char>(input, true);
        return move(grid).Count;
    }

    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 6)]
    [TestCase("...#..\n....#.\n..#...\n.^.#..\n......", ExpectedResult = 1)]
    [Puzzle(answer: 1562)]
    public int PartTwo(string input)
    {
        Grid<char> grid = new Grid<char>(input, true);

        char[][] map = grid.values;

        int amountOfLoops = 0;
        grid.ForEach((x, y) =>
        {
            char origionalValue = map[y][x];
            map[y][x] = '#';

            if (move(grid) == null)
                amountOfLoops++;
            map[y][x] = origionalValue;
            return false;
        });
        return amountOfLoops;
    }

    public Dictionary<Position, int>? move(Grid<char> grid)
    {
        var ret = new Dictionary<Position, int>();
        var pos = getPos(grid.values);
        if (pos == null)
            return [];

        Direction dir = (Direction)grid.values[pos.y][pos.x];

        while (!ret.ContainsValue(5))
        {
            Position offset = getDirectionOffset(dir);
            int targetY = pos.y + offset.y;
            int targetX = pos.x + offset.x;

            if (grid.OnEdge(pos.x, pos.y))
            {
                ret.Add(new Position { x = pos.x, y = pos.y }, 1);
                return ret;
            }
            else if (grid.values[targetY][targetX] == '#')
            {
                dir = TurnRight(dir);
            }
            else
            {
                if (ret.TryGetValue(pos, out int value))
                {
                    ret[pos] = ++value;
                }
                else
                {
                    ret.Add(new Position { x = pos.x, y = pos.y }, 1);
                }
                pos.y = targetY;
                pos.x = targetX;
            }
        }
        return null;
    }

    public Position? getPos(char[][] Map)
    {
        for (int y = 0; y < Map.Length; y++)
        {
            for (int x = 0; x < Map[y].Length; x++)
            {
                switch (Map[y][x])
                {
                    case '^':
                    case '>':
                    case 'v':
                    case '<':
                        return new Position {y=y, x=x};
                    default:
                        continue;
                }
            }
        }
        return null;
    }

    public static Direction TurnRight(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            default:
                return Direction.North;
        }
    }

    public static void PrintMap(in char[][] map) { 
        foreach(var row in map)
        {
            Console.WriteLine(row);
        }
    }

    public static Position getDirectionOffset(Direction dir) {
        switch (dir)
        {
            case Direction.North:
                return new Position() { y = -1, x = -0 };
            case Direction.East:
                return new Position() { y = 0, x = 1 };
            case Direction.South:
                return new Position() { y = 1, x = -0 };
            default:
                return new Position() { y = -0, x = -1 };
        }
    }
}

public enum Direction
{
    North = '^',
    East = '>',
    South = 'v',
    West = '<'
}