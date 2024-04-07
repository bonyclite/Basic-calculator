namespace BasicCalculator.BasicCalculator;

public class AnalyzeException : Exception
{
	public int Position { get; }

	public AnalyzeException(int position) : base($"На позиции '{position + 1}' неверный символ")
	{
		Position = position;
	}
}