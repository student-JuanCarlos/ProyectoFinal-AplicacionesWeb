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
    Cantidad INT NOT NULL,
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

------------------------------------------------------------------------------
----------PROCEDIMIENTOS ALMACENADOS DE Usuaro
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
    WHERE u.Email = @Email AND u.Contraseña = @Contraseña AND Estado = 1
END

GO
CREATE PROC sp_Listado_Usuario
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    u.IdUsuario,
    u.NombreUsuario,
    r.NombreRol,
    u.Documento,
    u.Telefono,
    u.Email,
    u.FechaCreacion,
    u.Estado
    FROM Usuario u
    INNER JOIN Rol r ON u.IdRol = r.IdRol
    ORDER BY u.Estado DESC, u.NombreUsuario ASC
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