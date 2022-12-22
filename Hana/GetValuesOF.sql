SELECT
	T0."DocNum",
	T0."DocEntry",
	T0."CardCode",
	T1."CardName",
	T1."U_IXX_HOLD" AS "Holding"
FROM OQUT T0 
INNER JOIN OCRD T1 ON T0."CardCode" = T1."CardCode"
WHERE
	T0."DocEntry" = '{0}'