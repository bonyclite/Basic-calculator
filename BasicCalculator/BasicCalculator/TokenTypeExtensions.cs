namespace LeetCode.BasicCalculator;

public static class TokenTypeExtensions
{
	public static bool IsSign(this TokenType self) =>
		self is TokenType.Divide or TokenType.Minus or TokenType.Multiply or TokenType.Plus;
}