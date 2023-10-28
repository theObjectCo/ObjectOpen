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
    }
    public enum Flag
    {
        OK = 0,
        None = -1,
        Error = 1
    }
}
