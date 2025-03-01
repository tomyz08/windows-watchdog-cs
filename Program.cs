using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_watchdog_cs
{
    class Program
    {
        static Process myProcess;
        static Process myInstance;
        static TaskCompletionSource<bool> processExited;
        static TaskCompletionSource<bool> instanceExited;
        static void myProcess_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine(
                $"ID del Proceso      : {myProcess.Id}.\n" +
                $"Nombre del Proceso  : {myProcess.ProcessName}.\n" +
                $"Hora de Cierre      : {myProcess.ExitTime}.\n" +
                $"Codigo de Cierre    : {myProcess.ExitCode}.\n" +
                $"Tiempo Transcurrido : {Math.Round((myProcess.ExitTime - myProcess.StartTime).TotalMilliseconds)}.");
            processExited.TrySetResult(true);
            ReStartProcess(myProcess.ProcessName);
        }
        static async Task ReStartProcess(string processName)
        {
            
            Console.WriteLine("La app/proceso {0} se cerro repentinamente", processName);
            ReStart:
            Console.WriteLine("Desea volver a abrirla? y/n");
            string inputSelection = Console.ReadLine();
            if (inputSelection.ToLower() != "y" && inputSelection.ToLower() != "n")
            {
                Console.WriteLine("Debe Seleccionar una opcion.");
                goto ReStart;
            }
            else if (inputSelection.ToLower() == "y")
            {
                await StartProcess(processName);
                return;
            }
            else
            {
                return;
            }
        }
        static void myInstance_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine(
                $"ID del Proceso      : {myInstance.Id}.\n" +
                $"Nombre del Proceso  : {myInstance.ProcessName}.\n" +
                $"Hora de Cierre      : {myInstance.ExitTime}.\n" +
                $"Codigo de Cierre    : {myInstance.ExitCode}.\n" +
                $"Tiempo Transcurrido : {Math.Round((myInstance.ExitTime - myInstance.StartTime).TotalMilliseconds)}.");
            instanceExited.TrySetResult(true);
        }
        static Process[] GetProcessesByName(string processName)
        {
            Process[] processesByName = Process.GetProcessesByName(processName);
            return processesByName;
        }
        static async Task KillInstances(Process process)
        {
            instanceExited = new TaskCompletionSource<bool>();

            using (myInstance = new Process())
            {
                try
                {
                    Console.WriteLine("Terminando app/proceso {0} - {1}...\n", process.Id, process.ProcessName);
                    myInstance = process;
                    myInstance.EnableRaisingEvents = true;
                    myInstance.Exited += new EventHandler(myInstance_Exited);
                    myInstance.Kill();
                    await myInstance.WaitForExitAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Ocurrio un error terminando la instancia ID \"{myInstance.Id}\" " +
                        $"de la app/proceso \"{myInstance.ProcessName}\":\n{ex.Message}.");
                    return;
                }
            }
            await Task.WhenAny(instanceExited.Task);
        }
        static async Task StartProcess(string processName)
        {
            processExited = new TaskCompletionSource<bool>();

            using (myProcess = new Process())
            {
                try
                {
                    Console.WriteLine("Abriendo app/proceso...");
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
        static async Task IsProcessRunning(string processName)
        {
            Process[] processes = GetProcessesByName(processName);
            if (processes.Length <= 0)
            {
                Console.WriteLine("No hay instancias abiertas de la app/proceso {0}.", processName);
                Start:
                Console.WriteLine("Desea abrir la app/proceso {0}? y/n", processName);
                string inputSelection = Console.ReadLine();
                if (inputSelection.ToLower() != "y" && inputSelection.ToLower() != "n")
                {
                    Console.WriteLine("Debe Seleccionar una opcion.");
                    goto Start;
                }
                else if (inputSelection.ToLower() == "y")
                {
                    await StartProcess(processName);
                    return;
                }
                else
                {
                    return;
                }
            }
            else if (processes.Length == 1)
            {
                Console.WriteLine("La app/proceso {0} esta funcionando.", processName);
                return;
            }
            else
            {
                Console.WriteLine("Hay {0} instancias activas de la app {1}.", processes.Count(), processName);
                Kill:
                Console.WriteLine("Desea terminar las instancias adicionales? y/n");
                string inputSelection = Console.ReadLine();
                if (inputSelection.ToLower() != "y" && inputSelection.ToLower() != "n")
                {
                    Console.WriteLine("Debe Seleccionar una opcion.");
                    goto Kill;
                }
                else if (inputSelection.ToLower() == "y")
                {
                    for (int i = 1; i < processes.Length; i++)
                    {
                        Process process = processes[i];
                        await KillInstances(process);
                    }
                }
                else
                {
                    return;
                }
            }
        }
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new WatchDog());
            /*Input:
            Console.WriteLine("Ingrese el nombre de la app/proceso:");
            string inputApp = Console.ReadLine();
            if (inputApp == "")
            {
                Console.WriteLine("Debe Ingresar un nombre.");
                goto Input;
            }
            await IsProcessRunning(inputApp);*/
        }
    }
}