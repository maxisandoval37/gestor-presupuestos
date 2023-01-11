INSERT INTO [dbo].[cuentas]
           ([nombre]
           ,[tipo_cuenta_id]
           ,[balance]
           ,[descripcion])
     VALUES
           ('Caja de ahorro',2,5600, 'sin descripción'),
		   ('Cuenta sueldo',5,-350, ''),
		   ('Cuenta corriente',2,-850, ''),
		   ('Cuenta Remunerada',10,120000, 'sin descripción')
GO