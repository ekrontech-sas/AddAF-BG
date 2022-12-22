SELECT 
	"SlpCode", 
	"SlpName" 
FROM OSLP 
WHERE 
	"Locked" = 'N' AND 
	"Active" = 'Y'