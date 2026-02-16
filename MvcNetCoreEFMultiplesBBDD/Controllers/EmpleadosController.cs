using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private IRepositoryEmpleados repo;
        
        public EmpleadosController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<DatosEmpleado> empleados = await this.repo.GetDatosEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            DatosEmpleado empleado = await this.repo.FindDatosEmpleadoAsync(id);
            return View(empleado);
        }

        public async Task<IActionResult> Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(string dnombre, string apellido, string oficio, int dir, int salario, int comision)
        {
            int idEmpleado = await this.repo.InsertEmpleado(dnombre, apellido, oficio, dir, salario, comision);
            return RedirectToAction("Details", new {id = idEmpleado});
        }

    }
}
