USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'App_Logistica_Inventario')
BEGIN
    ALTER DATABASE App_Logistica_Inventario SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE App_Logistica_Inventario;
END
GO

CREATE DATABASE App_Logistica_Inventario;
GO

USE App_Logistica_Inventario;
GO

-- TABLAS SIN DEPENDENCIAS
CREATE TABLE Rol(
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL
);

CREATE TABLE Categoria(
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(150),
);

CREATE TABLE Proveedor(
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
    NombreProveedor VARCHAR(100) NOT NULL,
    RUC VARCHAR(20) UNIQUE NOT NULL,
    Telefono VARCHAR(20),
    PaginaWeb VARCHAR(100),
    EmailEmpresa VARCHAR(100),
    ProductoOfrecido VARCHAR(100),
    Estado BIT DEFAULT 1
);

-- TABLAS DEPENDIENTES
CREATE TABLE Usuario(
    IdUsuario INT IDENTITY (1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(40) NOT NULL,
    Documento VARCHAR(20) NOT NULL,
    Telefono VARCHAR(30),
    Email VARCHAR(50) UNIQUE NOT NULL,
    Contraseña VARCHAR(100) NOT NULL, 
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Estado BIT DEFAULT 1,
    IdRol INT NOT NULL FOREIGN KEY REFERENCES Rol(IdRol)
);

CREATE TABLE Producto(
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto VARCHAR(100) NOT NULL,
    Fotografia VARCHAR(255) NOT NULL,
    Codigo VARCHAR(20) UNIQUE NOT NULL,
    CostoObtenido DECIMAL(10,2) NOT NULL,
    PrecioVendido DECIMAL(10,2) NOT NULL,
    StockActual INT DEFAULT 0,
    StockMinimo INT DEFAULT 10,
    Estado BIT DEFAULT 1,
    IdCategoria INT NOT NULL FOREIGN KEY REFERENCES Categoria(IdCategoria),
    IdProveedor INT NOT NULL FOREIGN KEY REFERENCES Proveedor(IdProveedor)
);

CREATE TABLE MovimientosStock(
    IdMovimiento INT IDENTITY(1,1) PRIMARY KEY,
    TipoDeMovimiento VARCHAR(20) NOT NULL CHECK (TipoDeMovimiento IN ('ENTRADA', 'SALIDA')),
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    Motivo VARCHAR(100),
    FechaMovimiento DATETIME DEFAULT GETDATE(),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES Producto(IdProducto),
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario)
);

CREATE TABLE Venta(
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    Cliente VARCHAR(60),
    DocumentoCliente VARCHAR(20),
    TelefonoCliente VARCHAR(20),
    FechaVenta DATETIME DEFAULT GETDATE(),
    MetodoPago VARCHAR(30),
    Total DECIMAL(12,2) NOT NULL,
    Estado BIT DEFAULT 1,
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario)
);

CREATE TABLE DetalleVenta(
    IdDetalleVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL FOREIGN KEY REFERENCES Venta(IdVenta),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES Producto(IdProducto),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    SubTotal AS (Cantidad * PrecioUnitario)
);

CREATE TABLE DetalleDescuento(
    IdDetalleDescuento INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL FOREIGN KEY REFERENCES Venta(IdVenta),
    IdDescuento INT NOT NULL FOREIGN KEY REFERENCES Descuento(IdDescuento),
    PorcentajeAplicado DECIMAL(5,2) NOT NULL 
);

CREATE TABLE Descuento(
    IdDescuento INT IDENTITY(1,1) PRIMARY KEY,
    NombreDescuento VARCHAR(100) NOT NULL,
    IdProducto INT NULL FOREIGN KEY REFERENCES Producto(IdProducto),
    TipoDescuento VARCHAR(30) NOT NULL CHECK (TipoDescuento IN ('Total', 'Por producto')),
    PorcentajeDescuento DECIMAL(5,2) NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    ColorCard VARCHAR(30) NULL DEFAULT 'White',
    Estado BIT DEFAULT 1
);

------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE Usuario
------------------------------------------------------------------------------
GO
CREATE PROC sp_Registro_Usuario
@NombreUsuario VARCHAR(40),
@IdRol INT,
@Documento VARCHAR(20),
@Telefono VARCHAR(30),
@Email VARCHAR(50),
@Contraseña VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Usuario (NombreUsuario, IdRol, Documento, Telefono, Email, Contraseña, FechaCreacion, Estado)
    VALUES (@NombreUsuario, @IdRol, @Documento, @Telefono, @Email, @Contraseña, GETDATE(), 1)
