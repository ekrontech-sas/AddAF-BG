SELECT
	T0."ItemCode",
	T0."ItemName",
	T1."Price",
	(SELECT "Price" FROM ITM1 X0 WHERE X0."ItemCode" = T0."ItemCode" AND "PriceList" = '2') AS "PriceVenta"
FROM OITM T0
INNER JOIN ITM1 T1 ON T0."ItemCode" = T1."ItemCode" AND T1."PriceList" = '{1}'
WHERE
	T0."U_BAG_DIST_TPE" = '{0}' AND
	T0."U_IXX_Suc_OCod" = '{2}'