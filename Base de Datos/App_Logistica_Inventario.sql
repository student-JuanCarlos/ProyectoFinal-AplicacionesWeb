CREATE DATABASE App_Logistica_Inventario

USE App_Logistica_Inventario

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
    Fotografia VARCHAR(100) NULL,
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
    Fotografia VARCHAR(100) NOT NULL,
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
    TipoDeMovimiento VARCHAR(20) NOT NULL, -- 'ENTRADA' o 'SALIDA'
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
    Total DECIMAL(12,2) NOT NULL
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
    INSERT INTO Usuario(Nombre) VALUES



 

    IdUsuario INT IDENTITY (1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(40) NOT NULL,
    Documento VARCHAR(20) NOT NULL,
    Telefono VARCHAR(30),
    Email VARCHAR(50) UNIQUE NOT NULL,
    Contraseña VARCHAR(100) NOT NULL, 
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Estado BIT DEFAULT 1,
    IdRol INT NOT NULL FOREIGN KEY REFERENCES Rol(IdRol)