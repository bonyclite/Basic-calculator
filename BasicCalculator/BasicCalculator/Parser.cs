using BasicCalculator.BasicCalculator.Nodes;

namespace BasicCalculator.BasicCalculator;

public class Parser(IReadOnlyList<Token> tokens)
{
	private int _pos;

	public int Parse()
	{
		var node = ParseExpression();
		return Parse(node);
	}

	private int Parse(ExpressionNode node)
	{
		return node switch
		{
			NumberNode numberNode => numberNode.Number,
			BinOperatorNode binOperatorNode => binOperatorNode.Sign switch
			{
				TokenType.Plus => Parse(binOperatorNode.LeftOperand) + Parse(binOperatorNode.RightOperand),
				TokenType.Minus => Parse(binOperatorNode.LeftOperand) - Parse(binOperatorNode.RightOperand),
				TokenType.Divide => Parse(binOperatorNode.LeftOperand) / Parse(binOperatorNode.RightOperand),
				TokenType.Multiply => Parse(binOperatorNode.LeftOperand) * Parse(binOperatorNode.RightOperand),
				_ => throw new NotSupportedException()
			},
			_ => throw new NotSupportedException()
		};
	}

	private Token? Match(params TokenType[] expectedTypes)
	{
		if (_pos < tokens.Count)
		{
			var currentToken = tokens[_pos];

			if (expectedTypes.Any(t => t == currentToken.Type))
			{
				_pos++;
				return currentToken;
			}
		}

		return null;
	}

	private Token Require(params TokenType[] expectedTypes)
	{
		var token = Match(expectedTypes);

		if (token is null)
			throw new Exception($"На позиции {_pos} ожидается {expectedTypes[0]}");

		return token;
	}

	private ExpressionNode ParseExpression()
	{
		var leftNode = ParseParenthesesExpression();
		var sign = Match(Token.GetSignTypes());

		while (sign is not null)
		{
			var rightNode = ParseParenthesesExpression();
			var nextSignNode = Match(Token.GetPrioritySignTypes());

			if (nextSignNode is not null && sign.Type is TokenType.Minus or TokenType.Plus)
			{
				rightNode = ParsePriorityExpression(rightNode, nextSignNode);
			}
			
			leftNode = new BinOperatorNode(sign.Type, leftNode, rightNode);
			sign = Match(Token.GetSignTypes());
		}

		return leftNode;
	}

	private ExpressionNode ParseParenthesesExpression()
	{
		if (Match(TokenType.OpenBracket) is not null)
		{
			var node = ParseExpression();
			Require(TokenType.CloseBracket);
			return node;
		}

		return ParseNumber();
	}

	private ExpressionNode ParsePriorityExpression(ExpressionNode leftNode, Token? sign)
	{
		while (sign is not null)
		{
			var rightNode = ParseParenthesesExpression();
			leftNode = new BinOperatorNode(sign.Type, leftNode, rightNode);
			sign = Match(Token.GetPrioritySignTypes());
		}

		return leftNode;
	}

	private ExpressionNode ParseNumber()
	{
		return new NumberNode(Require(TokenType.Number));
	}
}