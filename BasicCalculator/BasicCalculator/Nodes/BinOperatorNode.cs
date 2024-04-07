namespace BasicCalculator.BasicCalculator.Nodes;

public record BinOperatorNode(TokenType Sign, ExpressionNode LeftOperand, ExpressionNode RightOperand) : ExpressionNode;