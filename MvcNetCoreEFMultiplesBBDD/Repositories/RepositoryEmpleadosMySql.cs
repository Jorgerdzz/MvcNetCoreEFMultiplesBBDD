using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

#region
//create view V_EMPLEADOS
//as
//	select EMP.EMP_NO as IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO, DEPT.DEPT_NO, DEPT.DNOMBRE, DEPT.LOC
//    from EMP
//    inner join DEPT 
//    on EMP.DEPT_NO = DEPT.DEPT_NO;

//DELIMITER $$

//create procedure SP_ALL_VEMPLEADOS()
//begin
//	select * from V_EMPLEADOS;
//end $$
//DELIMITER ;
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosMySql: IRepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleadosMySql(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<DatosEmpleado>> GetDatosEmpleadosAsync()
        {
            //var consulta = from datos in this.context.Empleados
            //               select datos;
            //return await consulta.ToListAsync();
            string sql = "call SP_ALL_VEMPLEADOS()";
            var consulta = this.context.Empleados.FromSqlRaw(sql);
            List<DatosEmpleado> data = await consulta.ToListAsync();
            return data;
        }

        public async Task<DatosEmpleado> FindDatosEmpleadoAsync(int idEmpleado)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdEmpleado == idEmpleado
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }


    }
}
