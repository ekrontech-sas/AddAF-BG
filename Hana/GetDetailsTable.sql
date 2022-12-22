SELECT 
         T2."ItemCode",
		 T2."ItemName",
		 T1."Quantity",
         T3."Price",
		 T2."SWeight1",
		 (SELECT "Price" FROM ITM1 X0 WHERE X0."ItemCode" = T1."ItemCode" AND "PriceList" = '2') AS "PriceVenta"
FROM OQUT T0 
INNER JOIN QUT1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
INNER JOIN ITM1 T3 ON T2."ItemCode" = T3."ItemCode" AND T3."PriceList" = '{1}'
WHERE 
(T2."ItemCode" LIKE 'AEA%' OR T2."ItemCode" LIKE 'ALM%') AND
T0."DocEntry" = '{0}'