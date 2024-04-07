using System.Collections;
using System.Text;

namespace LeetCode.BasicCalculator;

public class Lexer(string str) : IEnumerator<Token>, IEnumerable<Token>
{
	private readonly List<TokenWithPosition> _tokens = [];

	private int _pos;

	public Token[] Analyze()
	{
		while (MoveNext())
		{
		}

		Validate();

		var normalTokens = _tokens.Cast<Token>().ToArray();
		return normalTokens;
	}

	public bool MoveNext()
	{
		if (_pos >= str.Length)
			return false;

		var currSymbol = str[_pos];
		if (char.IsDigit(currSymbol))
		{
			var sbNumber = new StringBuilder();

			while (_pos < str.Length && char.IsDigit(str[_pos]))
			{
				sbNumber.Append(str[_pos]);
				_pos++;
			}

			var originalNumberPosition = _pos - sbNumber.Length;
			_tokens.Add(TokenWithPosition.CreateNumberToken(sbNumber.ToString(), originalNumberPosition));
			return true;
		}

		switch (currSymbol)
		{
			case '(':
				_tokens.Add(TokenWithPosition.CreateOpenBracketToken(_pos));
				break;

			case ')':
				_tokens.Add(TokenWithPosition.CreateCloseBracketToken(_pos));
				break;

			case '+':
				_tokens.Add(TokenWithPosition.CreatePlusToken(_pos));
				break;
			
			case '-':
				_tokens.Add(TokenWithPosition.CreateMinusToken(_pos));
				break;
			
			case '/':
				_tokens.Add(TokenWithPosition.CreateDivideToken(_pos));
				break;
			
			case '*':
				_tokens.Add(TokenWithPosition.CreateMultiplyToken(_pos));
				break;

			case ' ':
				break;

			default:
				throw new Exception($"На позиции '{_pos + 1}' обнаружен недопустимый символ '{currSymbol}'");
		}

		_pos++;
		return true;
	}

	private void Validate()
	{
		var bracketsStack = new Stack<(char bracket, int pos)>();
		TokenWithPosition? prevToken = null;
		
		foreach (var token in _tokens)
		{
			switch (token.Type)
			{
				case TokenType.Number:
				{
					if (prevToken is null || prevToken.Type.IsSign() || prevToken.Type is TokenType.OpenBracket)
						break;

					throw Exception(prevToken.OriginalPos);
				}

				case TokenType.Plus or TokenType.Minus or TokenType.Divide or TokenType.Multiply:
				{
					if (prevToken?.Type is TokenType.Number or TokenType.CloseBracket)
						break;

					throw Exception(token.OriginalPos);
				}

				case TokenType.OpenBracket:
				{
					bracketsStack.Push(('(', token.OriginalPos));
					break;
				}

				case TokenType.CloseBracket:
				{
					var pos = token.OriginalPos;
					
					if (bracketsStack.TryPop(out var item))
					{
						if (item.bracket == '(')
							break;

						pos = item.pos;
					}

					throw Exception(pos);
				}

				default:
					throw new ArgumentOutOfRangeException();
			}

			prevToken = token;
		}

		if (bracketsStack.TryPop(out var i))
			throw Exception(i.pos);

		var numberAndSignTokens = _tokens
			.Where(t => t.Type is TokenType.Number || t.Type.IsSign())
			.ToArray();

		if (numberAndSignTokens.Length < 3)
		{
			var signToken = numberAndSignTokens.FirstOrDefault(s => s.Type.IsSign());
			var numberToken = numberAndSignTokens.FirstOrDefault(s => s.Type == TokenType.Number);
			
			throw Exception(signToken?.OriginalPos ?? numberToken?.OriginalPos ?? 0);	
		}
	}

	public void Reset()
	{
		_tokens.Clear();
	}

	public Token Current => _tokens.Last();

	object IEnumerator.Current => Current;

	public void Dispose()
	{
	}

	public IEnumerator<Token> GetEnumerator()
	{
		return _tokens.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private AnalyzeException Exception(int? pos = null) => new(pos ?? _pos);

	private record TokenWithPosition(string Value, TokenType Type, int OriginalPos) : Token(Value, Type)
	{
		public static TokenWithPosition CreateOpenBracketToken(int originalPos) => new("(", TokenType.OpenBracket, originalPos);
		public static TokenWithPosition CreateCloseBracketToken(int originalPos) => new(")", TokenType.CloseBracket, originalPos);
		public static TokenWithPosition CreateNumberToken(string number, int originalPos) => new(number, TokenType.Number, originalPos);

		public static TokenWithPosition CreatePlusToken(int originalPos) => new("+", TokenType.Plus, originalPos);
		public static TokenWithPosition CreateMinusToken(int originalPos) => new("-", TokenType.Minus, originalPos);
		public static TokenWithPosition CreateDivideToken(int originalPos) => new("/", TokenType.Divide, originalPos);
		public static TokenWithPosition CreateMultiplyToken(int originalPos) => new("*", TokenType.Multiply, originalPos);
	}
}