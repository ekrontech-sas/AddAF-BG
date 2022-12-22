SELECT 
	T1."Name" 
FROM OUSR T0 
INNER JOIN OUBR T1 ON T0."Branch" = T1."Code" 
WHERE 
	T0."USERID" = '{0}'