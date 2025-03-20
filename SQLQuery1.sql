-- Creación de la base de datos (Creación de la base de datos)
CREATE DATABASE Test20250320DB;
GO

USE Test20250320DB;
GO

-- Tabla: Warehouses (Tabla: Bodegas)
CREATE TABLE Warehouses (
    WarehouseID INT PRIMARY KEY IDENTITY(1,1), -- ID de la bodega (ID de la bodega)
    WarehouseName VARCHAR(255) NOT NULL,       -- Nombre de la bodega (Nombre de la bodega)
    Notes TEXT                                 -- Notas adicionales (Notas adicionales)
);
GO

-- Tabla: Brands (Tabla: Marcas)
CREATE TABLE Brands (
    BrandID INT PRIMARY KEY IDENTITY(1,1), -- ID de la marca (ID de la marca)
    BrandName VARCHAR(255) NOT NULL,       -- Nombre de la marca (Nombre de la marca)
    Country VARCHAR(255)                   -- País de origen de la marca (País de origen de la marca)
);
GO

-- Tabla: Products (Tabla: Productos)
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),  -- ID del producto (ID del producto)
    ProductName VARCHAR(255) NOT NULL,      -- Nombre del producto (Nombre del producto)
    Description TEXT,                       -- Descripción del producto (Descripción del producto)
    Price DECIMAL(10, 2) NOT NULL,          -- Precio del producto (Precio del producto)
    WarehouseID INT FOREIGN KEY REFERENCES Warehouses(WarehouseID), -- ID de la bodega (ID de la bodega)
    BrandID INT FOREIGN KEY REFERENCES Brands(BrandID)              -- ID de la marca (ID de la marca)
);
GO

-- Tabla: Users (Tabla: Usuarios)
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),     -- ID del usuario (ID del usuario)
    Username VARCHAR(255) NOT NULL,         -- Nombre de usuario (Nombre de usuario)
    Email VARCHAR(255) UNIQUE NOT NULL,     -- Correo electrónico del usuario (Correo electrónico del usuario)
    PasswordHash CHAR(32) NOT NULL,     -- Hash de la contraseña del usuario (Hash de la contraseña del usuario)
    Role VARCHAR(50) NOT NULL                -- Rol del usuario (Rol del usuario)
);
GO

ALTER TABLE Users
ADD Status TINYINT DEFAULT 1


INSERT INTO Users (Username, PasswordHash, Email, Role)
VALUES
    ('Root', '827ccb0eea8a706c4c34a16891f84e7b ', 'gerente@gmail.com', 'GERENTE');

INSERT INTO Users (Username, PasswordHash, Email, Role)
VALUES
    ('Root', '827ccb0eea8a706c4c34a16891f84e7b ', 'administrador@gmail.com', 'ADMINISTRADOR');