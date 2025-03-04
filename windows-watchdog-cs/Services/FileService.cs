using System.Text.Json;
using windows_watchdog_cs.Models;

namespace windows_watchdog_cs.Services
{
    public class FileService
    {
        private readonly string configFile;

        public FileService(string filePath)
        {
            configFile = filePath;
        }

        public async Task SaveMonitoredProcesses(List<MonitoredProcess> monitoredProcesses, Action<string> logMessage)
        {
            try
            {
                string directory = Path.GetDirectoryName(configFile);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                await using FileStream writeStream = File.Create(configFile);
                await JsonSerializer.SerializeAsync(writeStream, monitoredProcesses, new JsonSerializerOptions { WriteIndented = true });
                logMessage("Lista de procesos monitoreados guardada correctamente.");
            }
            catch (Exception ex)
            {
                logMessage($"Error guardando configuracion: {ex.Message}");
            }
        }

        public async Task<List<MonitoredProcess>> LoadMonitoredProcesses(Action<string> logMessage)
        {
            if (!File.Exists(configFile))
            {
                logMessage("No se encontro un archivo de configuracion. Se creará un nuevo archivo.");
                return new List<MonitoredProcess>();
            }
            try
            {
                using FileStream openStream = File.OpenRead(configFile);
                return await JsonSerializer.DeserializeAsync<List<MonitoredProcess>>(openStream);                
            }
            catch (Exception ex)
            {
                logMessage($"Error leyendo configuracion: {ex.Message}");
                return new List<MonitoredProcess>();

            }
        }
    }
}
