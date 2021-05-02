using System;
using System.IO;

namespace EaseApp
{
    class Program
    {
        public static int Size { get; set; }
        public static string[][] Path { get; set; }
        public static int maxDeep = 0;
        public static string maxPath;

        public static int FindLongestPathFromAPosition(int i, int j, int[][] map, int[][] lookout)
        {
            if (i < 0 || i >= Size || j < 0 || j >= Size)
            {
                return 0;
            }

            if (lookout[i][j] != -1)
            {
                return lookout[i][j];
            }

            int east = int.MinValue, west = int.MinValue, south = int.MinValue, north = int.MinValue;

            if (j < Size - 1 && ((map[i][j]) > map[i][j + 1]))
            {
                east = lookout[i][j] = 1 + FindLongestPathFromAPosition(i, j + 1, map, lookout);
            }

            if (j > 0 && (map[i][j] > map[i][j - 1]))
            {
                west = lookout[i][j] = 1 + FindLongestPathFromAPosition(i, j - 1, map, lookout);
            }

            if (i > 0 && (map[i][j] > map[i - 1][j]))
            {
                north = lookout[i][j] = 1 + FindLongestPathFromAPosition(i - 1, j, map, lookout);
            }

            if (i < Size - 1 && (map[i][j] > map[i + 1][j]))
            {
                south = lookout[i][j] = 1 + FindLongestPathFromAPosition(i + 1, j, map, lookout);
            }

            lookout[i][j] = Math.Max(north, Math.Max(south, Math.Max(west, Math.Max(east, 1))));

            if (lookout[i][j] == north)
                Path[i][j] += "-" + Path[i - 1][j]?.ToString();
            else
            if (lookout[i][j] == south)
                Path[i][j] += "-" + Path[i + 1][j]?.ToString();
            else
            if (lookout[i][j] == east)
                Path[i][j] += "-" + Path[i][j + 1]?.ToString();
            else
            if (lookout[i][j] == west)
                Path[i][j] += "-" + Path[i][j - 1]?.ToString();

            return lookout[i][j];
        }

        public static int FindLongestPath(int[][] map)
        {
            int result = 1;

            int[][] lookput = PathMaps.IntPathMap(Size);
            Path = PathMaps.StringPathMap(Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    lookput[i][j] = -1;
                    Path[i][j] = map[i][j].ToString();
                }
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (lookput[i][j] == -1)
                    {
                        FindLongestPathFromAPosition(i, j, map, lookput);
                    }

                    result = Math.Max(result, lookput[i][j]);
                }
            }

            foreach (var item in Path)
            {
                foreach (var i in item)
                {
                    int[] pathList = Array.ConvertAll(i.Split('-'), int.Parse);

                    if (pathList.Length == result)
                    {
                        var deep = pathList[0] - pathList[pathList.Length - 1];

                        if (deep > maxDeep)
                        {
                            maxDeep = deep;
                            maxPath = i;
                        }
                    }
                }
            }

            return result;
        }

        public static class PathMaps
        {
            public static int[][] IntPathMap(int size)
            {
                int[][] newArray = new int[size][];
                for (int i = 0; i < size; i++)
                {
                    newArray[i] = new int[size];
                }

                return newArray;
            }

            public static string[][] StringPathMap(int size)
            {
                string[][] newArray = new string[size][];
                for (int i = 0; i < size; i++)
                {
                    newArray[i] = new string[size];
                }

                return newArray;
            }
        }

        static void Main(string[] args)
        {
            int[][] map = LoadMap();

            Console.WriteLine($"Length of calculated path is {FindLongestPath(map)}");
            Console.WriteLine($"Drop of calculated path is {maxDeep}");
            Console.WriteLine($"Calculated path is {maxPath}");
            Console.ReadLine();
        }

        private static int[][] LoadMap()
        {
            string line;
            string fileName = "map.txt";
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"Resources\", fileName);
            int x = 0;

            StreamReader file = new StreamReader(path);

            if ((line = file.ReadLine()) != null)
            {
                string[] mapSize = line.Split(' ');

                Size = int.Parse(mapSize[0].Trim());
            }

            int[][] map = new int[Size][];

            while ((line = file.ReadLine()) != null)
            {
                string[] data = line.Split(' ');

                int[] heights = Array.ConvertAll(data, int.Parse);

                map[x] = heights;

                x++;
            }

            file.Close();

            return map;
        }
    }
}
