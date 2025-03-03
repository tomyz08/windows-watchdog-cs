namespace windows_watchdog_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new WatchDog());
        }
    }
}