END;

GO
CREATE PROC sp_Actualizar_Usuario
@IdUsuario INT,
@NombreUsuario VARCHAR(40),
@IdRol INT,
@Documento VARCHAR(20),
@Telefono VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario
    SET NombreUsuario = @NombreUsuario,
        IdRol = @IdRol,
        Documento = @Documento,
        Telefono = @Telefono
        WHERE IdUsuario = @IdUsuario
END

GO
CREATE PROC sp_Login_Usuario
@Email VARCHAR(50),
@Contraseña VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
    u.IdUsuario,
    u.NombreUsuario,
    r.NombreRol,
    u.Documento,
    u.Telefono,
    u.Estado
    FROM Usuario u
    INNER JOIN Rol r ON u.IdRol = r.IdRol
    WHERE u.Email = @Email AND u.Contraseña = @Contraseña 
END

GO
CREATE PROC sp_Listado_Usuario
@Busqueda VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    u.IdUsuario,
    u.NombreUsuario,
    r.NombreRol,
    u.Email,
    u.FechaCreacion,
    u.Estado
    FROM Usuario u
    INNER JOIN Rol r ON u.IdRol = r.IdRol
    WHERE (@Busqueda IS NULL OR u.NombreUsuario LIKE '%' + @Busqueda + '%'
    OR u.Email LIKE '%' + @Busqueda + '%')
    ORDER BY u.Estado DESC, u.NombreUsuario ASC
END

GO
CREATE PROC sp_Detalle_Usuario
@IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    u.IdUsuario,
    u.NombreUsuario,
    u.IdRol,
    r.NombreRol,
    u.Documento,
    u.Telefono,
    u.Email,
    u.FechaCreacion,
    u.Estado
    FROM Usuario u
    INNER JOIN Rol r ON u.IdRol = r.IdRol
    WHERE IdUsuario = @IdUsuario
END


GO
CREATE PROC sp_CambiarEstado_Usuario
@IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Usuario
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdUsuario = @IdUsuario
END

GO
------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE Producto
------------------------------------------------------------------------------

CREATE PROC sp_InsertarProducto
@NombreProducto VARCHAR(100),
@Fotografia VARCHAR(255),
@Codigo VARCHAR(20),
@CostoObtenido DECIMAL(10,2),
@PrecioVendido DECIMAL(10,2),
@IdCategoria INT,
@IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Producto (NombreProducto, Fotografia, Codigo, CostoObtenido, PrecioVendido, StockActual, StockMinimo, Estado, IdCategoria, IdProveedor)
    VALUES (@NombreProducto, @Fotografia, @Codigo, @CostoObtenido, @PrecioVendido, 0, 10, 1, @IdCategoria, @IdProveedor)
END

GO
CREATE PROC sp_ActualizarProducto
@IdProducto INT,
@NombreProducto VARCHAR(100),
@Fotografia VARCHAR(255),
@CostoObtenido DECIMAL(10,2),
@PrecioVendido DECIMAL(10,2),
@IdCategoria INT,
@IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON; 
    UPDATE Producto 
    SET NombreProducto = @NombreProducto,
    Fotografia = @Fotografia,
    CostoObtenido = @CostoObtenido,
    PrecioVendido = @PrecioVendido,
    IdCategoria = @IdCategoria,
    IdProveedor = @IdProveedor
    WHERE IdProducto = @IdProducto
END

GO
CREATE PROC sp_Listado_Producto
@Busqueda VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
    p.IdProducto,
    p.NombreProducto,
    p.Fotografia,
    c.NombreCategoria,
    p.Codigo,
    p.PrecioVendido,
    p.StockActual,
    p.Estado
    FROM Producto p
    INNER JOIN Categoria c ON p.IdCategoria = c.IdCategoria
    WHERE (@Busqueda IS NULL OR p.NombreProducto LIKE '%' + @Busqueda + '%'
    OR p.Codigo LIKE '%' + @Busqueda + '%')
    ORDER BY p.Estado DESC
END

