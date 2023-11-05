namespace ObjectOpen.Patterns.Solvers
{
    public class Result
    {
        public Result(Flag flag = Flag.OK, string message = "")
        {
            Flag = flag;
            Message = message;
        }
        public Flag Flag { get; set; }
        public string Message { get; set; }
        public void Combine(Result other)
        {
            if (other.Flag > 0)
            {
                if (other.Flag >= Flag)
                {
                    this.Flag = other.Flag;
                    this.Message = $"{(string.IsNullOrEmpty(this.Message) ? "" : this.Message + "; ")}{other.Message}";
                }
            }
        }
        public override string ToString()
        {
            return $"[{Flag}] {Message}";
        }
    }
    public enum Flag
    {
        OK = 0,
        None = -1,
        Warning = 1,
        Error = 2
    }
}
