SELECT
   T0."DocNum",
   T0."DocEntry",
   T0."DocDate",
   T0."DocTime",
   T0."CardCode",
   T0."CardName",
   T1."ItemCode",
   T1."Quantity",
   T1."OpenQty",
   T1."LineNum"
FROM ODLN T0
INNER JOIN DLN1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN "@BGT_CTR1" T2 ON T1."ItemCode" = T2."U_BGT_CodAlq"
INNER JOIN "@BGT_OCTR" T3 ON T2."DocEntry" = T3."DocEntry" AND T0."U_BGT_ContAsoc" = T3."DocNum"
WHERE
        T3."DocNum" = '{0}' AND
        T1."ItemCode" = '{1}' AND
		T1."OpenQty" > 0
ORDER BY T0."DocEntry", T0."DocDate" 