GO
CREATE PROC sp_Detalle_Producto
@IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
    p.IdProducto,
    p.NombreProducto,
    p.Fotografia,
    c.NombreCategoria,
    p.Codigo,
    p.IdCategoria,
    p.IdProveedor,
    p.CostoObtenido,
    p.PrecioVendido,
    pr.NombreProveedor,
    p.StockActual,
    p.Estado
    FROM Producto p
    INNER JOIN Categoria c ON p.IdCategoria = c.IdCategoria
    INNER JOIN Proveedor pr ON p.IdProveedor = pr.IdProveedor
    WHERE IdProducto = @IdProducto
END

GO
CREATE PROC sp_CambiarEstado_Producto
@IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Producto
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdProducto = @IdProducto
END

GO
CREATE PROC sp_Historial_MovimientosPorProducto
@IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    m.IdMovimiento,
    m.TipoDeMovimiento,
    m.Cantidad,
    m.Motivo,
    m.FechaMovimiento,
    u.NombreUsuario
    FROM MovimientosStock m
    INNER JOIN Usuario u ON m.IdUsuario = u.IdUsuario
    WHERE m.IdProducto = @IdProducto
    ORDER BY m.FechaMovimiento DESC
END

GO
CREATE PROC sp_Listado_Producto_ConFiltro
@Busqueda VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
    p.IdProducto,
    p.NombreProducto,
    p.Fotografia,
    c.NombreCategoria,
    p.Codigo,
    p.PrecioVendido,
    p.StockActual,
    p.Estado
    FROM Producto p
    INNER JOIN Categoria c ON p.IdCategoria = c.IdCategoria
    WHERE (@Busqueda IS NULL OR p.NombreProducto LIKE '%' + @Busqueda + '%'
    OR p.Codigo LIKE '%' + @Busqueda + '%') AND p.Estado = 1
END

GO
------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE MovimientosStock
------------------------------------------------------------------------------
CREATE PROC sp_RegistrarMovimiento
@TipoMovimiento VARCHAR(20),
@Cantidad INT,
@Motivo VARCHAR(100),
@IdProducto INT,
@IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION
        BEGIN TRY
            IF @TipoMovimiento = 'SALIDA'
            BEGIN
                DECLARE @StockActual INT
                SELECT @StockActual = StockActual FROM Producto WHERE IdProducto = @IdProducto
                IF @StockActual < @Cantidad
                BEGIN
                    ROLLBACK
                    RAISERROR('Stock insuficiente', 16, 1)
                    RETURN
                END
            END

            INSERT INTO MovimientosStock(TipoDeMovimiento, Cantidad, Motivo, FechaMovimiento, IdProducto, IdUsuario)
            VALUES (@TipoMovimiento, @Cantidad, @Motivo, GETDATE(), @IdProducto, @IdUsuario)

            UPDATE Producto
            SET StockActual = CASE 
                WHEN @TipoMovimiento = 'ENTRADA' THEN StockActual + @Cantidad
                WHEN @TipoMovimiento = 'SALIDA' THEN StockActual - @Cantidad
            END
            WHERE IdProducto = @IdProducto

        COMMIT
        END TRY
        BEGIN CATCH
            ROLLBACK
            RAISERROR('Error al registrar movimiento', 16, 1)
        END CATCH
END

GO
CREATE PROC sp_Filtrar_Movimientos
@TipoMovimiento VARCHAR(20) = NULL,
@Busqueda VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    m.IdMovimiento,
    p.NombreProducto,
    m.TipoDeMovimiento,
    m.Cantidad,
    m.Motivo,
    m.FechaMovimiento,
    u.NombreUsuario
    FROM MovimientosStock m
    INNER JOIN Producto p ON m.IdProducto = p.IdProducto
    INNER JOIN Usuario u ON m.IdUsuario = u.IdUsuario
    WHERE (@TipoMovimiento IS NULL OR m.TipoDeMovimiento = @TipoMovimiento)
    AND (@Busqueda IS NULL OR p.NombreProducto LIKE '%' + @Busqueda + '%')
    ORDER BY m.FechaMovimiento DESC
END


