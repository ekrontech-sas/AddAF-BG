SELECT 
	T1."U_BGT_SerieAF",
	T1."U_BGT_ClassAF"
FROM "@BGT_OPAR" T0 
INNER JOIN "@BGT_PAR1" T1 ON T0."Code" = T1."Code"
WHERE 
	T0."Code" = 'INIT_VAL' AND
	T1."U_BGT_SerieItem" = '{0}'