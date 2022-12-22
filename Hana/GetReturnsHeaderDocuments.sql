SELECT
	--ROW_NUMBER() OVER (ORDER BY T0."DocNum") AS "RowNumber",
	T0."DocNum" AS "Numero Contrato",
	T2."CardCode"||' - '||T2."CardName"  AS "Cliente",
	'Y' AS "Opt",
	T0."CreateDate" AS "Fecha Contrato",
	T8."ItemCode" AS "Codigo Articulo",
	T8."ItemName" AS "Nombre Articulo",
	T0."DocEntry" AS "Interno Contrato",
	T7."PrjCode" AS "Codigo Obra",
	T7."PrjName" AS "Nombre Obra",
	--0.0 AS "Cantidad a Modificar",
	T1."U_BGT_CantObra" AS "Cantidad a Devolver",
	T1."U_BGT_CantObra" AS "Cantidad en Obra", 
	0.0 AS "Devolucion Especial 1",
	CAST('' AS NVARCHAR(40)) AS "Bodega Devolucion Especial 1",
	0.0 AS "Devolucion Especial 2",
	CAST('' AS NVARCHAR(40)) AS "Bodega Devolucion Especial 2",
	T1."U_BGT_LineNum"
FROM "@BGT_OCTR" T0
INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN OITM T8 ON T1."U_BGT_CodAlq" = T8."ItemCode"
LEFT JOIN OCRD T2 ON T0."U_BGT_CardCode" = T2."CardCode" 
LEFT JOIN OCLG T6 ON T0."U_BGT_ActRel" = T6."ClgCode"
LEFT JOIN OPRJ T7 ON T0."U_BGT_Project" = T7."PrjCode"
WHERE 
	T8."ItemCode" LIKE 'AEA%' AND
	T0."U_BGT_Status" = (CASE WHEN '{0}' = '' THEN T0."U_BGT_Status" ELSE '{0}' END) AND
	--T6."ClgCode" >= (CASE WHEN '{1}' = '' THEN T6."ClgCode" ELSE '{1}' END) AND
	--T6."ClgCode" <= (CASE WHEN '{2}' = '' THEN T6."ClgCode" ELSE '{2}' END) AND
	T7."PrjCode" >= (CASE WHEN '{3}' = '' THEN T7."PrjCode" ELSE '{3}' END) AND
	T7."PrjCode" <= (CASE WHEN '{4}' = '' THEN T7."PrjCode" ELSE '{4}' END) AND
	T8."ItemCode" >= (CASE WHEN '{5}' = '' THEN T8."ItemCode" ELSE '{5}' END) AND
	T8."ItemCode" <= (CASE WHEN '{6}' = '' THEN T8."ItemCode" ELSE '{6}' END) AND
	T0."DocEntry" >= (CASE WHEN '{7}' = '' THEN T0."DocEntry" ELSE '{7}' END) AND
	T0."DocEntry" <= (CASE WHEN '{8}' = '' THEN T0."DocEntry" ELSE '{8}' END) AND
	T2."CardCode" >= (CASE WHEN '{9}' = '' THEN T2."CardCode" ELSE '{9}' END) AND
	T2."CardCode" <= (CASE WHEN '{10}' = '' THEN T2."CardCode" ELSE '{10}' END) AND
	T0."CreateDate" >= (CASE WHEN '{11}' = '' THEN T0."CreateDate" ELSE TO_DATE('{11}','DD/MM/YYYY') END) AND
	T0."CreateDate" <= (CASE WHEN '{12}' = '' THEN T0."CreateDate" ELSE TO_DATE('{12}','DD/MM/YYYY') END) AND
	T1."U_BGT_CantObra" > 0
UNION
SELECT
	--ROW_NUMBER() OVER (ORDER BY T0."DocNum") AS "RowNumber",
	T0."DocNum" AS "Numero Contrato",
	T2."CardCode"||' - '||T2."CardName"  AS "Cliente",
	'Y' AS "Opt",
	T0."CreateDate" AS "Fecha Contrato",
	T8."ItemCode" AS "Codigo Articulo",
	T8."ItemName" AS "Nombre Articulo",
	T0."DocEntry" AS "Interno Contrato",
	T7."PrjCode" AS "Codigo Obra",
	T7."PrjName" AS "Nombre Obra",
	--0.0 AS "Cantidad a Modificar",
	T1."U_BGT_CantObra" AS "Cantidad a Devolver",
	T1."U_BGT_CantObra" AS "Cantidad en Obra", 
	0.0 AS "Devolucion Especial 1",
	CAST('' AS NVARCHAR(40)) AS "Bodega Devolucion Especial 1",
	0.0 AS "Devolucion Especial 2",
	CAST('' AS NVARCHAR(40)) AS "Bodega Devolucion Especial 2",
	T1."U_BGT_LineNum"
FROM "@BGT_OCTR" T0
INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN OITM T8 ON T1."U_BGT_CodAlq" = T8."ItemCode"
LEFT JOIN OCRD T2 ON T0."U_BGT_CardCode" = T2."CardCode" 
LEFT JOIN OCLG T6 ON T0."U_BGT_ActRel" = T6."ClgCode"
LEFT JOIN OPRJ T7 ON T0."U_BGT_Project" = T7."PrjCode"
WHERE 
	T8."ItemCode" LIKE 'LOG%' AND
	T0."U_BGT_Status" = (CASE WHEN '{0}' = '' THEN T0."U_BGT_Status" ELSE '{0}' END) AND
	--T6."ClgCode" >= (CASE WHEN '{1}' = '' THEN T6."ClgCode" ELSE '{1}' END) AND
	--T6."ClgCode" <= (CASE WHEN '{2}' = '' THEN T6."ClgCode" ELSE '{2}' END) AND
	T7."PrjCode" >= (CASE WHEN '{3}' = '' THEN T7."PrjCode" ELSE '{3}' END) AND
	T7."PrjCode" <= (CASE WHEN '{4}' = '' THEN T7."PrjCode" ELSE '{4}' END) AND
	T8."ItemCode" >= (CASE WHEN '{5}' = '' THEN T8."ItemCode" ELSE '{5}' END) AND
	T8."ItemCode" <= (CASE WHEN '{6}' = '' THEN T8."ItemCode" ELSE '{6}' END) AND
	T0."DocEntry" >= (CASE WHEN '{7}' = '' THEN T0."DocEntry" ELSE '{7}' END) AND
	T0."DocEntry" <= (CASE WHEN '{8}' = '' THEN T0."DocEntry" ELSE '{8}' END) AND
	T2."CardCode" >= (CASE WHEN '{9}' = '' THEN T2."CardCode" ELSE '{9}' END) AND
	T2."CardCode" <= (CASE WHEN '{10}' = '' THEN T2."CardCode" ELSE '{10}' END) AND
	T0."CreateDate" >= (CASE WHEN '{11}' = '' THEN T0."CreateDate" ELSE TO_DATE('{11}','DD/MM/YYYY') END) AND
	T0."CreateDate" <= (CASE WHEN '{12}' = '' THEN T0."CreateDate" ELSE TO_DATE('{12}','DD/MM/YYYY') END)
ORDER BY 2, 1