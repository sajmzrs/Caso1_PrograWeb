USE [CasoEstudio]
GO
CREATE OR ALTER PROCEDURE dbo.ConsultarTiposHabitaciones
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TipoHabitacion AS Tipo, DescripcionTipo
    FROM dbo.TiposHabitaciones ORDER BY TipoHabitacion;
END
GO
CREATE OR ALTER PROCEDURE dbo.ConsultarReservas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.Consecutivo, r.NumeroHabitacion, r.FechaReserva, r.MontoTotal,
           r.TipoHabitacion, t.DescripcionTipo
    FROM dbo.Reservas r
    INNER JOIN dbo.TiposHabitaciones t ON t.TipoHabitacion = r.TipoHabitacion
    ORDER BY r.FechaReserva DESC, r.Consecutivo DESC;
END
GO
CREATE OR ALTER PROCEDURE dbo.RegistrarReserva
    @NumeroHabitacion varchar(10),
    @MontoTotal decimal(10,2),
    @TipoHabitacion int
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NULLIF(LTRIM(RTRIM(@NumeroHabitacion)), '') IS NULL
        THROW 50001, 'Indique el número de habitación.', 1;
    IF @MontoTotal IS NULL OR @MontoTotal <= 0
        THROW 50002, 'El monto total debe ser mayor que cero.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM dbo.TiposHabitaciones WITH (UPDLOCK, HOLDLOCK)
                       WHERE TipoHabitacion = @TipoHabitacion)
            THROW 50003, 'Seleccione un tipo de habitación existente.', 1;
        IF (SELECT COUNT(*) FROM dbo.Reservas WHERE TipoHabitacion = @TipoHabitacion) >= 3
            THROW 50004, 'No se pueden registrar más de 3 reservas por tipo de habitación.', 1;
        INSERT dbo.Reservas (NumeroHabitacion, FechaReserva, MontoTotal, TipoHabitacion)
        VALUES (LTRIM(RTRIM(@NumeroHabitacion)), GETDATE(), @MontoTotal, @TipoHabitacion);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
