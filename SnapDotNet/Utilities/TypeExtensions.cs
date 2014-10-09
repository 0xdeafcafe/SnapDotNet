using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapDotNet.Utilities
{
	public static class TypeExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="chunkSize"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
		{
			while (source.Any())
			{
				yield return source.Take(chunkSize);
				source = source.Skip(chunkSize);
			}
		}
	}
}
