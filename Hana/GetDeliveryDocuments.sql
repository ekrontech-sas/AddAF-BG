SELECT
	T0."DocNum" AS "Numero Contrato",
	(T2."CardCode"||' - '||T2."CardName") AS "Nombre Cliente",
	'Y' AS "Opt",
	T0."CreateDate" AS "Fecha Contrato",
	T7."PrjCode" AS "Codigo Obra",
	T7."PrjName" AS "Nombre Obra",
	T4."ItemCode" AS "Codigo Articulo",
	T4."ItemName" AS "Descripcion Articulo",
	0 AS "Precio Unitario",
	--T1."U_BGT_PrecAlq" AS "Precio Unitario",
	T1."U_BGT_CantContr" AS "Cantidad de Contrato",
	T1."U_BGT_CantObra" AS "Cantidad En Obra",
	T1."U_BGT_CantEntr" AS "A Entregar",
	T4."OnHand" AS "Stock Disponible",
	T1."U_BGT_CantEntr" AS "Stock Pendiente Contrato",
	T0."DocEntry" AS "Codigo Interno",
	(T2."CardCode"||' - '||T2."CardName") AS "Cliente"
FROM "@BGT_OCTR" T0
INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
LEFT JOIN OCRD T2 ON T0."U_BGT_CardCode" = T2."CardCode" 
LEFT JOIN OCRD T3 ON T0."U_BGT_CodHolding" = T3."CardCode"
LEFT JOIN OITM T4 ON T1."U_BGT_CodAlq" = T4."ItemCode"
LEFT JOIN OQUT T5 ON T0."U_BGT_DocEntryOF" = T5."DocEntry"
LEFT JOIN OCLG T6 ON T0."U_BGT_ActRel" = T6."ClgCode"
LEFT JOIN OPRJ T7 ON T0."U_BGT_Project" = T7."PrjCode"
WHERE 
	T4."ItemCode" LIKE 'AEA%' AND
	IFNULL(T0."U_BGT_Status",'') = (CASE WHEN '{0}' = '' THEN IFNULL(T0."U_BGT_Status",'') ELSE '{0}' END) AND
	--T6."ClgCode" >= (CASE WHEN '{1}' = '' THEN T6."ClgCode" ELSE '{1}' END) AND
	--T6."ClgCode" <= (CASE WHEN '{2}' = '' THEN T6."ClgCode" ELSE '{2}' END) AND
	IFNULL(T7."PrjCode",'') >= (CASE WHEN '{3}' = '' THEN IFNULL(T7."PrjCode",'') ELSE '{3}' END) AND
	IFNULL(T7."PrjCode",'') <= (CASE WHEN '{4}' = '' THEN IFNULL(T7."PrjCode",'') ELSE '{4}' END) AND
	IFNULL(T5."DocEntry",'-1') >= (CASE WHEN '{5}' = '' THEN IFNULL(T5."DocEntry",'-1') ELSE '{5}' END) AND
	IFNULL(T5."DocEntry",'-1') <= (CASE WHEN '{6}' = '' THEN IFNULL(T5."DocEntry",'-1') ELSE '{6}' END) AND
	T2."CardCode" >= (CASE WHEN '{7}' = '' THEN T2."CardCode" ELSE '{7}' END) AND
	T2."CardCode" <= (CASE WHEN '{8}' = '' THEN T2."CardCode" ELSE '{8}' END) AND
	IFNULL(T3."CardCode",'') >= (CASE WHEN '{9}' = '' THEN IFNULL(T3."CardCode",'') ELSE '{9}' END) AND
	IFNULL(T3."CardCode",'') <= (CASE WHEN '{10}' = '' THEN IFNULL(T3."CardCode",'') ELSE '{10}' END) AND
	T0."CreateDate" >= (CASE WHEN '{11}' = '' THEN T0."CreateDate" ELSE TO_DATE('{11}','DD/MM/YYYY') END) AND
	T0."CreateDate" <= (CASE WHEN '{12}' = '' THEN T0."CreateDate" ELSE TO_DATE('{12}','DD/MM/YYYY') END) AND
	T4."ItemCode" >= (CASE WHEN '{13}' = '' THEN T4."ItemCode" ELSE '{13}' END) AND
	T4."ItemCode" <= (CASE WHEN '{14}' = '' THEN T4."ItemCode" ELSE '{14}' END)  AND
	T0."DocNum" >= (CASE WHEN '{15}' = '' THEN T0."DocNum" ELSE '{15}' END) AND
	T0."DocNum" <= (CASE WHEN '{16}' = '' THEN T0."DocNum" ELSE '{16}' END) AND
	T1."U_BGT_CantEntr" > 0
