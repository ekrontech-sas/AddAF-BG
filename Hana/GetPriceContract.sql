SELECT 
	"Price",
	(SELECT "Price" FROM ITM1 X0 WHERE X0."ItemCode" = T0."ItemCode" AND "PriceList" = '2') AS "PriceVenta"
FROM ITM1 T0 
WHERE 
	"ItemCode" = '{0}' AND 
	"PriceList" = '{1}'