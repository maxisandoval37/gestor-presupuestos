USE [PresupuestosDB]
GO

INSERT INTO [dbo].[cuentas]
           ([nombre]
           ,[tipo_cuenta_id]
           ,[balance]
           ,[descripcion])
     VALUES
           ('CashBack Tarjeta Santander Black',2,5600, 'sin descripción'),
		   ('Fiado kiosco',5,-350, ''),
		   ('Recarga Tuenti',2,-850, ''),
		   ('Venta Penthouse 5 estrellas en Habbo',10,120000, 'sin descripción'),
		   ('Renault Plan de ahorro Kwid 120 cuotas',5,-67000, 'sin descripción')
GO