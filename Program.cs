using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace windows_watchdog_cs
{
    class Program
    {
        private Process myProcess;
        private TaskCompletionSource<bool> processExited;

        public async Task StartProcess(string processName)
        {
            processExited = new TaskCompletionSource<bool>();

            using (myProcess = new Process())
            {
                try
                {
                    myProcess.StartInfo.FileName = processName + ".exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.EnableRaisingEvents = true;
                    myProcess.Exited += new EventHandler(myProcess_Exited);
                    myProcess.Start();
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Ocurrio un error iniciando la app/proceso \"{processName}\":\n{ex.Message}");
                    return;
                }
                await Task.WhenAny(processExited.Task);
            }
        }
        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine(
                $"\n" +
                $"ID del Proceso      : {myProcess.Id}\n" +
                $"Nombre del Proceso  : {myProcess.ProcessName}\n" +
                $"Hora de Cierre      : {myProcess.ExitTime}\n" +
                $"Codigo de Cierre    : {myProcess.ExitCode}\n" +
                $"Tiempo Transcurrido : {Math.Round((myProcess.ExitTime - myProcess.StartTime).TotalMilliseconds)}");
            processExited.TrySetResult(true);
        }
        public Process[] GetProcessesByName(string processName)
        {
            Process[] processesByName = Process.GetProcessesByName(processName);
            Console.WriteLine("Hay {0} instancias activas de la app {1}", processesByName.Count(), processName);
            return processesByName;
        }
        public async Task KillAdditionalInstances(string processName)
        {
            Process[] processesByName = GetProcessesByName(processName);
            Console.WriteLine("Desea terminar las instancias adicionales? y/n");
            string inputSelection = Console.ReadLine();
            if(inputSelection.ToLower() == "y")
            {
                for (int i = 1; i < processesByName.Length; i++)
                {
                    processExited = new TaskCompletionSource<bool>();
                    
                    using (myProcess = new Process())
                    {
                        try
                        {
                            myProcess = processesByName[i];
                            myProcess.EnableRaisingEvents = true;
                            myProcess.Kill();
                            myProcess.Exited += new EventHandler(myProcess_Exited);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Ocurrio un error terminando la instancia ID \"{processesByName[i].Id}\" " +
                                $"de la app/proceso \"{processesByName[i].ProcessName}\":\n{ex.Message}");
                            return;
                        }
                    }
                    await Task.WhenAny(processExited.Task);
                }                
            }
        }
        public static async Task Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Console.WriteLine("Ingrese el nombre de la app/proceso:");
            string inputApp = Console.ReadLine();
            if (inputApp == "")
            {
                Console.WriteLine("Ingrese un nombre.");
                return;
            }
            Program myProcess = new Program();
            await myProcess.KillAdditionalInstances(inputApp);
            //await myProcess.StartProcess(inputApp);
            Console.ReadKey();
        }
    }
}