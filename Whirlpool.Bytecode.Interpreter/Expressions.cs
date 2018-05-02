/*
 * Expressions.cs
 * ----------------------------------------
 * Contains the base Expression class, as
 * well as various expressions built on top
 * of it (e.g. AdditionExpression).
 */

namespace Whirlpool.Bytecode.Interpreter
{
    public class Expression
    {
        public virtual object Evaluate()
        {
            return null;
        }
    }

    public class NumberExpression : Expression
    {
        public new virtual double Evaluate()
        {
            return 0;
        }
    }

    public class StringExpression : Expression
    {
        string value;

        public StringExpression(string value)
        {
            this.value = value;
        }

        public new virtual string Evaluate()
        {
            return value;
        }
    }


    public class IntegerExpression : NumberExpression
    {
        int value;

        public IntegerExpression(int value)
        {
            this.value = value;        
        }

        public override double Evaluate()
        {
            return value;
        }
    }

    public class BooleanExpression : Expression
    {
        public new virtual bool Evaluate()
        {
            return false;
        }
    }

    public class BooleanEvaluationExpression : BooleanExpression
    {
        protected NumberExpression left;
        protected NumberExpression right;

        public BooleanEvaluationExpression(NumberExpression left, NumberExpression right)
        {
            this.left = left;
            this.right = right;
        }
    }

    public class EqualToExpression : BooleanEvaluationExpression
    {
        public EqualToExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override bool Evaluate()
        {
            return left.Evaluate() == right.Evaluate();
        }
    }

    public class LessThanExpression : BooleanEvaluationExpression
    {
        public LessThanExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override bool Evaluate()
        {
            return left.Evaluate() < right.Evaluate();
        }
    }

    public class GreaterThanExpression : BooleanEvaluationExpression
    {
        public GreaterThanExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override bool Evaluate()
        {
            return left.Evaluate() > right.Evaluate();
        }
    }

    public class ArithmeticExpression : NumberExpression
    {
        protected NumberExpression left;
        protected NumberExpression right;

        public ArithmeticExpression(NumberExpression left, NumberExpression right)
        {
            this.left = left;
            this.right = right;
        }

    }
    
    public class AdditionExpression : ArithmeticExpression
    {
        public AdditionExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override double Evaluate()
        {
            return left.Evaluate() + right.Evaluate();
        }
    }

    public class SubtractionExpression : ArithmeticExpression
    {
        public SubtractionExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override double Evaluate()
        {
            return left.Evaluate() - right.Evaluate();
        }
    }

    public class MultiplicationExpression : ArithmeticExpression
    {
        public MultiplicationExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override double Evaluate()
        {
            return left.Evaluate() * right.Evaluate();
        }
    }

    public class DivisionExpression : ArithmeticExpression
    {
        public DivisionExpression(NumberExpression left, NumberExpression right) : base(left, right) { }

        public override double Evaluate()
        {
            return left.Evaluate() / right.Evaluate();
        }
    }
}
