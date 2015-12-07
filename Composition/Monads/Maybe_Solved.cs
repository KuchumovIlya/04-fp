using System;

namespace Composition.Monads
{
	public static class Maybe
	{
		public static Maybe<T> FromValue<T>(T value)
		{
			return new Maybe<T>(null, value);
		}

		public static Maybe<T> FromError<T>(Exception e)
		{
			return new Maybe<T>(e, default(T));
		}

		public static Maybe<T> Result<T>(Func<T> f)
		{
			try
			{
				return FromValue(f());
			}
			catch (Exception e)
			{
				return FromError<T>(e);
			}
		}
		public static Maybe<TOut> SelectMany<TIn, TTemp, TOut>(
			this Maybe<TIn> m, 
			Func<TIn, TTemp> map, 
			Func<TIn, TTemp, TOut> res)
		{
			return m.Success 
				? Result(() => res(m.Value, map(m.Value))) 
				: FromError<TOut>(m.Error);
		}

		public static Maybe<TOut> SelectMany<TIn, TOut>(
			this Maybe<TIn> m, 
			Func<TIn, TOut> map)
		{
			return m.Success
				? Result(() => map(m.Value)) 
				: FromError<TOut>(m.Error);
		}
	}

	public class Maybe<T>
	{
		public Maybe(Exception error, T value)
		{
			Error = error;
			Value = value;
		}

		public Exception Error { get; private set; }
		public T Value { get; private set; }
		public bool Success { get { return Error == null; } }
	}
}
