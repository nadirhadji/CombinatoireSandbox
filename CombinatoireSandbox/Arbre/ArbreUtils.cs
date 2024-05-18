namespace CombinatoireSandbox.Arbre
{
    public class ArbreUtils
    {
        public struct Option<T>
        {
            public static Option<T> None { get; } = new Option<T>();

            public T Value { get; }
            public bool HasValue { get; }

            private Option(T value)
            {
                Value = value;
                HasValue = true;
            }

            public static Option<T> Some(T value)
            {
                return new Option<T>(value);
            }
        }
    }
}
