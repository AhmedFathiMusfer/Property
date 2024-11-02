namespace Property_WepAPI.logging
{
    public class Logging : ILogging
    {
        public void log(string message, string type)
        {
            if (type == "Ereor")
            {
                Console.WriteLine("Error" + message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
