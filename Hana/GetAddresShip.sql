SELECT 
	"Address" AS "Value", 
	"Street" AS "Description" 
FROM CRD1 
WHERE 
	"AdresType" = 'S' AND 
	"CardCode" = '{0}'