UNION
SELECT
	T0."DocNum" AS "Numero Contrato",
	(T2."CardCode"||' - '||T2."CardName") AS "Nombre Cliente",
	'Y' AS "Opt",
	T0."CreateDate" AS "Fecha Contrato",
	T7."PrjCode" AS "Codigo Obra",
	T7."PrjName" AS "Nombre Obra",
	T4."ItemCode" AS "Codigo Articulo",
	T4."ItemName" AS "Descripcion Articulo",
	0 AS "Precio Unitario",
	--T1."U_BGT_PrecAlq" AS "Precio Unitario",
	T1."U_BGT_CantContr" AS "Cantidad de Contrato",
	T1."U_BGT_CantObra" AS "Cantidad En Obra",
	T1."U_BGT_CantEntr" AS "A Entregar",
	T4."OnHand" AS "Stock Disponible",
	T1."U_BGT_CantEntr" AS "Stock Pendiente Contrato",
	T0."DocEntry" AS "Codigo Interno",
	(T2."CardCode"||' - '||T2."CardName") AS "Cliente"
FROM "@BGT_OCTR" T0
INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
LEFT JOIN OCRD T2 ON T0."U_BGT_CardCode" = T2."CardCode" 
LEFT JOIN OCRD T3 ON T0."U_BGT_CodHolding" = T3."CardCode"
LEFT JOIN OITM T4 ON T1."U_BGT_CodAlq" = T4."ItemCode"
LEFT JOIN OQUT T5 ON T0."U_BGT_DocEntryOF" = T5."DocEntry"
LEFT JOIN OCLG T6 ON T0."U_BGT_ActRel" = T6."ClgCode"
LEFT JOIN OPRJ T7 ON T0."U_BGT_Project" = T7."PrjCode"
WHERE 
	T4."ItemCode" LIKE 'LOG%' AND
	IFNULL(T0."U_BGT_Status",'') = (CASE WHEN '{0}' = '' THEN IFNULL(T0."U_BGT_Status",'') ELSE '{0}' END) AND
	--T6."ClgCode" >= (CASE WHEN '{1}' = '' THEN T6."ClgCode" ELSE '{1}' END) AND
	--T6."ClgCode" <= (CASE WHEN '{2}' = '' THEN T6."ClgCode" ELSE '{2}' END) AND
	IFNULL(T7."PrjCode",'') >= (CASE WHEN '{3}' = '' THEN IFNULL(T7."PrjCode",'') ELSE '{3}' END) AND
	IFNULL(T7."PrjCode",'') <= (CASE WHEN '{4}' = '' THEN IFNULL(T7."PrjCode",'') ELSE '{4}' END) AND
	IFNULL(T5."DocEntry",'-1') >= (CASE WHEN '{5}' = '' THEN IFNULL(T5."DocEntry",'-1') ELSE '{5}' END) AND
	IFNULL(T5."DocEntry",'-1') <= (CASE WHEN '{6}' = '' THEN IFNULL(T5."DocEntry",'-1') ELSE '{6}' END) AND
	T2."CardCode" >= (CASE WHEN '{7}' = '' THEN T2."CardCode" ELSE '{7}' END) AND
	T2."CardCode" <= (CASE WHEN '{8}' = '' THEN T2."CardCode" ELSE '{8}' END) AND
	IFNULL(T3."CardCode",'') >= (CASE WHEN '{9}' = '' THEN IFNULL(T3."CardCode",'') ELSE '{9}' END) AND
	IFNULL(T3."CardCode",'') <= (CASE WHEN '{10}' = '' THEN IFNULL(T3."CardCode",'') ELSE '{10}' END) AND
	T0."CreateDate" >= (CASE WHEN '{11}' = '' THEN T0."CreateDate" ELSE TO_DATE('{11}','DD/MM/YYYY') END) AND
	T0."CreateDate" <= (CASE WHEN '{12}' = '' THEN T0."CreateDate" ELSE TO_DATE('{12}','DD/MM/YYYY') END) AND
	T4."ItemCode" >= (CASE WHEN '{13}' = '' THEN T4."ItemCode" ELSE '{13}' END) AND
	T4."ItemCode" <= (CASE WHEN '{14}' = '' THEN T4."ItemCode" ELSE '{14}' END)  AND
	T0."DocNum" >= (CASE WHEN '{15}' = '' THEN T0."DocNum" ELSE '{15}' END) AND
	T0."DocNum" <= (CASE WHEN '{16}' = '' THEN T0."DocNum" ELSE '{16}' END)