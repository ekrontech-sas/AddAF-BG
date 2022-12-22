SELECT 
	"Address2" 
FROM CRD1 
WHERE 
	"CardCode" = '{0}' AND 
	IFNULL("Address2",'') <> ''