SELECT
	T0."ListNum" AS "Value",
	T0."ListName" AS "Description"
FROM OPLN T0
INNER JOIN OUBR T1 ON T0."U_BAG_SUCUR" = T1."Name"
INNER JOIN OUSR T2 ON T1."Code" = T2."Branch"
WHERE	
	T0."ValidFor" = 'Y' AND 
	T0."U_BAG_TIPO" = 'A' AND 
	T2."USERID" = '{0}'