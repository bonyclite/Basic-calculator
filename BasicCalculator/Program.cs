// See https://aka.ms/new-console-template for more information

using BasicCalculator.BasicCalculator;

var strs = new[]
{
	"1 + 2",
	"1 +- 2",
	"2 + 2 * 3",
	"(1 + 2 - 3)",
	"((1 * 2 + 3)",
	"(1 + 2) - (3 * 4)",
	"((1 + 2) - (3 * 4))",
	"(((1)))",
	"(((1+)))",
	"(1 + 3 + 2) / (2 -3) + 1",
	"+ 2 3",
	"+",
	"2 + (3",
	"2 + (3 * 5",
	"2 + (3 * 5)",
	"3 * 2 + 5 + (1 - 3 * (2 + 4))",
	"3 + 2 * 5 + (1 - 3 * (2 + 4))",
	"3 + 4 * 5 / 2",
	"3 + 4 * 5 * 2",
	"1 / 1"
};
	
foreach (var str in strs)
{
	Console.WriteLine("---------------------------------");
	try
	{
		var lexer = new Lexer(str);
		var parser = new Parser(lexer.Analyze());
		var res = parser.Parse();
		Console.WriteLine($"{str} = {res}");
	}
	catch (AnalyzeException e)
	{
		Console.WriteLine(e.Message);
		for (var i = 0; i < str.Length; i++)
		{
			Console.ForegroundColor = i == e.Position ? ConsoleColor.Red : ConsoleColor.Black;
			Console.Write(str[i]);
		}

		Console.ForegroundColor = ConsoleColor.Black;
		Console.WriteLine();
	}
}