using Caso1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Caso1.Controllers;

public class ReservasController(ReservaDatos datos, ILogger<ReservasController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        try { return View(await datos.Consultar()); }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Error al consultar reservas");
            ViewBag.Error = "No se pudieron consultar las reservas. Verifique la conexión a la base de datos.";
            return View(Array.Empty<Reserva>());
        }
    }

    public async Task<IActionResult> Registrar()
    {
        await CargarTipos();
        return View(new Reserva());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar([Bind("NumeroHabitacion,MontoTotal,TipoHabitacion")] Reserva reserva)
    {
        if (reserva.MontoTotal.HasValue && decimal.Round(reserva.MontoTotal.Value, 2) != reserva.MontoTotal.Value)
            ModelState.AddModelError("MontoTotal", "El monto admite hasta dos decimales.");
        if (ModelState.IsValid)
        {
            try
            {
                await datos.Registrar(reserva);
                TempData["Mensaje"] = "La reserva se registró correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number is >= 50001 and <= 50004)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Error al registrar reserva");
                ModelState.AddModelError("", "No se pudo guardar la reserva. Verifique la conexión e intente nuevamente.");
            }
        }
        await CargarTipos();
        return View(reserva);
    }

    private async Task CargarTipos()
    {
        try { ViewBag.Tipos = new SelectList(await datos.ConsultarTipos(), "Tipo", "DescripcionTipo"); }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Error al consultar tipos");
            ViewBag.Tipos = new SelectList(Array.Empty<TipoHabitacion>(), "Tipo", "DescripcionTipo");
            ModelState.AddModelError("", "No se pudieron cargar los tipos de habitación. Verifique la conexión a la base de datos.");
        }
    }
}
