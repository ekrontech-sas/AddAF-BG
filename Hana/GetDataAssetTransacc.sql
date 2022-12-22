SELECT 
	T1."ItemCode", 
	T1."Quantity" AS "Quantity", 
	T0."DocEntry", 
	T0."DocEntry" AS "DocNum", 
	T0."ObjType"
FROM {1} T0 
INNER JOIN {2} T1 ON T0."DocEntry" = T1."DocEntry"
WHERE
	T0."DocEntry" = '{0}'