namespace Logic
{
    public class Square
    {
        public SquareType Type { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int Value { get; set; }
        
        protected bool Equals(Square other)
        {
            return Column == other.Column && Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Square)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column*397) ^ Row;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
