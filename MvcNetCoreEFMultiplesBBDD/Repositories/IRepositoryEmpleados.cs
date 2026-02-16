using Microsoft.Data.SqlClient;
using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<DatosEmpleado>> GetDatosEmpleadosAsync();
        Task<DatosEmpleado> FindDatosEmpleadoAsync(int idEmpleado);
        Task<int> InsertEmpleado(string dnombre, string apellido, string oficio, int dir, int salario, int comision);
    }   
}
