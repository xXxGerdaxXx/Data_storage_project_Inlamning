using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Interfaces;

public interface ILoggerService
{
    void LogError(string message, Exception ex);
    void LogWarning(string message);
    void LogInformation(string message);
}
