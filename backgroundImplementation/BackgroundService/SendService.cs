namespace backgroundImplementation.BackgroundService
{
    public class SendService : IsendService
    {
        public void DeleteItem()
        {
            Console.WriteLine($"Item is deleting {DateTime.Now}");
        }

        public void SendEmail()
        {
            Console.WriteLine($"Email is sending {DateTime.Now}");

        }

        public void updateDatabase()
        {
            Console.WriteLine($"update  {DateTime.Now}");
        }
    }
}