GO
------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE Venta
------------------------------------------------------------------------------
CREATE TYPE TVP_DetalleVenta AS TABLE(
    IdProducto INT,
    Cantidad INT
);
CREATE TYPE TVP_DetalleDescuento AS TABLE(
    IdDescuento INT NULL,
    PorcentajeAplicado DECIMAL(5,2) NULL
);
GO 
CREATE PROC sp_RegistrarVenta
@Cliente VARCHAR(60),
@DocumentoCliente VARCHAR(20),
@TelefonoCliente VARCHAR(20),
@MetodoPago VARCHAR(30),
@IdUsuario INT,
@Detalle TVP_DetalleVenta READONLY,
@Descuento TVP_DetalleDescuento READONLY 
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IdVenta INT;
    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM @Detalle)
        BEGIN
            RAISERROR('La venta no contiene productos', 16, 1);
            ROLLBACK;
            RETURN;
        END

        IF EXISTS(
            SELECT 1
            FROM @Detalle d
            INNER JOIN Producto p ON p.IdProducto = d.IdProducto
            WHERE p.Estado = 0 OR p.StockActual < d.Cantidad
        )
        BEGIN
            RAISERROR('Stock insuficiente o producto inactivo', 16, 1);
            ROLLBACK;
            RETURN;
        END

        INSERT INTO Venta(Cliente, DocumentoCliente, TelefonoCliente, MetodoPago, FechaVenta, Total, Estado, IdUsuario)
        VALUES(@Cliente, @DocumentoCliente, @TelefonoCliente, @MetodoPago, GETDATE(), 0, 1, @IdUsuario);
        SET @IdVenta = SCOPE_IDENTITY();

        INSERT INTO DetalleVenta(IdVenta, IdProducto, Cantidad, PrecioUnitario)
        SELECT
            @IdVenta,
            p.IdProducto,
            d.Cantidad,
            p.PrecioVendido
        FROM @Detalle d
        INNER JOIN Producto p ON p.IdProducto = d.IdProducto;

        UPDATE DetalleVenta
        SET PrecioUnitario = PrecioUnitario * (1 -(
            SELECT ISNULL(SUM(PorcentajeAplicado), 0) / 100
            FROM @Descuento de
            INNER JOIN Descuento d ON d.IdDescuento = de.IdDescuento
            WHERE d.TipoDescuento = 'Por producto' AND d.IdProducto = IdProducto
        ))
        FROM DetalleVenta dv
        WHERE dv.IdVenta = @IdVenta
        AND EXISTS(SELECT 1 FROM @Descuento de
                   INNER JOIN Descuento d ON d.IdDescuento = de.IdDescuento
                   WHERE d.TipoDescuento = 'Por producto'
        )

        INSERT INTO DetalleDescuento(IdVenta, IdDescuento, PorcentajeAplicado)
        SELECT @IdVenta, de.IdDescuento, de.PorcentajeAplicado
        FROM @Descuento de
        WHERE de.IdDescuento IS NOT NULL

        UPDATE p
        SET p.StockActual = p.StockActual - d.Cantidad
        FROM Producto p
        INNER JOIN @Detalle d ON p.IdProducto = d.IdProducto;

        INSERT INTO MovimientosStock(TipoDeMovimiento, Cantidad, Motivo, FechaMovimiento, IdProducto, IdUsuario)
        SELECT
            'SALIDA',
            d.Cantidad,
            'Venta #' + CAST(@IdVenta AS VARCHAR),
            GETDATE(),
            d.IdProducto,
            @IdUsuario
        FROM @Detalle d;

        UPDATE Venta
        SET Total = (
            SELECT SUM(SubTotal)
            FROM DetalleVenta
            WHERE IdVenta = @IdVenta
        )
        WHERE IdVenta = @IdVenta;

        UPDATE Venta 
        SET Total = Total * (1 - (
            SELECT ISNULL(SUM(de.PorcentajeAplicado), 0) / 100
            FROM @Descuento de
            INNER JOIN Descuento d ON d.IdDescuento = de.IdDescuento
            WHERE d.TipoDescuento = 'Total'
        ))
        WHERE IdVenta = @IdVenta
        AND EXISTS(SELECT 1 FROM @Descuento de
                   INNER JOIN Descuento d ON d.IdDescuento = de.IdDescuento
                   WHERE d.TipoDescuento = 'Total'
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END

GO
CREATE PROC sp_Detalle_Venta
@IdVenta INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    v.IdVenta,
    v.Cliente,
    v.DocumentoCliente,
    v.TelefonoCliente,
    v.MetodoPago,
    v.FechaVenta,
    v.Total,
    v.Estado,
    u.NombreUsuario
    FROM Venta v
    INNER JOIN Usuario u ON v.IdUsuario = u.IdUsuario
    WHERE v.IdVenta = @IdVenta

    SELECT
    p.NombreProducto,
    p.Codigo,
    dv.Cantidad,
    dv.PrecioUnitario,
    dv.SubTotal
    FROM DetalleVenta dv
    INNER JOIN Producto p ON dv.IdProducto = p.IdProducto
    WHERE dv.IdVenta = @IdVenta

    SELECT
    d.NombreDescuento,
    d.TipoDescuento,
    dd.PorcentajeAplicado
    FROM DetalleDescuento dd
    INNER JOIN Descuento d ON d.IdDescuento = dd.IdDescuento
    WHERE dd.IdVenta = @IdVenta
END

GO
CREATE PROC sp_AnularVenta
@IdVenta INT,
@IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE Venta SET Estado = 0
        WHERE IdVenta = @IdVenta AND Estado = 1

        INSERT INTO MovimientosStock(TipoDeMovimiento, Cantidad, Motivo, FechaMovimiento, IdProducto, IdUsuario)
        SELECT
            'ENTRADA',
            dv.Cantidad,
            'Anulación Venta #' + CAST(@IdVenta AS VARCHAR),
            GETDATE(),
            dv.IdProducto,
            @IdUsuario
        FROM DetalleVenta dv
        WHERE dv.IdVenta = @IdVenta;

        UPDATE p
        SET p.StockActual = p.StockActual + dv.Cantidad
        FROM Producto p
        INNER JOIN DetalleVenta dv ON p.IdProducto = dv.IdProducto
        WHERE dv.IdVenta = @IdVenta;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END

GO
CREATE PROC sp_Filtrar_Ventas
@Busqueda VARCHAR(100) = NULL,
@NombreUsuario VARCHAR(40) = NULL,
@Estado BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    v.IdVenta,
    v.IdUsuario,
    v.Cliente,
    v.FechaVenta,
    v.Total,
    v.Estado,
    u.NombreUsuario
    FROM Venta v
    INNER JOIN Usuario u ON v.IdUsuario = u.IdUsuario
    WHERE (@Busqueda IS NULL OR v.Cliente LIKE '%' + @Busqueda + '%')
    AND (@NombreUsuario IS NULL OR u.NombreUsuario LIKE '%' + @NombreUsuario + '%')
    AND (@Estado IS NULL OR v.Estado = @Estado)
    ORDER BY v.FechaVenta DESC
END

GO
------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE Categoria
------------------------------------------------------------------------------
CREATE PROC sp_Insertar_Categoria
@NombreCategoria VARCHAR(50),
@Descripcion VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Categoria(NombreCategoria, Descripcion)
    VALUES(@NombreCategoria, @Descripcion)
END

GO
CREATE PROC sp_Actualizar_Categoria
@IdCategoria INT,
@NombreCategoria VARCHAR(50),
@Descripcion VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Categoria
    SET NombreCategoria = @NombreCategoria,
        Descripcion = @Descripcion
    WHERE IdCategoria = @IdCategoria
END

GO
CREATE PROC sp_Listado_Categoria
@Busqueda VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    IdCategoria,
    NombreCategoria,
    Descripcion
    FROM Categoria
    WHERE (@Busqueda IS NULL OR NombreCategoria LIKE '%' + @Busqueda + '%')
    ORDER BY NombreCategoria ASC
END

GO
------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE Proveedor
------------------------------------------------------------------------------
CREATE PROC sp_Insertar_Proveedor
@NombreProveedor VARCHAR(100),
@RUC VARCHAR(20),
@Telefono VARCHAR(20),
@PaginaWeb VARCHAR(100),
@EmailEmpresa VARCHAR(100),
@ProductoOfrecido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Proveedor(NombreProveedor, RUC, Telefono, PaginaWeb, EmailEmpresa, ProductoOfrecido, Estado)
    VALUES(@NombreProveedor, @RUC, @Telefono, @PaginaWeb, @EmailEmpresa, @ProductoOfrecido, 1)
END

GO
CREATE PROC sp_Actualizar_Proveedor
@IdProveedor INT,
@NombreProveedor VARCHAR(100),
@RUC VARCHAR(20),
@Telefono VARCHAR(20),
@PaginaWeb VARCHAR(100),
@EmailEmpresa VARCHAR(100),
@ProductoOfrecido VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Proveedor
    SET NombreProveedor = @NombreProveedor,
        RUC = @RUC,
        Telefono = @Telefono,
        PaginaWeb = @PaginaWeb,
        EmailEmpresa = @EmailEmpresa,
        ProductoOfrecido = @ProductoOfrecido
    WHERE IdProveedor = @IdProveedor
END

GO
CREATE PROC sp_Detalle_Proveedor
@IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    IdProveedor,
    NombreProveedor,
    RUC,
    Telefono,
    PaginaWeb,
    EmailEmpresa,
    ProductoOfrecido,
    Estado
    FROM Proveedor
    WHERE IdProveedor = @IdProveedor
END

GO
CREATE PROC sp_Listado_Proveedor
@Busqueda VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    IdProveedor,
    NombreProveedor,
    Telefono,
    EmailEmpresa,
    ProductoOfrecido,
    Estado
    FROM Proveedor
    WHERE (@Busqueda IS NULL OR NombreProveedor LIKE '%' + @Busqueda + '%')
    ORDER BY Estado DESC, NombreProveedor ASC
END

GO
CREATE PROC sp_Listado_ProveedorConFiltro
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    IdProveedor,
    NombreProveedor,
    Telefono,
    EmailEmpresa,
    ProductoOfrecido,
    Estado
    FROM Proveedor
    WHERE Estado = 1
    ORDER BY NombreProveedor ASC
END

GO
CREATE PROC sp_CambiarEstado_Proveedor
@IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Proveedor
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdProveedor = @IdProveedor
END

------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS del InicioAdministrador
------------------------------------------------------------------------------
GO
CREATE PROC sp_VentasHoy
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
    COUNT(*) AS TotalVentas,
    ISNULL(SUM(Total), 0) AS TotalIngresos
    FROM Venta
    WHERE CAST(FechaVenta AS DATE) = CAST(GETDATE() AS DATE)
    AND Estado = 1
END


GO
CREATE PROC sp_ProductosBajoStock
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    p.IdProducto,
    p.NombreProducto,
    p.StockActual,
    p.StockMinimo,
    c.NombreCategoria
    FROM Producto p
    INNER JOIN Categoria c ON p.IdCategoria = c.IdCategoria
    WHERE p.StockActual <= p.StockMinimo
    AND p.Estado = 1
    ORDER BY p.StockActual ASC
END


GO
CREATE PROC sp_ProductosMasVendidos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 5
    p.NombreProducto,
    SUM(dv.Cantidad) AS TotalVendido
    FROM DetalleVenta dv
    INNER JOIN Producto p ON dv.IdProducto = p.IdProducto
    INNER JOIN Venta v ON dv.IdVenta = v.IdVenta
    WHERE v.Estado = 1
    GROUP BY p.NombreProducto
    ORDER BY TotalVendido DESC
END


GO
CREATE PROC sp_UltimasVentas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 5
    v.IdVenta,
    v.Cliente,
    v.FechaVenta,
    v.Total,
    u.NombreUsuario
    FROM Venta v
    INNER JOIN Usuario u ON v.IdUsuario = u.IdUsuario
    WHERE v.Estado = 1
    ORDER BY v.FechaVenta DESC
END


------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS de Descuento
------------------------------------------------------------------------------
GO
CREATE PROC sp_InsertarDescuento
@NombreDescuento VARCHAR(100),
@IdProducto INT = NULL,
@TipoDescuento VARCHAR(30),
@PorcentajeDescuento DECIMAL(5,2),
@FechaInicio DATE,
@FechaFin DATE,
@ColorCard VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Descuento (NombreDescuento, IdProducto, TipoDescuento, PorcentajeDescuento, FechaInicio, FechaFin, ColorCard, Estado)
    VALUES (@NombreDescuento, @IdProducto, @TipoDescuento, @PorcentajeDescuento, @FechaInicio, @FechaFin, @ColorCard, 1)
END

GO
CREATE PROC sp_ActualizarDescuento
@IdDescuento INT,
@NombreDescuento VARCHAR(100),
@IdProducto INT = NULL,
@TipoDescuento VARCHAR(30),
@PorcentajeDescuento DECIMAL(5,2),
@FechaInicio DATE,
@FechaFin DATE,
@ColorCard VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Descuento 
        SET NombreDescuento = @NombreDescuento, 
        IdProducto = @IdProducto, 
        TipoDescuento = @TipoDescuento,
        PorcentajeDescuento = @PorcentajeDescuento,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        ColorCard = @ColorCard
    WHERE IdDescuento = @IdDescuento
END

GO
CREATE PROC sp_ListadoDescuento
@Busqueda VARCHAR(100) = NULL,
@Estado BIT = NULL
AS
BEGIN
    SELECT 
        d.IdDescuento,
        d.NombreDescuento,
        d.TipoDescuento,
        d.IdProducto,
        ISNULL(p.NombreProducto,'Aplica para toda la Venta') AS NombreProducto,
        d.PorcentajeDescuento,
        d.FechaFin,
        d.ColorCard,
        d.Estado
    FROM Descuento d
    LEFT JOIN Producto p ON p.IdProducto = d.IdProducto
    WHERE (@Busqueda IS NULL OR d.NombreDescuento LIKE '%'+@Busqueda+'%' OR p.NombreProducto LIKE '%'+@Busqueda+'%') AND
    (@Estado IS NULL OR d.Estado = @Estado)
END

GO
CREATE PROC sp_DetalleDescuento
@IdDescuento INT 
AS
BEGIN
    SELECT
        d.IdDescuento,
        d.NombreDescuento,
        d.IdProducto,
        ISNULL(p.NombreProducto, 'Aplica para toda la venta') AS NombreProducto,
        d.TipoDescuento,
        d.PorcentajeDescuento,
        d.FechaInicio,
        d.FechaFin,
        d.ColorCard,
        d.Estado
    FROM Descuento d
    LEFT JOIN Producto p ON p.IdProducto = d.IdProducto
    WHERE d.IdDescuento = @IdDescuento
END

GO
CREATE PROC sp_CambiarEstadoDescuento
@IdDescuento INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Descuento 
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdDescuento = @IdDescuento
END

GO
CREATE PROC sp_ListadoDescuentoConFiltro --Para el modal de Registrar Ventas
@Busqueda VARCHAR(100) = NULL
AS
BEGIN
    SELECT 
        d.IdDescuento,
        d.NombreDescuento,
        d.IdProducto,
        ISNULL(p.NombreProducto, 'Aplica para toda la venta') AS NombreProducto,
        d.TipoDescuento,
        d.PorcentajeDescuento,
        d.FechaInicio,
        d.FechaFin,
        d.ColorCard,
        d.Estado
    FROM Descuento d
    LEFT JOIN Producto p ON p.IdProducto = d.IdProducto
    WHERE (@Busqueda IS NULL OR d.NombreDescuento LIKE '%'+@Busqueda+'%' OR p.NombreProducto LIKE '%'+@Busqueda+'%') AND
    d.Estado = 1
END

------------------------------------------------------------------------------
----------SELECTS PRINCIPALES
------------------------------------------------------------------------------
SELECT * FROM Venta
SELECT * FROM DetalleVenta
SELECT * FROM DetalleDescuento
SELECT * FROM Producto
SELECT * FROM Usuario
SELECT * FROM MovimientosStock
SELECT * FROM Rol
SELECT * FROM Categoria
SELECT * FROM Proveedor
SELECT * FROM Descuento

--------------------------INSERCIONES----------------------------------------
INSERT INTO Rol(NombreRol) VALUES('SuperUser')
INSERT INTO Rol(NombreRol) VALUES('Administrador')
INSERT INTO Rol(NombreRol) VALUES('Trabajador')

INSERT INTO Usuario(NombreUsuario, Documento, Telefono, Email, Contraseña, FechaCreacion, Estado, IdRol)
VALUES('SuperUser', '12345678', '987 654 321', 'superuser@gmail.com', 
'05528689310962b998791afd56c27da7bb2520d7130657cd1c8f87f2361b55e3', 
GETDATE(), 1, 1)


------------------------------------------------------------------------------
----------PARCHES DE SEGURIDAD
------------------------------------------------------------------------------

GO
CREATE PROC sp_CambiarContraseña
@IdUsuario INT,
@ContraseñaNueva VARCHAR(255)
AS
BEGIN
    UPDATE Usuario
        SET Contraseña = @ContraseñaNueva
        WHERE IdUsuario = @IdUsuario
END
