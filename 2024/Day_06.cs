namespace AdventOfCode._2024;

public class Day_06
{
    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 41)]
    [Puzzle(answer: 4711)]
    public int PartOne(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        int AmountOfLoops = 0;
        while (getPos(map) != null)
        {
            while (forward(ref map, ref AmountOfLoops));
            TurnRight(map);
        }
        PrintMap(map);
        return map.Sum(row=>row.Count(c => c == 'X'));
    }

    [TestCase("....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...", ExpectedResult = 6)]
    [Puzzle(answer: 4711)]
    public int PartTwo(string input)
    {
        List<string> lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        char[][] map = lines.Select(str => str.ToArray()).ToArray();
        int AmountOfLoops = 0;
        while (getPos(map) != null)
        {
            while (forward(ref map, ref AmountOfLoops));
            TurnRight(map);
        }
        PrintMap(map);
        return AmountOfLoops;

    }

    public bool forward(ref char[][] map, ref int AmountOfLoops)
    {
        var pos = getPos(map);
        if (pos == null)
            return false;

        int targetY = 0;
        int targetX = 0;
        char directionSymbol = ' ';

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
            return true;
        }
        else if (map[targetY][targetX] == '#')
        {
            return false;
        }
        else
        {
            map[pos.y][pos.x] = 'X';
            map[targetY][targetX] = directionSymbol;
            if (BlockingForwardResultsInLoop(map))
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

    public bool BlockingForwardResultsInLoop(in char[][] map)
    {
        Position? pos = getPos(map);

        if (pos == null)
            return false;

        int forwardDirY = 0;
        int forwardDirX = 0;
        int rightDirY = 0;
        int rightDirX = 0;

        int posY = pos.y;
        int posX = pos.x;

        switch ((Direction)map[pos.y][pos.x])
        {
            case Direction.North:
                forwardDirY = -1;
                forwardDirX = 0;
                rightDirY = 0;
                rightDirX = 1;
                break;
            case Direction.East:
                forwardDirY = 0;
                forwardDirX = 1;
                rightDirY = 1;
                rightDirX = 0;
                break;
            case Direction.South:
                forwardDirY = 1;
                forwardDirX = 0;
                rightDirY = 0;
                rightDirX = -1;
                break;
            case Direction.West:
                forwardDirY = 0;
                forwardDirX = -1;
                rightDirY = -1;
                rightDirX = 0;
                break;
        }

        int forwardPosY = posY + forwardDirY;
        int forwardPosX = posX + forwardDirX;
        if (forwardPosY >= 0 && forwardPosY < map.Length && forwardPosX >= 0 && forwardPosX < map[0].Length && map[forwardPosY][forwardPosX] != '.')
            return false;

        while (posY > 0 && posY < map.Length - 1 && posX > 0 && posX < map[0].Length - 1){
            char c = map[posY][posX];
            if (c == '#')
                return false;
            else if (c == 'X' && map[posY + rightDirY][posX + rightDirX] == '#')
                return true;

            posY += rightDirY;
            posX += rightDirX;
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