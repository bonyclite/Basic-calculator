namespace LeetCode.BasicCalculator;

public record Token(string Value, TokenType Type)
{
	public static TokenType[] GetSignTypes() => [TokenType.Plus, TokenType.Divide, TokenType.Minus, TokenType.Multiply];
	public static TokenType[] GetPrioritySignTypes() => [TokenType.Divide, TokenType.Multiply];
}