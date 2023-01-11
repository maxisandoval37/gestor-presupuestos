INSERT INTO [dbo].[transacciones]
           ([usuario_id]
           ,[fecha_transaccion]
           ,[monto]
           ,[nota]
           ,[cuenta_id]
           ,[categoria_id])
     VALUES
           (2,CAST('2023-01-11T00:00:00.0000000' AS DATE),1000000.00,'Venta penthouse en Habbo',1,4),
           (2,CAST('2023-01-02T00:00:00.0000000' AS DATE),450.00,'Alfajor jorgito',3,2),
           (2,CAST('2023-01-11T00:00:00.0000000' AS DATE),100.00,NULL,4,1),
           (2,CAST('2023-01-26T00:00:00.0000000' AS DATE),45000.00,'Deuda Luz',3,3),
           (2,CAST('2022-11-01T00:00:00.0000000' AS DATE),150.00,NULL,4,1),
           (2,CAST('2022-11-13T00:00:00.0000000' AS DATE),350.00,NULL,1,1),
           (2,CAST('2023-01-11T00:00:00.0000000' AS DATE),5000.00,'Gas',3,3)  
GO