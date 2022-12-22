SELECT 
	"Series" AS "Value", 
	"SeriesName" AS "Description" 
FROM NNM1 
WHERE 
	"ObjectCode" = '4' AND 
	"Series" NOT IN (3)