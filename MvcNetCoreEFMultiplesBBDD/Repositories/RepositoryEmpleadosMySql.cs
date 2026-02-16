using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Mysqlx.Crud;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Utilities.Zlib;

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

//DELIMITER $$

//CREATE PROCEDURE SP_INSERT_EMPLEADOS(
//    IN dnombre VARCHAR(50),
//    IN apellido VARCHAR(50),
//    IN oficio VARCHAR(50),
//    IN dir INT,
//    IN salario INT,
//    IN comision INT
//)
//BEGIN
//    DECLARE idDept INT;
//DECLARE idEmpleado INT;
//DECLARE fecha DATETIME;

//--Obtener el DEPT_NO usando el DNOMBRE
//    SELECT DEPT_NO INTO idDept FROM DEPT WHERE DNOMBRE = dnombre LIMIT 1;

//--Obtener el siguiente EMP_NO
//    SELECT COALESCE(MAX(EMP_NO), 0) + 1 INTO idEmpleado FROM EMP;

//--Obtener la fecha actual
//    SET fecha = NOW();

//--Insertar los datos en la tabla EMP
//    INSERT INTO EMP (EMP_NO, APELLIDO, OFICIO, DIR, FECHA, SALARIO, COMISION, DEPT_NO)
//    VALUES (idEmpleado, apellido, oficio, dir, fecha, salario, comision, idDept);

//SELECT idEmpleado AS NuevoIDEmpleado;

//END $$

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

        public async Task<int> InsertEmpleado(string dnombre, string apellido, string oficio, int dir, int salario, int comision)
        {
            string sql = "SP_INSERT_EMPLEADOS @dnombre, @apellido, @oficio, @dir, @salario, @comision";
            SqlParameter pamNombreDept = new SqlParameter("@dnombre", dnombre);
            SqlParameter pamApe = new SqlParameter("@apellido", apellido);
            SqlParameter pamOfi = new SqlParameter("@oficio", oficio);
            SqlParameter pamDir = new SqlParameter("@dir", dir);
            SqlParameter pamSalario = new SqlParameter("@salario", salario);
            SqlParameter pamComision = new SqlParameter("@comision", comision);
            int idEmpleado = await this.context.Database.ExecuteSqlRawAsync(sql, pamNombreDept, pamApe, pamOfi, pamDir, pamSalario, pamComision);
            return idEmpleado;
        }

    }
}
