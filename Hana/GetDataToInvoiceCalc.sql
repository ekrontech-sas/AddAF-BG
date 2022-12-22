select TR.* from (
SELECT
	X0."ItemCode",
	(CASE WHEN X0."PrecioAlq" < 0 THEN -1*X0."Cantidad" ELSE X0."Cantidad" END) AS "Cantidad",
	X0."Dias",
	(CASE WHEN X0."PrecioAlq" < 0 THEN -1*(X0."PrecioAlq" * X0."Cantidad") ELSE (X0."PrecioAlq" * X0."Cantidad") END) AS "Total",
	X0."Dcto",
	X0."Tipo",
	X0."Guia",
	X0."DocEntry",
	X0."U_BGT_FecFact"
FROM 
(
	SELECT
		T2."ItemCode",
		IFNULL(((SELECT SUM(Y0."U_BGT_Quantity") FROM "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" Y0 LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ODLN" Y1 ON Y0."U_BGT_DocEntry" = Y1."DocEntry" AND Y0."U_BGT_DocType" = Y1."ObjType" LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ODRF" Y2 ON Y0."U_BGT_DocEntry" = Y2."DocEntry" AND Y0."U_BGT_DocType" = Y2."ObjType" WHERE Y0."U_BGT_DocType" IN (15,112) AND Y0."U_BGT_Contract" = TO_NVARCHAR(T0."DocNum")  AND Y0."U_BGT_ItemCode" = T2."ItemCode" AND (CASE WHEN Y0."U_BGT_DocType" = 15 THEN Y1."U_BGT_FecFact" ELSE Y2."U_BGT_FecFact" END) < TO_DATE('{1}','YYYYMMDD'))
			-
		IFNULL((SELECT SUM(Y0."U_BGT_Quantity") FROM "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" Y0 LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ORDN" Y1 ON Y0."U_BGT_DocEntry" = Y1."DocEntry" AND Y0."U_BGT_DocType" = Y1."ObjType" WHERE Y0."U_BGT_DocType" IN (16) AND Y0."U_BGT_Contract" = TO_NVARCHAR(T0."DocNum") AND Y1."U_BGT_FecFact" < TO_DATE('{1}','YYYYMMDD')  AND Y0."U_BGT_ItemCode" = T2."ItemCode" ),0)),0) AS "Cantidad",
		(DAYS_BETWEEN (TO_DATE('{1}','YYYYMMDD'), TO_DATE('{2}','YYYYMMDD'))+1) AS "Dias",
		(T1."U_BGT_PrecAlq"+(IFNULL(T1."U_BGT_Dscto",0)*T1."U_BGT_PrecAlq"/100) )/(SELECT "U_BGT_PerPrec" FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCFA" WHERE "Code" = T0."U_BGT_CicloFact") AS "PrecioAlq",
		T1."U_BGT_Dscto" AS "Dcto",
		'SALDO' AS "Tipo",
		'' AS "Guia",
		0 AS "DocEntry",
		null as "U_BGT_FecFact"
	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'AEA') > 0 AND 
		T1."U_BGT_CantObra" > 0

	UNION ALL

	SELECT
		T2."ItemCode",
		SUM("U_BGT_Quantity"),
		DAYS_BETWEEN((CASE WHEN T3."U_BGT_DocType" = 15 THEN T4."U_BGT_FecFact"  END), TO_DATE('{2}','YYYYMMDD')	)+1,
		(T1."U_BGT_PrecAlq"+(IFNULL(T1."U_BGT_Dscto",0)*T1."U_BGT_PrecAlq"/100) )/(SELECT "U_BGT_PerPrec" FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCFA" WHERE "Code" = MAX(T0."U_BGT_CicloFact"))  AS "PrecioAlq",
		T1."U_BGT_Dscto",
		'ENTREGA',
		T4."U_NUM_GUIA",
		T4."DocEntry",
		T4."U_BGT_FecFact"
	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" T3 ON TO_NVARCHAR(T0."DocNum") = T3."U_BGT_Contract" AND T2."ItemCode" = T3."U_BGT_ItemCode"
	LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ODLN" T4 ON T3."U_BGT_DocEntry" = T4."DocEntry"
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'AEA') > 0 AND 
		T1."U_BGT_CantEntreg" > 0 AND
		T3."U_BGT_DocType" IN (15,112) AND
		(CASE WHEN T3."U_BGT_DocType" = 15 THEN T4."U_BGT_FecFact" ELSE T4."U_BGT_FecFact" END) >= TO_DATE('{1}','YYYYMMDD') AND
		(CASE WHEN T3."U_BGT_DocType" = 15 THEN T4."U_BGT_FecFact" ELSE T4."U_BGT_FecFact" END) <= TO_DATE('{2}','YYYYMMDD')
	GROUP BY 
		T2."ItemCode",
		T3."U_BGT_DocNum",
		T1."U_BGT_PrecAlq", 
		T4."U_BGT_FecFact", 
		T3."U_BGT_DocType",
		T1."U_BGT_Dscto",
		T4."U_NUM_GUIA",
		T4."DocEntry"
		
		UNION ALL

	SELECT
		T2."ItemCode",
		SUM("U_BGT_Quantity"),
		DAYS_BETWEEN((CASE WHEN T3."U_BGT_DocType" = 15 THEN T5."U_BGT_FecFact" ELSE T5."U_BGT_FecFact" END), TO_DATE('{2}','YYYYMMDD')	)+1,
		(T1."U_BGT_PrecAlq"+(IFNULL(T1."U_BGT_Dscto",0)*T1."U_BGT_PrecAlq"/100) )/(SELECT "U_BGT_PerPrec" FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCFA" WHERE "Code" = MAX(T0."U_BGT_CicloFact"))  AS "PrecioAlq",
		T1."U_BGT_Dscto",
		'ENTREGA PRELIMINAR',
		T5."U_NUM_GUIA",
		T5."DocEntry",
		T5."U_BGT_FecFact"
	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" T3 ON TO_NVARCHAR(T0."DocNum") = T3."U_BGT_Contract" AND T2."ItemCode" = T3."U_BGT_ItemCode"
	LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ODRF" T5 ON T3."U_BGT_DocEntry" = T5."DocEntry" 
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'AEA') > 0 AND 
		T1."U_BGT_CantEntreg" > 0 AND
		T3."U_BGT_DocType" IN (15,112) AND
		(CASE WHEN T3."U_BGT_DocType" = 112 THEN T5."U_BGT_FecFact"  END) >= TO_DATE('{1}','YYYYMMDD') AND
		(CASE WHEN T3."U_BGT_DocType" = 112 THEN T5."U_BGT_FecFact"  END) <= TO_DATE('{2}','YYYYMMDD')
	GROUP BY 
		T2."ItemCode",
		T3."U_BGT_DocNum",
		T1."U_BGT_PrecAlq", 
		T5."U_BGT_FecFact", 
		T3."U_BGT_DocType",
		T1."U_BGT_Dscto",
		T5."U_NUM_GUIA",
		T5."DocEntry"
		
	UNION ALL

	SELECT
		T2."ItemCode",
		SUM("U_BGT_Quantity"),
		(DAYS_BETWEEN(MAX(T4."U_BGT_FecFact"), TO_DATE('{2}','YYYYMMDD') )),
		-1*((T1."U_BGT_PrecAlq"+(IFNULL("U_BGT_Dscto",0)*T1."U_BGT_PrecAlq"/100) )/(SELECT "U_BGT_PerPrec" FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCFA" WHERE "Code" = MAX(T0."U_BGT_CicloFact"))) AS "PrecioAlq",
		T1."U_BGT_Dscto",
		'DEVOLUCION',
		T4."U_NUM_GUIA",
		T4."DocEntry",
		T4."U_BGT_FecFact"
	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" T3 ON TO_NVARCHAR(T0."DocNum") = T3."U_BGT_Contract" AND T2."ItemCode" = T3."U_BGT_ItemCode"
	LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ORDN" T4 ON T3."U_BGT_DocEntry" = T4."DocEntry" AND T3."U_BGT_DocType" = T4."ObjType"
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'AEA') > 0 AND 
		T1."U_BGT_CantDev" > 0 AND
		T3."U_BGT_DocType" IN (16) AND
		T4."U_BGT_FecFact" >= TO_DATE('{1}','YYYYMMDD') AND
		T4."U_BGT_FecFact" <= TO_DATE('{2}','YYYYMMDD')
	GROUP BY 
		T2."ItemCode",
		T3."U_BGT_DocNum",
		T1."U_BGT_PrecAlq",
		T1."U_BGT_Dscto",
		T4."U_NUM_GUIA",
		T4."DocEntry",T4."U_BGT_FecFact"
) X0
WHERE
	X0."Cantidad" > 0 OR X0."Cantidad" < 0
	
UNION ALL

SELECT
	X0."ItemCode",
	X0."Cantidad",	
	0,
	(X0."Cantidad"*X0."PrecioAlq") AS "Total",
	0,
	X0."Tipo",
	X0."Guia",
	X0."DocEntry",
	X0."U_BGT_FecFact"
FROM 
(	
	SELECT
		T2."ItemCode",
		COUNT(1) AS "Cantidad",
		T1."U_BGT_PrecAlq" AS "PrecioAlq",
		'ENTREGA' AS "Tipo",
		T4."U_NUM_GUIA" AS "Guia",
		T4."DocEntry",
		T4."U_BGT_FecFact"
	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" T3 ON TO_NVARCHAR(T0."DocNum") = T3."U_BGT_Contract" AND T2."ItemCode" = T3."U_BGT_ItemCode"
	LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ODLN" T4 ON T3."U_BGT_DocEntry" = T4."DocEntry"
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'LOG') > 0 AND 
		T1."U_BGT_CantEntreg" > 0 AND
		T3."U_BGT_DocType" IN (15,112) AND
		(CASE WHEN T3."U_BGT_DocType" = 15 THEN T4."U_BGT_FecFact" END) >= TO_DATE('{1}','YYYYMMDD') AND
		(CASE WHEN T3."U_BGT_DocType" = 15 THEN T4."U_BGT_FecFact"END) <= TO_DATE('{2}','YYYYMMDD')
	GROUP BY 
		T2."ItemCode",
		T3."U_BGT_DocNum",
		T1."U_BGT_PrecAlq",
		T4."U_NUM_GUIA",
		T4."DocEntry",T4."U_BGT_FecFact"

	UNION ALL 
	
	SELECT
		T2."ItemCode",
		COUNT(1) AS "Cantidad",
		T1."U_BGT_PrecAlq" AS "PrecioAlq",
		'ENTREGA PRELIMINAR' AS "Tipo",
		T5."U_NUM_GUIA" AS "Guia",
		T5."DocEntry",
		T5."U_BGT_FecFact"

	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" T3 ON TO_NVARCHAR(T0."DocNum") = T3."U_BGT_Contract" AND T2."ItemCode" = T3."U_BGT_ItemCode"
	LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ODRF" T5 ON T3."U_BGT_DocEntry" = T5."DocEntry" 
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'LOG') > 0 AND 
		T1."U_BGT_CantEntreg" > 0 AND
		T3."U_BGT_DocType" IN (15,112) AND
		(CASE WHEN T3."U_BGT_DocType" = 15 THEN T5."U_BGT_FecFact" ELSE T5."U_BGT_FecFact" END) >= TO_DATE('{1}','YYYYMMDD') AND
		(CASE WHEN T3."U_BGT_DocType" = 15 THEN T5."U_BGT_FecFact" ELSE T5."U_BGT_FecFact" END) <= TO_DATE('{2}','YYYYMMDD')
	GROUP BY 
		T2."ItemCode",
		T3."U_BGT_DocNum",
		T1."U_BGT_PrecAlq",
		T5."U_NUM_GUIA",
		T5."DocEntry",T5."U_BGT_FecFact"

	UNION ALL 
	SELECT
		T2."ItemCode",
		COUNT(1),
		-1*T1."U_BGT_PrecAlq" AS "PrecioAlq",
		'DEVOLUCION',
		T4."U_NUM_GUIA",
		T4."DocEntry",
		T4."U_BGT_FecFact"
	FROM "ZZZ_PRUEBAS_ADDON"."@BGT_OCTR" T0
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."OITM" T2 ON T1."U_BGT_CodAlq" = T2."ItemCode"
	INNER JOIN "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT" T3 ON TO_NVARCHAR(T0."DocNum") = T3."U_BGT_Contract" AND T2."ItemCode" = T3."U_BGT_ItemCode"
	LEFT JOIN "ZZZ_PRUEBAS_ADDON"."ORDN" T4 ON T3."U_BGT_DocEntry" = T4."DocEntry" AND T3."U_BGT_DocType" = T4."ObjType"
	WHERE 
		T0."DocEntry" = '{0}' AND
		LOCATE(T2."ItemCode", 'LOG') > 0 AND 
		T1."U_BGT_CantDev" > 0 AND
		T3."U_BGT_DocType" IN (16) AND
		T4."U_BGT_FecFact" >= TO_DATE('{1}','YYYYMMDD') AND
		T4."U_BGT_FecFact" <= TO_DATE('{2}','YYYYMMDD')
	GROUP BY 
		T2."ItemCode",
		T1."U_BGT_PrecAlq",
		T4."U_NUM_GUIA",
		T4."DocEntry",T4."U_BGT_FecFact"
) X0 
WHERE
	X0."Cantidad" > 0 OR X0."Cantidad" < 0


) TR
ORDER BY TR."ItemCode", TR."U_BGT_FecFact"