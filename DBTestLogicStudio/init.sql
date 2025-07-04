-- 1 Crear la base de datos y usuario
CREATE DATABASE DBTestLogicStudio;

GO

USE DBTestLogicStudio;

GO

CREATE LOGIN UserTest WITH 
        PASSWORD = 'Test.123456',
        DEFAULT_DATABASE = DBTestLogicStudio,
        CHECK_EXPIRATION = OFF,
        CHECK_POLICY = OFF;

GO

CREATE USER UserTest FOR LOGIN UserTest WITH DEFAULT_SCHEMA = dbo;

GO

-- 2 Crear esquemas definidos en la documentación previa

CREATE SCHEMA Productos;

GO

CREATE SCHEMA Cajas;

GO

GRANT CONTROL ON SCHEMA::Cajas      TO UserTest;
GRANT CONTROL ON SCHEMA::Productos  TO UserTest;

GO


-- 3 Crear tablas

CREATE TABLE Productos.Categoria (
  Id          INT IDENTITY(1,1) PRIMARY KEY,
  Descripcion NVARCHAR(50)      NOT NULL,
  Activo      BIT               NOT NULL
);

GO

CREATE TABLE Productos.Producto (
  Id           INT IDENTITY(1,1)    PRIMARY KEY,
  Id_Categoria INT                  NOT NULL,
  Nombre       NVARCHAR(100)        NOT NULL,
  Descripcion  NVARCHAR(255)        NOT NULL,
  Imagen       NVARCHAR(MAX)        NOT NULL,
  Precio       DECIMAL(8,2)         NOT NULL,
  Stock        INT                  NOT NULL,
  CONSTRAINT FK_Producto_Categoria
    FOREIGN KEY (Id_Categoria)
    REFERENCES Productos.Categoria(Id)
);

GO

CREATE TABLE Cajas.Tipo_Transaccion (
  Id     INT IDENTITY(1,1)  PRIMARY KEY,
  Nombre NVARCHAR(50)       NOT NULL,
  Activo BIT                NOT NULL
);

GO

CREATE TABLE Cajas.Transaccion_Cabecera (
  Id                   INT IDENTITY(1,1)    PRIMARY KEY,
  Id_Tipo_Transaccion  INT                  NOT NULL,
  Fecha                DATETIME             NOT NULL,
  CONSTRAINT FK_Cabecera_TipoTransaccion
    FOREIGN KEY (Id_Tipo_Transaccion)
    REFERENCES Cajas.Tipo_Transaccion(Id)
);

GO

CREATE TABLE Cajas.Transaccion_Detalle (
  Id                          INT IDENTITY(1,1) PRIMARY KEY,
  Id_Transaccion_Cabecera     INT               NOT NULL,
  Id_Producto                 INT               NOT NULL,
  Cantidad                    INT               NOT NULL,
  Precio_Unitario             DECIMAL(18,2)     NOT NULL,
  Precio_Total                DECIMAL(18,2)     NOT NULL,
  Detalle                     NVARCHAR(255)     NULL,
  CONSTRAINT FK_Detalle_Cabecera
    FOREIGN KEY (Id_Transaccion_Cabecera)
    REFERENCES Cajas.Transaccion_Cabecera(Id),
  CONSTRAINT FK_Detalle_Producto
    FOREIGN KEY (Id_Producto)
    REFERENCES Productos.Producto(Id)
);

GO

INSERT INTO Cajas.Tipo_Transaccion (Nombre,Activo)
    VALUES ('Compra',1);

GO

INSERT INTO Cajas.Tipo_Transaccion (Nombre,Activo)
    VALUES ('Venta',1);

GO

INSERT INTO Productos.Categoria (Descripcion, Activo)
    VALUES ('HOGAR',1)

GO

INSERT INTO Productos.Categoria (Descripcion, Activo)
    VALUES ('COMESTIBLES',1)

GO

INSERT INTO Productos.Categoria (Descripcion, Activo)
    VALUES ('LIMPIEZA',1)

GO