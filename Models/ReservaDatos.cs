using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Caso1.Models;

public class ReservaDatos(IConfiguration configuration)
{
    private SqlConnection Conexion() => new(configuration.GetConnectionString("CasoEstudio"));

    public async Task<IEnumerable<TipoHabitacion>> ConsultarTipos()
    {
        using var conexion = Conexion();
        return await conexion.QueryAsync<TipoHabitacion>("dbo.ConsultarTiposHabitaciones", commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Reserva>> Consultar()
    {
        using var conexion = Conexion();
        return await conexion.QueryAsync<Reserva>("dbo.ConsultarReservas", commandType: CommandType.StoredProcedure);
    }

    public async Task Registrar(Reserva reserva)
    {
        using var conexion = Conexion();
        await conexion.ExecuteAsync("dbo.RegistrarReserva", new
        {
            NumeroHabitacion = reserva.NumeroHabitacion.Trim(),
            reserva.MontoTotal,
            reserva.TipoHabitacion
        }, commandType: CommandType.StoredProcedure);
    }
}
