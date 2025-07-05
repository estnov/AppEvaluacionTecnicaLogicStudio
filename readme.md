# Prueba técnica para Logic Studio - Produbanco

A continuación se presenta como ejecutar el proyecto completo y como usarlo correctamente.

## Ejecución del proyecto:

Todo el proyecto se arranca desde docker para facilidad de la prueba y su evaluación.

1. Ejecutar en la raíz del proyecto

    Comando: docker-compose up -d 

    Con ello el proyecto arrancará correctamente. (Nota: para no tener problemas tener libres los puertos 1433, 8080, 8081 y 4200 sino cambiarlos en el docker-compose)

2. Evidencias
    Se puede acceder a las evidencias con el siguiente link (copiar y pegar en el navegador)

    https://docs.google.com/document/d/1ZPUohKEjfknDqr3nzyFSNn4_Qfq8WmfYvO4ZeIxFvRo/edit?usp=sharing

## Base de datos: SQL Server.

    El script de la base de datos se ejecuta solo la primera vez que se hacer el docker-compose up -d.

    Se encuentra en:
        ./DBTestLogicStudio/init.sql

    Parámetros de conexión:

    - Host: localhost, Puerto: 1433
    - User: UserTest
    - Password: Test.123456

    La base se ha inicializado con 3 categorias de productos:

    - HOGAR
    - COMESTIBLES
    - LIMPIEZA

## Backend 2 microservicios separados.

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

## Angular (Front)

    Se ha realizado el proyecto con Angular 19 + NgZorro.

