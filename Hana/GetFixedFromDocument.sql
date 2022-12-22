SELECT
	T2."U_AGT_ItemAsoc" AS "Asset",
	T1."Quantity",
	T1."Price"
FROM {0} T0
INNER JOIN {1} T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN OITM T2 ON T1."ItemCode" = T2."ItemCode"
WHERE
T0."DocEntry" = '{2}' AND IFNULL(T2."U_AGT_ItemAsoc",'') <> ''