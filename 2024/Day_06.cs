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
        move(ref map);
        return map.Sum(row=>row.Count(c => c == 'X'));
    }

    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 6)]
    [TestCase("...#..\n....#.\n..#...\n.^.#..\n......", ExpectedResult = 1)]
    [Puzzle(answer: 4711)]
    public int PartTwo(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        var positions = move(ref map);
        var data = positions.Where(pos => VirtualWalkEndsInLoop(map, pos));
        return data.Count();
    }

    public List<Tuple<Position, Direction>> move(ref char[][] map)
    {
        var ret = new List<Tuple<Position, Direction>>();
        var pos = getPos(map);
        if (pos == null)
            return [];

        int targetY = 0;
        int targetX = 0;
        char directionSymbol = ' ';

        while (true)
        {
            switch ((Direction)map[pos.y][pos.x])
            {
                case Direction.North:
                    targetY = pos.y - 1;
                    targetX = pos.x;
                    directionSymbol = '^';
                    break;
                case Direction.East:
                    targetY = pos.y;
                    targetX = pos.x + 1;
                    directionSymbol = '>';
                    break;
                case Direction.South:
                    targetY = pos.y + 1;
                    targetX = pos.x;
                    directionSymbol = 'v';
                    break;
                case Direction.West:
                    targetY = pos.y;
                    targetX = pos.x - 1;
                    directionSymbol = '<';
                    break;
            }

            if (pos.y == 0 || pos.y == map.Length - 1 || pos.x == 0 || pos.x == map[0].Length - 1)
            {
                map[pos.y][pos.x] = 'X';
                return ret;
            }
            else if (map[targetY][targetX] == '#')
            {
                ret.Remove(new Tuple<Position, Direction>(new Position() { x = pos.x, y = pos.y }, (Direction)directionSymbol));
                map[pos.y][pos.x] = TurnRight((Direction)map[pos.y][pos.x]);
            }
            else
            {
                ret.Add(new Tuple<Position, Direction>(new Position { x = pos.x, y = pos.y }, (Direction)directionSymbol));
                map[pos.y][pos.x] = 'X';
                map[targetY][targetX] = directionSymbol;
                pos.y = targetY;
                pos.x = targetX;
            }
        }
    }
    public bool VirtualWalkEndsInLoop(char[][] map, Tuple<Position, Direction> position)
    {

        List<Tuple<Position, Direction>> foundPositionsAndDirections = [];

        Position startPos = new Position { x = position.Item1.x, y = position.Item1.y };
        Direction startDir = position.Item2;

        Position p = getDirectionOffset(startDir);
        char OriginalValue = map[startPos.y + p.y][startPos.x + p.x];
        map[startPos.y + p.y][startPos.x + p.x] = '#';


        Position pos = new Position { x = position.Item1.x, y = position.Item1.y };
        Direction dir = position.Item2;

        int directionY = 0;
        int directionX = 0;

        bool ret = false;

        while (true)
        {
            Position offset = getDirectionOffset(dir);
            directionX = offset.x; 
            directionY = offset.y;

            if (pos.y == 0 || pos.y == map.Length - 1 || pos.x == 0 || pos.x == map[0].Length - 1)
            {
                break;
            }

            int targetY = pos.y + directionY;
            int targetX = pos.x + directionX;

            char targetChar = map[targetY][targetX];

            var currentPositionAndDirection = new Tuple<Position, Direction>(new Position { x = pos.x, y = pos.y }, dir);
            if (foundPositionsAndDirections.Contains(currentPositionAndDirection))
            {
                ret = true;
                break;
            }
            else if (targetChar != '#')
            {
                foundPositionsAndDirections.Add(currentPositionAndDirection);
                pos.y = targetY;
                pos.x = targetX;
            }
            else
            {
                dir = (Direction)TurnRight(dir);
            }
        }

        map[startPos.y + p.y][startPos.x + p.x] = OriginalValue;
        return ret;
    }

    public Position? getPos(in char[][] Map)
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

    public char TurnRight(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return '>';
            case Direction.East:
                return 'v';
            case Direction.South:
                return '<';
            case Direction.West:
                return '^';
        }
        return ' ';
    }

    void PrintMap(in char[][] map) { 
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

public record Position{
    public int y {  get; set; }
    public int x {  get; set; }
};