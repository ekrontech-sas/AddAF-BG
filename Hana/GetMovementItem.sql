SELECT  
		T0."U_BGT_ItemCode" AS "Articulo",
		(SELECT "U_BGT_Quantity" FROM "@BGT_RECEPMAT" X0 WHERE X0."U_BGT_ItemCode" = T0."U_BGT_ItemCode" AND X0."U_BGT_Batch"=T0."U_BGT_Batch" AND X0."U_BGT_DocType" = 20) AS "Cantidad Inicial",
		T0."U_BGT_Batch" AS "Lote",		
		(CASE T0."U_BGT_DocType" 
			WHEN '20' THEN 'Recepcion Compra'
			WHEN '60' THEN 'Salida de Inventario'	
			WHEN '59' THEN 'Entrada de Inventario'
			WHEN '140' THEN 'Entrega'
			WHEN '16' THEN 'Devolucion Venta'
			WHEN '21' THEN 'Devolucion Compra'
			WHEN '67' THEN 'Transferencia'
			ELSE
				'Desconocido'
		END) AS "Tipo Objeto",	
		(CASE T0."U_BGT_DocType" 
			WHEN '20' THEN T0."U_BGT_Quantity"
			WHEN '60' THEN -T0."U_BGT_Quantity"	
			WHEN '59' THEN T0."U_BGT_Quantity"
			WHEN '140' THEN -T0."U_BGT_Quantity"
			WHEN '16' THEN T0."U_BGT_Quantity"
			WHEN '21' THEN -T0."U_BGT_Quantity"
			WHEN '67' THEN T0."U_BGT_Quantity"
			ELSE
				0
		END) AS "Cantidad OP",
		T0."U_BGT_WhsCodeTo" AS "Almacen",
		0 AS "Venta",
		0 AS "Renta",
		'Activo' AS "Estado Mercantil", 
        T2."Descr" AS "Estatus Operativo"
FROM "@BGT_RECEPMAT" T0
LEFT JOIN "@BGT_PAR2" T1 ON T0."U_BGT_WhsCodeTo" = T1."U_BGT_WareHouse"  AND T1."Code" = 'INIT_VAL'
LEFT JOIN UFD1 T2 ON T1."U_BGT_StatusOp" = T2."FldValue" AND T2."TableID" = '@BGT_PAR2'  AND  T2."FieldID" = 2
WHERE T0."U_BGT_ItemCode" = '{0}'