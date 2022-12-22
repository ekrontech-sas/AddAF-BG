SELECT 
	"Address" AS "Value", 
	"Street" AS "Description" 
FROM CRD1 
WHERE 
	"AdresType" = 'B' AND 
	"CardCode" = '{0}'