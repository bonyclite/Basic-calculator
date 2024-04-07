namespace LeetCode.BasicCalculator.Nodes;

public record NumberNode : ExpressionNode
{
	public int Number { get; private set; }
	
	public NumberNode(Token token)
	{
		if (token.Type != TokenType.Number)
			throw new Exception("Ожидается токен с типоп Number");
		
		Number = int.Parse(token.Value);
	}
}