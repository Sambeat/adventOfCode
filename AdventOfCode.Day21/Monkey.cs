namespace AdventOfCode.Day21;

public class Monkey
{
    public string Name { get; set; }

    public string Operator { get; set; }

    public string LeftName { get; set; }
    public string RightName { get; set; }

    public long? Value { get; set; }

    public Monkey? Left { get; set; }
    public Monkey? Right { get; set; }

    public bool Marked { get; set; }

    public bool AddMonkey(Monkey monkey)
    {
        var result = Left?.AddMonkey(monkey);

        if (result == true)
        {
            return true;
        }

        result = Right?.AddMonkey(monkey);

        if (result == true)
        {
            return true;
        }

        if (Left == null && LeftName == monkey.Name)
        {
            Left = monkey;

            return true;
        }
        else if (Right == null && RightName == monkey.Name)
        {
            Right = monkey;

            return true;
        }

        return false;
    }

    public (long, string) Compute()
    {
        if (Value.HasValue)
        {
            return (Value.Value, Value.Value.ToString());
        }


        // var left = Left?.Compute() ?? Value ?? 0;
        // var right = Right?.Compute() ?? Value ?? 0;

        var left = Left!.Compute();
        var right = Right!.Compute();

        return Operator switch
        {
            "+" => (left.Item1 + right.Item1, (left.Item1 + right.Item1).ToString()),
            "-" => (left.Item1 - right.Item1, (left.Item1 - right.Item1).ToString()),
            "*" => (left.Item1 * right.Item1, (left.Item1 * right.Item1).ToString()),
            "/" => (left.Item1 / right.Item1, (left.Item1 / right.Item1).ToString()),
            "=" => (left.Item1 + right.Item1, $"{left.Item2} {Operator} {right.Item2}"),
            _ => throw new Exception("Unknown operator")
        };
    }

    private void MarkPathTo(string target, bool found = false)
    {
        Left?.MarkPathTo(target, found);
        Right?.MarkPathTo(target, found);

        Marked = Operator == target || Left is { Marked: true } || Right is { Marked: true };
    }

    public void ResolveFor(string target)
    {
        MarkPathTo(target);
        if (!Marked) throw new Exception("Could not find symbol (" + target + ")");

        // Make sure the target is in the left subtree
        if (!Left.Marked)
        {
            // swap sides of equation

            (Left, Right) = (Right, Left);
        }

        var operators = new Dictionary<string, string>
        {
            { "+", "-" },
            { "-", "+" },
            { "*", "/" },
            { "/", "*" }
        };
        var leftOperator = Left?.Operator;
        while (leftOperator != target)
        {
            var toMove = Left?.Left is { Marked: true } ? Left.Right : Left?.Left;
            if (leftOperator is "+" or "*" || (Left?.Right?.Marked ?? false))
            {
                Right = new Monkey
                    { Name = Right?.Name, Left = Right, Right = toMove, Operator = operators[leftOperator] };
            }
            else
            {
                Right = new Monkey { Name = Right?.Name, Left = toMove, Right = Right, Operator = leftOperator };
            }

            Left = Left?.Left is { Marked: true } ? Left.Left : Left?.Right;
            leftOperator = Left?.Operator;
        }
    }

    public string Print()
    {
        if (Operator == "t")
            return "t";
        
        if (Value.HasValue)
            return Value.Value.ToString();

        return $"({Left?.Print()} {Operator} {Right?.Print()})";
    }
}