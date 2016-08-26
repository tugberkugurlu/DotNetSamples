using System;

namespace RazorOnConsole.Views
{
    public class AttributeValue
    {
        public AttributeValue(Tuple<string, int> prefix, Tuple<object, int> value, bool literal)
        {
            Prefix = prefix;
            Value = value;
            Literal = literal;
        }

        public Tuple<string, int> Prefix { get; private set; }

        public Tuple<object, int> Value { get; private set; }

        public bool Literal { get; private set; }

        public static AttributeValue FromTuple(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
        {
            return new AttributeValue(value.Item1, value.Item2, value.Item3);
        }

        public static AttributeValue FromTuple(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
        {
            return new AttributeValue(value.Item1, new Tuple<object, int>(value.Item2.Item1, value.Item2.Item2), value.Item3);
        }

        public static implicit operator AttributeValue(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
        {
            return FromTuple(value);
        }

        public static implicit operator AttributeValue(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
        {
            return FromTuple(value);
        }
    }
}