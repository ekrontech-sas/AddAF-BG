SELECT T1."ItemCode", 
	IFNULL(T2."BatchNum",T3."BatchNum")  AS "BatchNum", 
	IFNULL(T2."Quantity",T1."Quantity") AS "Quantity", 
	T0."DocEntry", 
	T0."DocNum",
	T0."ObjType",
	(CASE WHEN '{3}' = '61' THEN T1."FromWhsCod" ELSE T1."WhsCode" END) AS "WhsCodeFrom",
	(CASE WHEN '{3}' = '61' THEN T1."WhsCode" ELSE T1."WhsCode" END) AS "WhsCodeTo",
	T0."U_BGT_ContAsoc" AS "Contrato"
FROM {1} T0 
INNER JOIN {2} T1 ON T0."DocEntry" = T1."DocEntry"
LEFT JOIN IBT1 T2 ON T1."ItemCode" = T2."ItemCode" AND T1."LineNum" = T2."BaseLinNum" AND T1."DocEntry" = T2."BaseEntry" AND T2."BaseType" = {3}
LEFT JOIN IBT1 T3 ON T1."ItemCode" = T3."ItemCode" AND T1."LineNum" = T3."BaseLinNum" AND T1."BaseEntry" = T3."BaseEntry" AND T3."BaseType" = T1."BaseType"
WHERE
	T0."DocEntry" = '{0}'
