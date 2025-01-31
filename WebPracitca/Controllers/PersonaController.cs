using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebPracitca.Models;

namespace WebPracitca.Controllers
{
    [Route("/[controller]")]
    public class PersonaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApiPruebaClient _apiClient;
        public PersonaController( IConfiguration configuration)
        {
            _configuration = configuration;
            _apiClient = new ApiPruebaClient(_configuration["PruebaApi"], new HttpClient());
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("RegisterView")]
        public IActionResult RegisterView()
        {
            return PartialView("~/Views/Home/_Registro.cshtml");
        }
        [HttpGet("EditView")]
        public IActionResult EditView()
        {
            return PartialView("~/Views/Home/_EditView.cshtml");
        }
        [HttpPost("InsertarPersona")]
        public async Task<JsonResult> InsertarPersona(InsertarPersonaRequest request)
        {
            request.Estatus = true;

            try
            {
                var response = await _apiClient.InsertarAsync(request);
                if (response.Tipo == "Éxito")
                {
                    return Json(new { success = true, message = response.Mensaje });
                }
                else
                {
                    return Json(new { success = false, message = response.Mensaje });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al insertar la persona: {ex.Message}" });
            }
        }

        [HttpPut("EditarPersona")]
        public async Task<JsonResult> EditarPersona(ActualizarPersonaRequest request)
        {
            request.Estatus = true;
            try
            {
                var response = await _apiClient.ActualizarAsync(request);
                if (response.Tipo == "Éxito")
                {
                    return Json(new { success = true, message = response.Mensaje });
                }
                else
                {
                    return Json(new { success = false, message = response.Mensaje });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al editar la persona: {ex.Message}" });
            }
        }
        [HttpPost("EliminarPersona")]
        public async Task<JsonResult> EliminarPersona(int id)
        {
            try
            {
                var response = await _apiClient.EliminarAsync(id);
                if (response.Tipo == "Éxito")
                {
                    return Json(new { success = true, message = response.Mensaje });
                }
                else
                {
                    return Json(new { success = false, message = response.Mensaje });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al editar la persona: {ex.Message}" });
            }
        }

        [HttpPost("ListarPersonas")]
        public async Task<JsonResult> ListarPersonas(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var request = new RequestLista
            {
                Draw = Request.Form["draw"].FirstOrDefault(),
                PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0"),
                Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"),
                Busqueda = Request.Form["search[value]"].FirstOrDefault(),
                ColumnaOrdenamiento = Request.Form["order[0][column]"].FirstOrDefault(),
                Ordenamiento = Request.Form["order[0][dir]"].FirstOrDefault()
            };


            var alumnos = await _apiClient.ListarPorFechaAsync(fechaInicio, fechaFin);

            var gridData = new ResponseGrid<List<PersonaResponse>>
            {
                Data = alumnos.ToList(),
                RecordsTotal = alumnos.Count(),
                RecordsFiltered = alumnos.Count(),
                Draw = request.Draw
            };
            return Json(gridData);
        }
        public class PersonaRequest
        {
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
        }
    }

    internal class RequestLista
    {
        public string Draw { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public string Busqueda { get; set; }
        public string ColumnaOrdenamiento { get; set; }
        public string Ordenamiento { get; set; }
    }

}
