using System;
using System.IO;
using System.Linq;
using static System.Console;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
	static string Clean(string text)
	{
		var colonIndex = text.IndexOf(":");
		var spaceIndex = text.IndexOf(" ", colonIndex);
		text = text.Substring(spaceIndex);
		text = Regex.Replace(text, @"[\:\.\,\(\)\[|\]\;\?\'\!]", string.Empty);
		text = text.Replace(Environment.NewLine, string.Empty);
		return text.Trim();
	}
	
	static void Main()
	{
		const string Path = @"C:/users/mikes/dropbox/docs/kjv.txt";
		var verses = File.ReadAllLines(Path).Select(Clean);
		var text = string.Join(" ", verses);
		var words = text.Split(' ').Select(w => w.ToUpper());
		var uniqueWords = words.Distinct();
		
		var wordCounts = uniqueWords
			.AsParallel()
			.GroupBy(w => w)
			.Select(w => new { Word = w.Key, Count = w.Count() })
			.OrderByDescending(w => w.Count);
		
		foreach (var wordCount in wordCounts)
		{
			WriteLine($"{wordCount.Word}\t{wordCount.Count}");
		}		
	}
}