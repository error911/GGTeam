﻿// StringExtensions.cs
//

using System;
using System.Linq;

namespace GGTools.SmartLocalization.Editor
{
/// <summary>
/// Contains extension methods for string
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Removes all the whitespaces in a string
	/// </summary>
	/// <param name="input">The string to remove whitespaces from</param>
	/// <returns>The string with the removed whitespaces</returns>
	public static string RemoveWhitespace(this string input)
	{
		return new string(input.ToCharArray()
			.Where(c => !Char.IsWhiteSpace(c))
			.ToArray());
	}
}
}
