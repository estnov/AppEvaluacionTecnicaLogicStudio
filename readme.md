# Prueba técnica para Logic Studio - Produbanco

A continuación se presenta como ejecutar el proyecto completo y como usarlo correctamente.

## Arrancar el proyecto:

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