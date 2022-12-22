SELECT 
	T0."U_IXX_HOLD",
	T1."CardName"
FROM OCRD T0
LEFT JOIN OCRD T1 ON T0."U_IXX_HOLD" = T1."CardCode" 
WHERE 
	T0."CardCode" = '{0}'