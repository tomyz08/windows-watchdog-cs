using System;
using System.ComponentModel;
using System.Diagnostics;
using windows_watchdog_cs.Models;

namespace windows_watchdog_cs.Services
{
    public class ProcessService
    {
        public async Task<List<MonitoredProcess>> GetAllProcesses()
        {
            return await Task.Run(() =>
            {
                return Process.GetProcesses()
                .Select(p => new MonitoredProcess
                {
                    ProcessName = p.ProcessName,
                    IsMonitored = false
                }).ToList();
            });
        }

        public async Task<string> GetProcessFilePath(string processName, Action<string> logMessage)
        {
            return await Task.Run(() =>
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    try
                    {
                        return processes[0].MainModule.FileName;
                    }
                    catch (Win32Exception) 
                    {
                        return "Acceso denegado";
                    }
                    catch (Exception ex) 
                    {
                        logMessage($"Error obteniendo la ruta de {processName}: {ex.Message}");
                        return "Acceso denegado";
                    }
                }
                return "Proceso no encontrado";
            });
        }

        public async Task StartProcess(string filePath, Action<string> logMessage)
        {
            try
            {
                if (filePath == "Acceso denegado")
                {
                    logMessage("No se puede iniciar el proceso porque el acceso este esta restringido.");
                    return;
                }
                if (!File.Exists(filePath))
                {
                    logMessage($"El archivo \"{filePath}\" no existe. No se puede iniciar el proceso.");
                    return;
                }
                await Task.Run(() =>
                {
                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = filePath,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                });
                logMessage($"{Path.GetFileName(filePath)} se ha iniciado correctamente.");
            }
            catch (Exception e)
            {
                logMessage($"Error iniciando {filePath}: {e.Message}");
            }
        }

        public  async Task KillProcess(Process process)
        {
            process.Kill();
            await process.WaitForExitAsync();
        }

        public async Task KillAdditionalInstances(Process[] instances, string processName, Action<string> logMessage)
        {
            for (int i = 1; i < instances.Length; i++)
            {
                try
                {
                    await KillProcess(instances[i]);
                }
                catch (Exception e)
                {
                    logMessage($"Error terminando instancias adicionales de {processName}: {e.Message}");
                }
            }
            logMessage($"Instancias adicionales de {processName} terminadas.");
        }
    }
}
