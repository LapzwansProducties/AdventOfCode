namespace AdventOfCode._2024;

public class Day_12
{
    [TestCase("AAAA\nBBCD\nBBCC\nEEEC", ExpectedResult = 140)]
    [TestCase("OOOOO\nOXOXO\nOOOOO\nOXOXO\nOOOOO", ExpectedResult = 772)]
    [TestCase("RRRRIICCFF\nRRRRIICCCF\nVVRRRCCFFF\nVVRCCCJFFF\nVVVVCJJCFE\nVVIVCCJJEE\nVVIIICJJEE\nMIIIIIJJEE\nMIIISIJEEE\nMMMISSJEEE", ExpectedResult = 1930)]
    [Puzzle(answer: 1433460)]
    public int PartOne(string input)
    {
        Grid<char> grid = new Grid<char>(input, false);
        Dictionary<Position, int> positionInGroup = new Dictionary<Position, int>();

        int groupnumber = 0;
        grid.ForEach((int x, int y) =>
        {
            if (positionInGroup.ContainsKey(new Position { x = x, y = y })) {
                return false;
            }

            positionInGroup.Add(new Position { x = x, y = y }, groupnumber);
            CheckSurroundings(grid, positionInGroup, x, y, groupnumber++);

            return false;
        });

        return positionInGroup.GroupBy(pos => pos.Value).Select(group => group.Select(t => t.Key).ToList()).Select(calculateCosts).Sum();
    }

    [TestCase("AAAA\nBBCD\nBBCC\nEEEC", ExpectedResult = 80)]
    [TestCase("OOOOO\nOXOXO\nOOOOO\nOXOXO\nOOOOO", ExpectedResult = 436)]
    [TestCase("EEEEE\nEXXXX\nEEEEE\nEXXXX\nEEEEE", ExpectedResult = 236)]
    [TestCase("AAAAAA\nAAABBA\nAAABBA\nABBAAA\nABBAAA\nAAAAAA", ExpectedResult = 368)]
    [TestCase("RRRRIICCFF\nRRRRIICCCF\nVVRRRCCFFF\nVVRCCCJFFF\nVVVVCJJCFE\nVVIVCCJJEE\nVVIIICJJEE\nMIIIIIJJEE\nMIIISIJEEE\nMMMISSJEEE", ExpectedResult = 1206)]
    [Puzzle(answer: 855082)]
    public int PartTwo(string input)
    {
        Grid<char> grid = new Grid<char>(input, false);
        Dictionary<Position, int> positionInGroup = new Dictionary<Position, int>();

        int groupnumber = 0;
        grid.ForEach((int x, int y) =>
        {
            if (positionInGroup.ContainsKey(new Position { x = x, y = y }))
            {
                return false;
            }

            positionInGroup.Add(new Position { x = x, y = y }, groupnumber);
            CheckSurroundings(grid, positionInGroup, x, y, groupnumber++);

            return false;
        });

        return positionInGroup.GroupBy(pos => pos.Value).Select(group => group.Select(t => t.Key).ToList()).Select(calculateCostsDiscount).Sum();
    }

    void CheckSurroundings(Grid<char> grid, Dictionary<Position, int> positionInGroup, int x, int y, int groupnumber)
    {
        char c = grid.values[y][x];
        if (!positionInGroup.ContainsKey(new Position { x = x +1 , y = y }) && grid.InBounds(x + 1, y) && grid.values[y][x + 1] == c) {
            positionInGroup.Add(new Position { x = x + 1, y = y }, groupnumber);
            CheckSurroundings(grid, positionInGroup, x + 1, y, groupnumber);
        }
        if (!positionInGroup.ContainsKey(new Position { x = x - 1, y = y }) && grid.InBounds(x - 1, y) && grid.values[y][x - 1] == c)
        {
            positionInGroup.Add(new Position { x = x - 1, y = y }, groupnumber);
            CheckSurroundings(grid, positionInGroup, x - 1, y, groupnumber);
        }
        if (!positionInGroup.ContainsKey(new Position { x = x, y = y + 1 }) && grid.InBounds(x, y + 1) && grid.values[y + 1][x] == c)
        {
            positionInGroup.Add(new Position { x = x, y = y + 1 }, groupnumber);
            CheckSurroundings(grid, positionInGroup, x, y + 1, groupnumber);
        }
        if (!positionInGroup.ContainsKey(new Position { x = x, y = y - 1 }) && grid.InBounds(x, y - 1) && grid.values[y - 1][x] == c)
        {
            positionInGroup.Add(new Position { x = x, y = y - 1 }, groupnumber);
            CheckSurroundings(grid, positionInGroup, x, y - 1, groupnumber);
        }
    }

    int calculateCosts(List<Position> positions) {
        int totalFences = 0;
        foreach (var pos in positions) {
            int neighboars = 0;
            if (positions.Contains(new Position { x = pos.x + 1, y = pos.y }))
            {
                neighboars++;
            }
            if (positions.Contains(new Position { x = pos.x - 1, y = pos.y }))
            {
                neighboars++;
            }
            if (positions.Contains(new Position { x = pos.x, y = pos.y + 1 }))
            {
                neighboars++;
            }
            if (positions.Contains(new Position { x = pos.x, y = pos.y - 1 }))
            {
                neighboars++;
            }

            totalFences += 4 - neighboars;
        }

        return positions.Count * totalFences;
    }

    int calculateCostsDiscount(List<Position> positions)
    {
        int minX = positions.Min(pos => pos.x);
        int maxX = positions.Max(pos => pos.x);
        int minY = positions.Min(pos => pos.y);
        int maxY = positions.Max(pos => pos.y);

        int edges = 0;

        // horizontal edges
        for (int y = minY; y <= maxY; y++) {
            bool upEdge = false;
            bool downEdge = false;
            for (int x = minX; x <= maxX; x++)
            {
                if (!positions.Contains(new Position { x = x, y = y }))
                {
                    if (upEdge)
                        edges++;
                    upEdge = false;
                    if (downEdge)
                        edges++;
                    downEdge = false;
                    continue;
                }

                if (positions.Contains(new Position { x = x, y = y - 1 }))
                {
                    if (upEdge) edges++;
                    upEdge = false;
                }
                else
                {
                    upEdge = true;
                }

                if (positions.Contains(new Position { x = x, y = y + 1 }))
                {
                    if (downEdge) edges++;
                    downEdge = false;
                }
                else
                {
                    downEdge = true;
                }
            }

            if (upEdge)
                edges++;
            if (downEdge)
                edges++;
        }

        // vertical edge
        for (int x = minX; x <= maxX; x++)
        {
            bool leftEdge = false;
            bool rightEdge = false;
            for (int y = minY; y <= maxY; y++)
            {
                if (!positions.Contains(new Position { x = x, y = y }))
                {
                    if (leftEdge) 
                        edges++;
                    leftEdge = false;
                    if (rightEdge) 
                        edges++;
                    rightEdge = false;
                    continue;
                }

                if (positions.Contains(new Position { x = x - 1, y = y }))
                {
                    if (leftEdge) 
                        edges++;
                    leftEdge = false;
                }
                else
                {
                    leftEdge = true;
                }

                if (positions.Contains(new Position { x = x + 1, y = y }))
                {
                    if (rightEdge) 
                        edges++;
                    rightEdge = false;
                }
                else
                {
                    rightEdge = true;
                }
            }

            if (leftEdge)
                edges++;
            if (rightEdge)
                edges++;
        }

        return positions.Count * edges;
    }
}
