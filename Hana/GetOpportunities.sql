SELECT 
	"ClgCode", 
	"DocNum", 
	"DocEntry", 
	"DocType" 
FROM OCLG 
WHERE 
	"DocType" = '23' AND 
	"DocEntry" = '{0}'