using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<DatosEmpleado>> GetDatosEmpleadosAsync();
        Task<DatosEmpleado> FindDatosEmpleadoAsync(int idEmpleado);
    }   
}
