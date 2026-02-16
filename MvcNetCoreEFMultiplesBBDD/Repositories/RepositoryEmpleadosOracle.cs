using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;

#region STORED PROCEDURES
/*ORACLE
//create or replace view V_EMPLEADOS
//as
//    select EMP.EMP_NO as IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO, DEPT.DEPT_NO, DEPT.DNOMBRE, DEPT.LOC
//    from EMP
//    inner join DEPT 
//    on EMP.DEPT_NO = DEPT.DEPT_NO

create or replace procedure SP_ALL_VEMPLEADOS
(p_cursor_empleados out SYS_REFCURSOR)
as
BEGIN
    open p_cursor_empleados for
    select * from V_EMPLEADOS;
END;    

CREATE OR REPLACE PROCEDURE SP_INSERT_EMPLEADOS (
    dnombre IN DEPT.DNOMBRE%type,
    apellido IN EMP.APELLIDO%type,
    oficio IN EMP.OFICIO%type,
    dir IN EMP.DIR%type,
    salario IN EMP.SALARIO%type,
    comision IN EMP.COMISION%type,
    idEmpleado OUT EMP.EMP_NO%type
)
AS
    idDept NUMBER;
    fecha DATE;
BEGIN
    SELECT DEPT_NO INTO idDept FROM DEPT WHERE DNOMBRE = dnombre;

    SELECT NVL(MAX(EMP_NO), 0) + 1 INTO idEmpleado FROM EMP;

    fecha := SYSDATE;

    INSERT INTO EMP (EMP_NO, APELLIDO, OFICIO, DIR, FECHA, SALARIO, COMISION, DEPT_NO)
    VALUES (idEmpleado, apellido, oficio, dir, fecha, salario, comision, idDept);
END;

 */
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosOracle: IRepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleadosOracle(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<DatosEmpleado>> GetDatosEmpleadosAsync()
        {
            //begin
            //    sp_procedure(:param1, param2)
            //end;
            string sql = "begin ";
            sql += " SP_ALL_VEMPLEADOS (:p_cursor_empleados); ";
            sql += " end;";
            OracleParameter pamCursor = new OracleParameter();
            pamCursor.ParameterName = "p_cursor_empleados";
            pamCursor.Value = null;
            pamCursor.Direction = System.Data.ParameterDirection.Output;
            //INDICAMOS EL TIPO DE ORACLE
            pamCursor.OracleDbType = OracleDbType.RefCursor;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamCursor);
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
            string sql = " begin ";
            sql += " SP_INSERT_EMPLEADOS (:dnombre, :apellido, :oficio, :dir, :salario, :comision, :idEmpleado) ";
            sql += " end;";
            OracleParameter pamNombreDept = new OracleParameter(":dnombre", dnombre);
            OracleParameter pamApe = new OracleParameter(":apellido", apellido);
            OracleParameter pamOfi = new OracleParameter(":oficio", oficio);
            OracleParameter pamDir = new OracleParameter(":dir", dir);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            OracleParameter pamComision = new OracleParameter(":comision", comision);
            OracleParameter pamIdEmpleado = new OracleParameter();
            pamIdEmpleado.ParameterName = "idEmpleado";
            pamIdEmpleado.Value = null;
            pamIdEmpleado.Direction = System.Data.ParameterDirection.Output;
            //INDICAMOS EL TIPO DE ORACLE
            pamIdEmpleado.OracleDbType = OracleDbType.Int32;
            await this.context.Database.ExecuteSqlRawAsync(sql, pamNombreDept, pamApe, pamOfi, pamDir, pamSalario, pamComision, pamIdEmpleado);
            return (int)pamIdEmpleado.Value;
        }

    }
}
