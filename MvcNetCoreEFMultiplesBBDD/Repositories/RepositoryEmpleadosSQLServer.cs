using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using System.Diagnostics.Metrics;

#region STORED PROCEDURES
/*SQL SERVER
alter view V_EMPLEADOS
as
	select EMP.EMP_NO as IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO, DEPT.DEPT_NO, DEPT.DNOMBRE, DEPT.LOC
	from EMP
	inner join DEPT 
	on EMP.DEPT_NO = DEPT.DEPT_NO
go

//create procedure SP_ALL_VEMPLEADOS
//as
//	select * from V_EMPLEADOS
//go*/
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosSQLServer: IRepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleadosSQLServer(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<DatosEmpleado>> GetDatosEmpleadosAsync()
        {
            //var consulta = from datos in this.context.Empleados
            //               select datos;
            //return await consulta.ToListAsync();
            string sql = "SP_ALL_VEMPLEADOS";
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
