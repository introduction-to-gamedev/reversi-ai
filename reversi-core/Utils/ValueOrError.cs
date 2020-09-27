namespace IntroToGameDev.Reversi.Utils
{
    using System;

    public struct ValueOrError<T>
    {
        public T Value
        {
            get
            {
                if (Error != null)
                {
                    throw new Exception($"Can not fetch value, error {Error} present");
                }

                return value;
            }
        }

        public string Error { get; }

        public bool HasValue => Error == null;

        private readonly T value;

        public ValueOrError(T value, string error)
        {
            this.value = value;
            Error = error;
        }

       
    }

    public struct ValueOrError
    {
        public static ValueOrError<T> FromError<T>(string error)
        {
            return new ValueOrError<T>(default, error);
        }

        public static ValueOrError<T> FromValue<T>(T value)
        {
            return new ValueOrError<T>(value, null);
        }
    }
}