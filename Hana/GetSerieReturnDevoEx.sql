SELECT 
   T0."U_BGT_TipDoc" AS "Series"
FROM "@BGT_PAR5" T0
INNER JOIN OUBR T1 ON T0."U_BGT_CodSurc" = T1."Name" 
INNER JOIN OUSR T2 ON T1."Code" = T2."Branch"
WHERE
	T2."USERID" = '{0}' AND 
	T0."U_BGT_TipDoc" LIKE '%EX'
