SELECT
	X0."Articulo",
	 "Nom. Articulo",
	X0."Numero Documento",
	--X0."Code",
	X0."Numero Interno",
	X0."Fecha de Documento",
	X0."Fecha de Factura",
	X0."Type",
	X0."Tipo Objeto",
	X0."Numero Guia",
	X0."Cantidad",
	--X0."Lote",
	X0."Hora"
FROM 
(
	SELECT 
		 T1."U_BGT_CodAlq" as "Articulo",
		 T3."ItemName" AS "Nom. Articulo",
		 T2."U_BGT_DocNum" AS "Numero Documento",
		 T2."Code",
		 T2."U_BGT_DocEntry" AS "Numero Interno",
		 --T2."U_BGT_DocNum" AS "Numero Documento",
		 T2."U_BGT_DocDate" AS "Fecha de Documento",
		  (CASE T2."U_BGT_DocType" 
			WHEN '13' THEN T7."U_BGT_FecFact"
			WHEN '14' THEN T6."U_BGT_FecFact"
			WHEN '15' THEN T4."U_BGT_FecFact"
			WHEN '16' THEN T5."U_BGT_FecFact"
			WHEN '18' THEN T8."U_BGT_FecFact"
			WHEN '19' THEN T9."U_BGT_FecFact"
			WHEN '20' THEN T10."U_BGT_FecFact"
			WHEN '21' THEN T11."U_BGT_FecFact"
			WHEN '60' THEN T12."U_BGT_FecFact"	
			WHEN '59' THEN T13."U_BGT_FecFact"	
			WHEN '67' THEN T14."U_BGT_FecFact"	
			WHEN '112' THEN T15."U_BGT_FecFact"
			WHEN '116' THEN T16."U_BGT_FecFact"
			ELSE
				''
			END) AS "Fecha de Factura",
		 T2."U_BGT_DocType" AS "Type",
		 (CASE T2."U_BGT_DocType" 
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
			WHEN '67' THEN 'Transferencia'
			WHEN '112' THEN 'Entrega Preeliminar'
			WHEN '116' THEN 'Devolucion Exceso'
			ELSE
				'Desconocido'
		 END) AS "Tipo Objeto",	
		 (CASE T2."U_BGT_DocType" 
			WHEN '13' THEN T7."U_NUM_GUIA"
			WHEN '14' THEN T6."U_NUM_GUIA"
			WHEN '15' THEN T4."U_NUM_GUIA"
			WHEN '16' THEN T5."U_NUM_GUIA"
			WHEN '18' THEN T8."U_NUM_GUIA"
			WHEN '19' THEN T9."U_NUM_GUIA"
			WHEN '20' THEN T10."U_NUM_GUIA"
			WHEN '21' THEN T11."U_NUM_GUIA"
			WHEN '60' THEN T12."U_NUM_GUIA"	
			WHEN '59' THEN T13."U_NUM_GUIA"	
			WHEN '67' THEN T14."U_NUM_GUIA"	
			WHEN '112' THEN T15."U_NUM_GUIA"	
			WHEN '116' THEN T16."U_NUM_GUIA"	
			ELSE
				''
		 END) AS "Numero Guia",	
		 T2."U_BGT_Quantity" AS "Cantidad",
		 T2."U_BGT_Batch" AS "Lote",
		(CASE T2."U_BGT_DocType" 
			WHEN '13' THEN T7."DocTime"
			WHEN '14' THEN T6."DocTime"
			WHEN '15' THEN T4."DocTime"
			WHEN '16' THEN T5."DocTime"
			WHEN '18' THEN T8."DocTime"
			WHEN '19' THEN T9."DocTime"
			WHEN '20' THEN T10."DocTime"
			WHEN '21' THEN T11."DocTime"
			WHEN '60' THEN T13."DocTime"
			WHEN '59' THEN T12."DocTime"
			WHEN '67' THEN T14."DocTime"
			WHEN '112' THEN T15."DocTime"
			WHEN '116' THEN T16."DocTime"
			ELSE
				null
		 END) AS "Hora"
	FROM "@BGT_OCTR" T0
	INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	LEFT JOIN "@BGT_RECEPMAT" T2 ON TO_NVARCHAR(T0."DocNum") = T2."U_BGT_Contract" AND T1."U_BGT_CodAlq" = T2."U_BGT_ItemCode"
	LEFT JOIN OITM T3 ON T1."U_BGT_CodAlq" = T3."ItemCode"
	LEFT JOIN ODLN T4 ON T2."U_BGT_DocEntry" = T4."DocEntry" AND T2."U_BGT_DocType" = '15'
	LEFT JOIN ORDN T5 ON T2."U_BGT_DocEntry" = T5."DocEntry" AND T2."U_BGT_DocType" = '16'
	LEFT JOIN ORIN T6 ON T2."U_BGT_DocEntry" = T6."DocEntry" AND T2."U_BGT_DocType" = '14'
	LEFT JOIN OINV T7 ON T2."U_BGT_DocEntry" = T7."DocEntry" AND T2."U_BGT_DocType" = '13'
	LEFT JOIN OPCH T8 ON T2."U_BGT_DocEntry" = T8."DocEntry" AND T2."U_BGT_DocType" = '18'
	LEFT JOIN ORPC T9 ON T2."U_BGT_DocEntry" = T9."DocEntry" AND T2."U_BGT_DocType" = '19'
	LEFT JOIN OPDN T10 ON T2."U_BGT_DocEntry" = T10."DocEntry" AND T2."U_BGT_DocType" = '20'
	LEFT JOIN ORPD T11 ON T2."U_BGT_DocEntry" = T11."DocEntry" AND T2."U_BGT_DocType" = '21'
	LEFT JOIN OIGN T12 ON T2."U_BGT_DocEntry" = T12."DocEntry" AND T2."U_BGT_DocType" = '59'
	LEFT JOIN OIGE T13 ON T2."U_BGT_DocEntry" = T13."DocEntry" AND T2."U_BGT_DocType" = '60'
	LEFT JOIN OWTR T14 ON T2."U_BGT_DocEntry" = T14."DocEntry" AND T2."U_BGT_DocType" = '67'
	LEFT JOIN ODRF T15 ON T2."U_BGT_DocEntry" = T15."DocEntry" AND T2."U_BGT_DocType" = '112'
	LEFT JOIN ORDN T16 ON T2."U_BGT_DocEntry" = T16."DocEntry" AND T2."U_BGT_DocType" = '116'
	WHERE 
	T0."DocNum" = '{0}' AND
	T2."U_BGT_DocType" NOT IN ('13','14','19','20') AND 
	IFNULL(T2."U_BGT_Quantity",0) > 0 

	UNION

	SELECT 
		 T2."U_BGT_ItemCode" as "Articulo",
		 T3."ItemName" AS "Nom. Articulo",
		 T2."U_BGT_DocNum" AS "Numero Documento",
		 T2."Code",
		 T2."U_BGT_DocEntry" AS "Numero Interno",
		 --T2."U_BGT_DocNum" AS "Numero Documento",
		 T2."U_BGT_DocDate" AS "Fecha de Documento",
		 T16."U_BGT_FecFact" AS "Fecha de Factura",
		 T2."U_BGT_DocType" AS "Type",
		 'Devolucion Exceso' AS "Tipo Objeto",	
		 T16."U_NUM_GUIA" AS "Numero Guia",	
		 T2."U_BGT_Quantity" AS "Cantidad",
		 T2."U_BGT_Batch" AS "Lote",
		 T16."DocTime"
	FROM "@BGT_RECEPMAT" T2 
	LEFT JOIN ORDN T16 ON T2."U_BGT_DocEntry" = T16."DocEntry" AND T2."U_BGT_DocType" = '116'
	LEFT JOIN OITM T3 ON T2."U_BGT_ItemCode" = T3."ItemCode"
	WHERE 
	T2."U_BGT_Contract" = '{0}' AND
	IFNULL(T2."U_BGT_Quantity",0) > 0 AND
	T2."U_BGT_DocType" NOT IN ('13','14','19','20') AND
	T2."U_BGT_DocType" = '116'
) X0
where  X0."Articulo"='{1}'
ORDER BY 
	X0."Articulo", X0."Numero Documento", X0."Fecha de Factura"