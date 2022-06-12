namespace exceptions
{
    public class BoardException : Exception
    {
        public BoardException(String message) : base(message) {

        }
    }

    public class InvalidPlayerException : BoardException
    {
        public InvalidPlayerException() : base("Not a valid player") {

        }
    }

    public class InvalidColumnException : BoardException
    {
        public InvalidColumnException() : base("Not a valid column choice") {

        }
    }
}
