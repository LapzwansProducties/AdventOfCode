namespace AdventOfCode._2024;

public class Day_06
{
    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 41)]
    [Puzzle(answer: 4711)]
    public int PartOne(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        bool wasIntersectionOrCorner = false;
        int AmountOfLoops = 0;
        while (getPos(map) != null)
        {
            while (forward(ref map, ref wasIntersectionOrCorner, ref AmountOfLoops));
            TurnRight(map);
        }
        PrintMap(map);
        return map.Sum(row=>row.Count(c => c=='-' || c == '|' || c=='+'));
    }

    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 6)]
    [Puzzle(answer: 4711)]
    public int PartTwo(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        bool wasIntersectionOrCorner = false;
        int AmountOfLoops = 0;
        while (getPos(map) != null)
        {
            while (forward(ref map, ref wasIntersectionOrCorner, ref AmountOfLoops));
            TurnRight(map);
        }
        PrintMap(map);
        return AmountOfLoops;

    }

    public bool forward(ref char[][] map, ref bool wasIntersectionOrCorner, ref int AmountOfLoops)
    {
        var pos = getPos(map);
        if (pos == null)
            return false;

        int targetY = 0;
        int targetX = 0;
        char footprintSymbol = ' ';
        char directionSymbol = ' ';

        switch ((Direction)map[pos.y][pos.x])
        {
            case Direction.North:
                targetY = pos.y - 1;
                targetX = pos.x;
                footprintSymbol = '|';
                directionSymbol = '^';
                break;
            case Direction.East:
                targetY = pos.y;
                targetX = pos.x + 1;
                footprintSymbol = '-';
                directionSymbol = '>';
                break;
            case Direction.South:
                targetY = pos.y + 1;
                targetX = pos.x;
                footprintSymbol = '|';
                directionSymbol = 'v';
                break;
            case Direction.West:
                targetY = pos.y;
                targetX = pos.x - 1;
                footprintSymbol = '-';
                directionSymbol = '<';
                break;
        }

        if (wasIntersectionOrCorner)
            footprintSymbol = '+';

        if (pos.y == 0 || pos.y == map.Length - 1 || pos.x == 0 || pos.x == map[0].Length - 1)
        {
            map[pos.y][pos.x] = footprintSymbol;
            wasIntersectionOrCorner = false;
            return true;
        }
        else if (map[targetY][targetX] == '#')
        {
            wasIntersectionOrCorner = true;
            return false;
        }
        else if (new char[] {'|', '-', '+' }.Contains(map[targetY][targetX]))
        {
            map[pos.y][pos.x] = footprintSymbol;
            map[targetY][targetX] = directionSymbol;
            wasIntersectionOrCorner = true;
            if (RightIsWalkedPath(map))
                AmountOfLoops++;
            return true;
        }
        else
        {
            map[pos.y][pos.x] = footprintSymbol;
            map[targetY][targetX] = directionSymbol;
            wasIntersectionOrCorner = false;
            if (RightIsWalkedPath(map))
                AmountOfLoops++;
            return true;
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
                        return new Position(y, x);
                    default:
                        continue;
                }
            }
        }

        return null;
    }

    public void TurnRight(in char[][] map) {
        Position? pos = getPos(map);

        if (pos == null)
            return;

        map[pos.y][pos.x] = TurnRight((Direction)map[pos.y][pos.x]);
    }

    public bool RightIsWalkedPath(in char[][] map)
    {
        Position? pos = getPos(map);

        if (pos == null)
            return false;

        int dirY = 0;
        int dirX = 0;

        int posY = pos.y;
        int posX = pos.x;

        switch ((Direction)map[pos.y][pos.x])
        {
            case Direction.North:
                dirY = 0;
                dirX = 1;
                break;
            case Direction.East:
                dirY = 1;
                dirX = 0;
                break;
            case Direction.South:
                dirY = 0;
                dirX = -1;
                break;
            case Direction.West:
                dirY = -1;
                dirX = 0;
                break;
        }

        while (posY > 0 && posY < map.Length && posX > 0 && posX < map[0].Length){
            char c = map[posY][posX];
            if (c == '#')
                return false;
            if (new char[] { '|', '-', '+' }.Contains(c) && map[posY + dirY][posX+dirX] == '#')
                return true;

            posY += dirY;
            posX += dirX;
        }

        return false;
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

public record Position(int y, int x);