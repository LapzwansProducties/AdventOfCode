using System.IO;

namespace AdventOfCode._2024;

public class Day_06
{
    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 41)]
    [Puzzle(answer: 4711)]
    public int PartOne(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        return move(ref map).Count;
    }

    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 6)]
    [TestCase("...#..\n....#.\n..#...\n.^.#..\n......", ExpectedResult = 1)]
    [Puzzle(answer: 1562)]
    public int PartTwo(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();

        int amountOfLoops = 0;
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map.Length; x++)
            {
                char origionalValue = map[y][x];
                map[y][x] = '#';

                if (move(ref map) == null)
                    amountOfLoops++;
                map[y][x] = origionalValue;
            }
        }
        return amountOfLoops;
    }

    public Dictionary<Position, int>? move(ref char[][] map)
    {
        var ret = new Dictionary<Position, int>();
        var pos = getPos(map);
        if (pos == null)
            return [];
        char directionSymbol = ' ';

        Direction dir = (Direction)map[pos.y][pos.x];

        while (!ret.ContainsValue(5))
        {
            Position offset = getDirectionOffset(dir);
            int targetY = pos.y + offset.y;
            int targetX = pos.x + offset.x;

            if (pos.y == 0 || pos.y == map.Length - 1 || pos.x == 0 || pos.x == map[0].Length - 1)
            {
                ret.Add(new Position { x = pos.x, y = pos.y }, 1);
                return ret;
            }
            else if (map[targetY][targetX] == '#')
            {
                dir = TurnRight(dir);
            }
            else
            {
                Position posCopy = new Position { x = pos.x, y = pos.y };
                if (ret.ContainsKey(posCopy))
                {
                    ret[posCopy]++;
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