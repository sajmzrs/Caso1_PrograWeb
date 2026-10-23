# Caso de estudio 1

Aplicación ASP.NET Core MVC (.NET 10) para registrar y consultar reservas. Utiliza Bootstrap, Dapper y SQL Server con procedimientos almacenados.

## Preparación

1. Instalar el SDK de .NET 10 y SQL Server.
2. Ejecutar `ScriptCaso1.txt` en SQL Server Management Studio. Crea la base `CasoEstudio`, las tablas originales y los dos tipos de habitación. Ejecutarlo únicamente si la base no existe.
3. Ejecutar `Database/Procedimientos.sql` sobre la misma instancia.
4. Ajustar `ConnectionStrings:CasoEstudio` en `appsettings.json` si el servidor no es `localhost`. La configuración incluida usa autenticación de Windows.
5. Desde esta carpeta ejecutar:

```powershell
dotnet restore
dotnet run --no-launch-profile --urls http://localhost:5080
```

Abrir http://localhost:5080. El menú permite registrar y consultar reservas.

## Funcionamiento

- El registro solicita número de habitación, monto total y un tipo existente.
- La fecha y hora se asignan en SQL Server con `GETDATE()`; no aparecen como campo del formulario.
- Cada tipo admite hasta tres reservas. El procedimiento verifica este límite dentro de una transacción.
- La consulta muestra fecha en formato `dd/MM/yyyy`, número de habitación, descripción del tipo y monto con dos decimales.
- `Funcionamiento.pdf` contiene capturas de la aplicación y sus resultados.

La entrega conserva la estructura de las tablas suministradas. No requiere un proyecto Web API ni migraciones.
