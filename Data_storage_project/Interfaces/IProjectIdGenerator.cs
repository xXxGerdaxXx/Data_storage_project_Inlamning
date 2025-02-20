using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Interfaces;

public interface IProjectIdGenerator
{
    Task<string> GenerateProjectIdAsync();
}
