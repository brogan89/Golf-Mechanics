using System.Collections.Generic;

public static class Utils
{
	public static string ToArrayString<T>(this IEnumerable<T> enumerator)
	{
		return "[\n" + string.Join(",\n", enumerator) + "\n]";
	}
}