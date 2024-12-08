namespace AdventOfCode._2024;

public class Day_08
{
    [TestCase("..........\n..........\n..........\n....a.....\n..........\n.....a....\n..........\n..........\n..........\n..........", ExpectedResult = 2)]
    [TestCase("..........\n..........\n..........\n....a.....\n........a.\n.....a....\n..........\n..........\n..........\n..........", ExpectedResult = 4)]
    [TestCase("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............", ExpectedResult = 14)]
    [Puzzle(answer: 280)]
    public int PartOne(string input)
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        int height = lines.Length;
        int width = lines[0].Length;

        List<Tuple<Char, Position>> positions = [];
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines.Length; x++)
            {
                char c = lines[y][x];
                if (lines[y][x] != '.')
                    positions.Add(new Tuple<char, Position>(c, new Position { x = x, y = y }));
            }
        }

        var antennasGrouped = positions.GroupBy(pair => pair.Item1).ToList();
        List<Position> antinodes = [];


        foreach (var group in antennasGrouped)
        {
            for (int antenna = 0; antenna < group.Count(); antenna++)
            {
                for (int otherAntenna = antenna + 1; otherAntenna < group.Count(); otherAntenna++)
                {
                    Position antenna1 = group.ElementAt(antenna).Item2;
                    Position antenna2 = group.ElementAt(otherAntenna).Item2;
                    int xdiff = antenna1.x - antenna2.x;
                    int ydiff = antenna1.y - antenna2.y;

                    List<Position> possibleAntinodePositions = new List<Position>()
                    {
                        new Position { x = antenna1.x + xdiff, y = antenna1.y + ydiff },
                        new Position { x = antenna1.x - xdiff, y = antenna1.y - ydiff },
                        new Position { x = antenna2.x + xdiff, y = antenna2.y + ydiff },
                        new Position { x = antenna2.x - xdiff, y = antenna2.y - ydiff },
                    };

                    foreach (var position in possibleAntinodePositions)
                    {
                        if (position.x < width && position.x >= 0 && position.y < height && position.y >= 0 && position != antenna1 && position != antenna2 && !antinodes.Contains(position))
                            antinodes.Add(position);
                    }
                }
            }
        }

        return antinodes.Count;
    }

    [TestCase("T.........\n..........\n.T........\n..........\n..........\n..........\n..........\n..........\n..........\n..........", ExpectedResult = 5)]
    [TestCase("T.........\n...T......\n.T........\n..........\n..........\n..........\n..........\n..........\n..........\n..........", ExpectedResult = 9)]
    [TestCase("............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............", ExpectedResult = 34)]
    [Puzzle(answer: 958)]
    public int PartTwo(string input)
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        int height = lines.Length;
        int width = lines[0].Length;

        List<Tuple<Char, Position>> positions = [];
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines.Length; x++)
            {
                char c = lines[y][x];
                if (lines[y][x] != '.')
                    positions.Add(new Tuple<char, Position>(c, new Position { x = x, y = y }));
            }
        }

        var antennasGrouped = positions.GroupBy(pair => pair.Item1).ToList();
        List<Position> antinodes = [];


        foreach (var group in antennasGrouped)
        {
            for (int antenna = 0; antenna < group.Count(); antenna++)
            {
                for (int otherAntenna = antenna + 1; otherAntenna < group.Count(); otherAntenna++)
                {
                    Position antenna1 = group.ElementAt(antenna).Item2;
                    Position antenna2 = group.ElementAt(otherAntenna).Item2;
                    int xdiff = antenna1.x - antenna2.x;
                    int ydiff = antenna1.y - antenna2.y;

                    Position posCopy = new Position { x = antenna1.x, y = antenna1.y };

                    while (posCopy.x < width && posCopy.x >= 0 && posCopy.y < height && posCopy.y >= 0)
                    {
                        if (!antinodes.Contains(posCopy))
                            antinodes.Add(new Position { x = posCopy.x, y = posCopy.y });
                        posCopy.x += xdiff;
                        posCopy.y += ydiff;
                    }

                    posCopy = new Position { x = antenna1.x, y = antenna1.y };

                    while (posCopy.x < width && posCopy.x >= 0 && posCopy.y < height && posCopy.y >= 0)
                    {
                        if (!antinodes.Contains(posCopy))
                            antinodes.Add(new Position { x = posCopy.x, y = posCopy.y});
                        posCopy.x -= xdiff;
                        posCopy.y -= ydiff;
                    }

                }
            }
        }

        return antinodes.Count;
    }
}

