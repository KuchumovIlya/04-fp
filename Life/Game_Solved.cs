using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife
{
	/*
	Теперь игра не знает про существование Ui.
	Остался неизменяемый класс ImmutableGame и чистая функция Step. 
	Это и есть стиль FP
	*/
	public class ImmutableGame : IReadonlyField
	{
		private readonly int width;
		private readonly int height;
		private readonly int[,] cellAge;
		public List<Point> LastChanged { get; private set; }

		public ImmutableGame(int[,] ages, IEnumerable<Point> lastChanged)
		{
			cellAge = ages;
			width = cellAge.GetLength(0);
			height = cellAge.GetLength(1);
			LastChanged = lastChanged.ToList();
		}

		public ImmutableGame(int width, int height, params Point[] aliveCells)
			: this(MakeAgesMap(width, height, aliveCells), aliveCells)
		{
		}

		private static int[,] MakeAgesMap(int width, int height, Point[] aliveCells)
		{
			var ages = new int[width, height];
			foreach (var pos in aliveCells)
				ages[(pos.X + width)%width, (pos.Y + height)%height] = 1;
			return ages;
		}

		public int GetAge(int x, int y)
		{
			return cellAge[(x + width) % width, (y + height) % height];
		}

		public ImmutableGame Step()
		{
			int[,] newCellAge = new int[width, height];
			var lastChanged = new List<Point>();
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					var aliveCount = GetNeighbours(x, y).Count(p => GetAge(p.X, p.Y) > 0);
					newCellAge[x, y] = NewCellAge(GetAge(x, y), aliveCount);
					if (newCellAge[x, y] != GetAge(x, y))
						lastChanged.Add(new Point(x, y));
			}
			return new ImmutableGame(newCellAge, lastChanged);
		}

		private static int NewCellAge(int age, int aliveNeighbours)
		{
			var willBeAlive = aliveNeighbours == 3 || aliveNeighbours == 2 && age > 0;
			return willBeAlive ? age + 1 : 0;
		}

		private IEnumerable<Point> GetNeighbours(int x, int y)
		{
			return
				from xx in Enumerable.Range(x - 1, 3)
				from yy in Enumerable.Range(y - 1, 3)
				where xx != x || yy != y
				select new Point(xx, yy);
		}

		public override string ToString()
		{
			var rows = Enumerable.Range(0, height)
				.Select(y => string.Join("", Enumerable.Range(0, width).Select(x => cellAge[x, y] > 0 ? "#" : " ")));
			return string.Join("\n", rows);
		}
	}
}