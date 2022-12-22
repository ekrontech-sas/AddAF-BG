SELECT
	Z0."U_BGT_DocDate" AS "Fecha",
	Z0."Tipo Objeto",
	Z0."U_BGT_DocEntry" AS "Nro Interno",
	Z0."U_BGT_DocNum" AS "Nro Documento",
	Z0."Warehouse" AS "Almacen",
	Z0."Lote" AS "Lote",
	Z0."Cantidad OP"
FROM (
	SELECT   	
			T0."Name",
			T0."U_BGT_ItemCode" AS "Articulo",
			(SELECT "U_BGT_Quantity" FROM "@BGT_RECEPMAT" X0 WHERE X0."U_BGT_ItemCode" = T0."U_BGT_ItemCode" AND X0."U_BGT_DocType" = 20) AS "Cantidad Inicial",	
			(CASE T0."U_BGT_DocType" 
				WHEN '13' THEN 'Factura'
				WHEN '14' THEN 'NC Venta'
				WHEN '15' THEN 'Entrega'
				WHEN '16' THEN 'Devoluciones'
				WHEN '18' THEN 'Factura Proveedor'
				WHEN '19' THEN 'NC Proveedor'
				WHEN '20' THEN 'Recepcion Compra'
				WHEN '21' THEN 'Devolucion Mercancia'
				WHEN '60' THEN 'Salida de Inventario'	
				WHEN '59' THEN 'Entrada de Inventario'
				WHEN '140' THEN 'Entrega'
				WHEN '16' THEN 'Devolucion Venta'
				WHEN '21' THEN 'Devolucion Compra'
				WHEN '67' THEN 'Transferencia'
				WHEN '1470000049' THEN 'Capitalizacion'
				WHEN '1470000060' THEN 'NC Capitalizacion'
				WHEN '1470000094' THEN 'Baja AF'
				WHEN '9470000013' THEN 'Traspaso AF'
				WHEN '9472000038' THEN 'Amortizacion Manual'
				ELSE
					'Desconocido'
			END) AS "Tipo Objeto",	
			(CASE T0."U_BGT_DocType" 

				WHEN '13' THEN T0."U_BGT_Quantity"
				WHEN '14' THEN T0."U_BGT_Quantity"
				WHEN '15' THEN T0."U_BGT_Quantity"
				WHEN '16' THEN T0."U_BGT_Quantity"
				WHEN '18' THEN T0."U_BGT_Quantity"
				WHEN '19' THEN T0."U_BGT_Quantity"
				WHEN '20' THEN T0."U_BGT_Quantity"
				WHEN '21' THEN -T0."U_BGT_Quantity"
				WHEN '60' THEN -T0."U_BGT_Quantity"		
				WHEN '59' THEN T0."U_BGT_Quantity"
				WHEN '140' THEN -T0."U_BGT_Quantity"
				WHEN '16' THEN T0."U_BGT_Quantity"
				WHEN '21' THEN -T0."U_BGT_Quantity"
				WHEN '67' THEN T0."U_BGT_Quantity"
				WHEN '1470000049' THEN T0."U_BGT_Quantity"
				WHEN '1470000060' THEN T0."U_BGT_Quantity"
				WHEN '1470000094' THEN T0."U_BGT_Quantity"
				WHEN '9470000013' THEN T0."U_BGT_Quantity"
				WHEN '9472000038' THEN T0."U_BGT_Quantity"
				ELSE
					0
			END) AS "Cantidad OP",
			0 AS "Venta",
			0 AS "Renta",
			'Activo' AS "Estado Mercantil", 
			T2."Descr" AS "Estatus Operativo",
			T0."U_BGT_DocDate",
			T0."U_BGT_DocEntry",
			T0."U_BGT_WhsCodeTo" AS "Warehouse",
			T0."U_BGT_Batch" AS "Lote",
			T0."U_BGT_DocNum" 
	FROM "@BGT_RECEPMAT" T0
	LEFT JOIN "@BGT_PAR2" T1 ON T0."U_BGT_WhsCodeTo" = T1."U_BGT_WareHouse"  AND T1."Code" = 'INIT_VAL'
	LEFT JOIN UFD1 T2 ON T1."U_BGT_StatusOp" = T2."FldValue" AND T2."TableID" = '@BGT_PAR2'  AND  T2."FieldID" = 2
	LEFT JOIN OITM T3 ON T0."U_BGT_ItemCode" = T3."ItemCode" 
	WHERE T3."U_AGT_ItemAsoc" = '{0}'

	UNION

	SELECT  
			T0."Name",
			T0."U_BGT_ItemAsset" AS "Articulo",
			(SELECT "U_BGT_Quantity" FROM "@BGT_RECEPASSET" X0 WHERE X0."U_BGT_ItemAsset" = T0."U_BGT_ItemAsset" AND X0."U_BGT_DocType" = 20) AS "Cantidad Inicial",	
			(CASE T0."U_BGT_DocType" 
				WHEN '13' THEN 'Factura'
				WHEN '14' THEN 'NC Venta'
				WHEN '15' THEN 'Entrega'
				WHEN '16' THEN 'Devoluciones'
				WHEN '18' THEN 'Factura Proveedor'
				WHEN '19' THEN 'NC Proveedor'
				WHEN '20' THEN 'Recepcion Compra'
				WHEN '21' THEN 'Devolucion Mercancia'
				WHEN '60' THEN 'Salida de Inventario'	
				WHEN '59' THEN 'Entrada de Inventario'
				WHEN '140' THEN 'Entrega'
				WHEN '16' THEN 'Devolucion Venta'
				WHEN '21' THEN 'Devolucion Compra'
				WHEN '67' THEN 'Transferencia'
				WHEN '1470000049' THEN 'Capitalizacion'
				WHEN '1470000060' THEN 'NC Capitalizacion'
				WHEN '1470000094' THEN 'Baja AF'
				WHEN '9470000013' THEN 'Traspaso AF'
				WHEN '9472000038' THEN 'Amortizacion Manual'
				ELSE
					'Desconocido'
			END) AS "Tipo Objeto",	
			(CASE T0."U_BGT_DocType" 
				WHEN '13' THEN T0."U_BGT_Quantity"
				WHEN '14' THEN T0."U_BGT_Quantity"
				WHEN '15' THEN T0."U_BGT_Quantity"
				WHEN '16' THEN T0."U_BGT_Quantity"
				WHEN '18' THEN T0."U_BGT_Quantity"
				WHEN '19' THEN T0."U_BGT_Quantity"
				WHEN '20' THEN T0."U_BGT_Quantity"
				WHEN '21' THEN -T0."U_BGT_Quantity"
				WHEN '60' THEN -T0."U_BGT_Quantity"	
				WHEN '59' THEN T0."U_BGT_Quantity"
				WHEN '140' THEN -T0."U_BGT_Quantity"
				WHEN '16' THEN T0."U_BGT_Quantity"
				WHEN '21' THEN T0."U_BGT_Quantity"
				WHEN '67' THEN T0."U_BGT_Quantity"
				WHEN '1470000049' THEN T0."U_BGT_Quantity"
				WHEN '1470000060' THEN T0."U_BGT_Quantity"
				WHEN '1470000094' THEN T0."U_BGT_Quantity"
				WHEN '9470000013' THEN T0."U_BGT_Quantity"
				WHEN '9472000038' THEN T0."U_BGT_Quantity"
				ELSE
					0
			END) AS "Cantidad OP",
			0 AS "Venta",
			0 AS "Renta",
			'Activo' AS "Estado Mercantil", 
			'' AS "Estatus Operativo",
			T0."U_BGT_DocDate",
			T0."U_BGT_DocEntry",
			'',
			'' AS "Lote",
			T0."U_BGT_DocNum" 
	FROM "@BGT_RECEPASSET" T0
	WHERE T0."U_BGT_ItemAsset" = '{0}'
) Z0
ORDER BY Z0."Name" ASC