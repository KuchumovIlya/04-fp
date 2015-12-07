using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Composition.HOF
{
	public class Summator
	{
		/*
		Отрефакторите код.
			1. Отделите максимум логики от побочных эффектов.
			2. Создайте нужные вам методы.
			3. Сделайте так, чтобы максимум кода оказалось внутри универсальных методов, потенциально полезных в других местах программы.
		*/

		public static string ProcessLine(string[] nums)
		{
			var sum = nums.Select(n => Convert.ToInt32(n, 16)).Sum();
			return string.Format("Sum({0}) = {1:x}", string.Join(" ", nums), sum);
		}

		public static void Process()
		{
			using (var dataSource = new DataSource())
			{
				var res = Enumer.Repeat(dataSource.NextData)
					.TakeWhile(x => x != null)
					.OnEvery(100, c => Console.WriteLine("processed {0} items", c))
					.Select(ProcessLine);
				File.WriteAllLines("process-result.txt", res);
			}
		}
	}

	public static class Enumer
	{
		public static IEnumerable<T> Repeat<T>(Func<T> get)
		{
			while (true) yield return get();
			// ReSharper disable once FunctionNeverReturns
		}

		public static IEnumerable<T> OnEvery<T>(this IEnumerable<T> items, int period, Action<int> beforeNth)
		{
			var c = 0;
			foreach (var item in items)
			{
				c++;
				if (c % period == 0) beforeNth(c);
				yield return item;
			}
		} 
	}
}