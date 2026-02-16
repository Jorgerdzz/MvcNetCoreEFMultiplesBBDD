using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using System.Data;
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
//go

alter procedure SP_INSERT_EMPLEADOS
(@dnombre nvarchar(50), @apellido nvarchar(50), @oficio nvarchar(50), @dir int, @salario int, @comision int, @idEmpleado int OUT )
as
	declare @idDept int
	declare @fecha DateTime
	select @idDept = DEPT_NO from DEPT where DNOMBRE = @dnombre
	select @idEmpleado = MAX(EMP_NO) from EMP
	set @idEmpleado = @idEmpleado + 1
	set @fecha = GETDATE()
	insert into EMP values(@idEmpleado, @apellido, @oficio, @dir, @fecha, @salario, @comision, @idDept)
go*/
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

        public async Task<int> InsertEmpleado(string dnombre, string apellido, string oficio, int dir, int salario, int comision)
        {
            var idEmpleado = new SqlParameter("@idEmpleado", SqlDbType.Int) { Direction = ParameterDirection.Output };
            string sql = "SP_INSERT_EMPLEADOS @dnombre, @apellido, @oficio, @dir, @salario, @comision, @idEmpleado out";
            SqlParameter pamNombreDept = new SqlParameter("@dnombre", dnombre);
            SqlParameter pamApe = new SqlParameter("@apellido", apellido);
            SqlParameter pamOfi = new SqlParameter("@oficio", oficio);
            SqlParameter pamDir = new SqlParameter("@dir", dir);
            SqlParameter pamSalario = new SqlParameter("@salario", salario);
            SqlParameter pamComision = new SqlParameter("@comision", comision);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamNombreDept, pamApe, pamOfi, pamDir, pamSalario, pamComision, idEmpleado);
            return (int)idEmpleado.Value;
        }

    }
}
