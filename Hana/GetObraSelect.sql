SELECT 
	T0."Address2",
	T1."PrjName"
FROM CRD1 T0
INNER JOIN OPRJ T1 ON T0."Address2" = T1."PrjCode"
WHERE 
	T0."CardCode" = '{0}' AND 
	T0."Address" = '{1}'