SELECT
	T8."DocNum" AS "Numero Entrega",
	--(T2."CardCode"||' - '||T2."CardName") AS "Nombre Cliente",
	T0."DocNum" AS "Numero Contrato",
	'Y' AS "Opt",
	T0."CreateDate" AS "Fecha Contrato",
	T7."PrjCode" AS "Codigo Obra",
	T7."PrjName" AS "Nombre Obra",
	T4."ItemCode" AS "Codigo Articulo",
	T4."ItemName" AS "Descripcion Articulo",
	T1."U_BGT_PrecAlq" AS "Precio Unitario",
	T1."U_BGT_CantContr" AS "Cantidad de Contrato",
	T1."U_BGT_CantObra" AS "Cantidad En Obra",
	T9."OpenQty" AS "Cantidad a Devolver",
	T4."OnHand" AS "Stock Disponible",
	T1."U_BGT_CantEntr" AS "Stock Pendiente Contrato",
	T0."DocEntry" AS "Interno Contrato",
	T8."DocEntry" AS "Interno Entrega"
FROM "@BGT_OCTR" T0
INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN ODLN T8 ON T0."DocNum" = T8."U_BGT_ContAsoc"
INNER JOIN DLN1 T9 ON T8."DocEntry" = T9."DocEntry" AND T1."U_BGT_CodAlq" = T9."ItemCode"
LEFT JOIN OCRD T2 ON T0."U_BGT_CardCode" = T2."CardCode" 
LEFT JOIN OCRD T3 ON T0."U_BGT_CodHolding" = T3."CardCode"
LEFT JOIN OITM T4 ON T1."U_BGT_CodAlq" = T4."ItemCode"
LEFT JOIN OCLG T6 ON T0."U_BGT_ActRel" = T6."ClgCode"
LEFT JOIN OPRJ T7 ON T0."U_BGT_Project" = T7."PrjCode"
WHERE 
	T8."DocEntry" IN ({0})