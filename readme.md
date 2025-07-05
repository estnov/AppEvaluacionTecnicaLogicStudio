# Prueba técnica para Logic Studio - Produbanco

A continuación se presenta como ejecutar el proyecto completo y como usarlo correctamente.

## Ejecución del proyecto:

Todo el proyecto se arranca desde docker para facilidad de la prueba y su evaluación.

1. Ejecutar el proyecto

    Comando: docker-compose up -d 

    Con ello el proyecto arrancará correctamente.

2. Conexión al SQL Server.

    Parámetros de conexión:

    - Host: localhost, Puerto: 1433
    - User: UserTest
    - Password: Test.123456

    La base se ha inicializado con 3 categorias de productos:

    - HOGAR
    - COMESTIBLES
    - LIMPIEZA

## Detalles de endpoints.

En cumplimiento con el ejercicio propuesto se han creado los siguientes endpoints a continuación descritos:

- Productos API — http://localhost:8080
    - GET	/api/Products/GetCategoriesList	        Lista todas las categorías.
    - GET	/api/Products/GetProductsList	        Lista todos los productos.
    - GET	/api/Products/GetProduct/{id}	        Devuelve un producto por id.
    - PUT	/api/Products/CreateProduct	            Crea un producto.
    - PUT	/api/Products/UpdateProduct/{id}	    Actualiza un producto.
    - DELETE	/api/Products/DeleteProduct/{id}	Elimina un producto por id.

- Transacciones API — http://localhost:8081
    - POST /api/Transactions/GenerateTransaction	Registra una transacción (cabecera + detalles) y ajusta stock.
    - GET	 /api/Transactions/GetTransactionList	Devuelve todas las transacciones con sus detalles.
    - GET	 /api/Transactions/GetTransactionTypes	Lista los tipos de transacción (p. ej. Venta = 1).

Cada esquema tiene Swagger para su consulta (/swagger/index.html)