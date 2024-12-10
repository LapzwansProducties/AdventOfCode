using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    public class Grid<T>
    {
        public T[][] values;
        public int height { get; init; }
        public int width { get; init; }
        public int maxY { get; init; }
        public int maxX { get; init; }



        public Grid(string input, bool seperatedBySpace)
        {
            var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            height = lines.Length;
            maxY = height - 1 ;

            if (typeof(T) == typeof(char))
            {
                values = input
                    .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => str.TrimEnd('\n').TrimEnd('\r').ToArray() as T[]).ToArray();
                width = lines[0].Length;
                maxX = width - 1;
            }
            else if (typeof(T) == typeof(int))
            {
                if (seperatedBySpace)
                    values = input
                        .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.TrimEnd('\n').TrimEnd('\r').Split(' ').Select(int.Parse).ToArray() as T[]).ToArray();
                else
                    values = input
                        .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.TrimEnd('\n').TrimEnd('\r').ToArray().Select(c => c - '0').ToArray() as T[]).ToArray();
                width = values[0].Length;
                maxX = width - 1;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void ForEach(Func<int, int, object> function) {
            for (int y = 0; y < values.Length; y++)
            {
                for (int x = 0; x < values[0].Length; x++)
                {
                    function.Invoke(x, y);
                }
            }
        }

        public void ForEach(Func<T, object> function)
        {
            for (int y = 0; y < values.Length; y++)
            {
                for (int x = 0; x < values[0].Length; x++)
                {
                    function.Invoke(values[y][x]);
                }
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && x <= maxX && y >= 0 && y <= maxY;

        public bool InBoundsNotEdge(int x, int y) => x > 0 && x < maxX && y > 0 && y < maxY;

        public bool OnEdge(int x, int y) => x == 0 || x == maxX || y == 0 || y == maxY;

        public void Print()
        {
            if (typeof(T) == typeof(char))
            {
                if (values == null)
                {
                    return;
                }
                foreach(var row in values)
                {
                    foreach(var col in row)
                    {
                        Console.Write(col);
                    }
                    Console.Write('\n');
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (values == null)
                {
                    return;
                }
                foreach (var row in values)
                {
                    foreach (var col in row)
                    {
                        Console.Write(col);
                        Console.Write(' ');
                    }
                    Console.Write('\n');
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
