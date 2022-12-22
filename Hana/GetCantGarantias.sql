SELECT count(*) as "TotalGarantias"
FROM "@BAG_GRT_CAB" T0 
INNER JOIN "@BAG_GRT_DET" T1 ON T0."DocEntry" = T1."DocEntry" 
WHERE
	T0."U_GRT_C_CLI" = '{0}' AND
	T1."U_GRT_D_EST" = 'AR'
