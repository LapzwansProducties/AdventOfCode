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
    [Puzzle(answer: 4711)]
    public int PartTwo(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        var positions = move(ref map);
        var data = positions.Where(pos => VirtualWalkEndsInLoop(map, pos));
        return data.Count();
    }

    public Dictionary<Position, Direction> move(ref char[][] map)
    {
        var ret = new Dictionary<Position, Direction>();
        var pos = getPos(map);
        if (pos == null)
            return [];

        int targetY = 0;
        int targetX = 0;
        char directionSymbol = ' ';
        bool afterCorner = false;

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
                map[pos.y][pos.x] = TurnRight((Direction)map[pos.y][pos.x]);
                ret.Remove(new Position() { x = pos.x, y = pos.y });
                afterCorner = true;
            }
            else
            {
                map[pos.y][pos.x] = 'X';
                map[targetY][targetX] = directionSymbol;
                pos.y = targetY;
                pos.x = targetX;

                if(!afterCorner && !ret.ContainsKey(new Position { x = targetX, y = targetY }))
                    ret.Add(new Position { x = targetX, y = targetY }, (Direction)directionSymbol);

                afterCorner = false;
            }
        }
    }
    public bool VirtualWalkEndsInLoop(char[][] map, KeyValuePair<Position, Direction> position)
    {
        var pos = position.Key;
        var dir = position.Value;

        dir = (Direction)TurnRight(dir);

        int directionY = 0;
        int directionX = 0;

        while (true)
        {
            switch (dir)
            {
                case Direction.North:
                    directionY = -1;
                    directionX = 0;
                    break;
                case Direction.East:
                    directionY = 0;
                    directionX = +1;
                    break;
                case Direction.South:
                    directionY = 1;
                    directionX = 0;
                    break;
                case Direction.West:
                    directionY = 0 ;
                    directionX = -1;
                    break;
            }


            if (pos.y == 0 || pos.y == map.Length - 1 || pos.x == 0 || pos.x == map[0].Length - 1)
            {
                return false;
            }

            int targetY = pos.y + directionY;
            int targetX = pos.x + directionX;

            char targetChar = map[targetY][targetX];

            if (targetChar != '#')
            {
                pos.y = targetY;
                pos.x = targetX;
            }
            else
            {
                if (map[pos.y][pos.x] == 'X')
                    return true;
                else
                    dir = (Direction)TurnRight(dir);
            }
        }
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