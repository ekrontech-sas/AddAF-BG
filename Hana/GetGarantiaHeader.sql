SELECT 
	(SELECT SUM("U_BGT_CantObra"*"U_BGT_PrecVenta") FROM "@BGT_CTR1" WHERE "DocEntry" = {0} AND "U_BGT_CodAlq" NOT LIKE 'LOG%') AS "GarConsumObra",
	( 
		SELECT SUM(IFNULL(X1."U_BGT_CantContr",0)*IFNULL(X1."U_BGT_PrecVenta",0))
		FROM "@BGT_CTR1" X1
		INNER JOIN "@BGT_OCTR" X0 ON X0."DocEntry" = X1."DocEntry"
		WHERE
			X0."U_BGT_CardCode" = '{1}'
	) AS "GarConsumCont",
	(
		IFNULL((SELECT SUM(X1."U_GRT_D_VALOR") FROM "@BAG_GRT_CAB" X0 INNER JOIN "@BAG_GRT_DET" X1 ON X0."DocEntry" = X1."DocEntry" WHERE X0."U_GRT_C_CLI" = MAX(T0."U_BGT_CardCode") AND X1."U_GRT_D_EST" = 'AR'),0)
			-
		IFNULL((SELECT SUM(IFNULL(X1."U_BGT_CantContr",0)*IFNULL(X1."U_BGT_PrecVenta",0)) FROM "@BGT_CTR1" X1 INNER JOIN "@BGT_OCTR" X0 ON X0."DocEntry" = X1."DocEntry" WHERE	X0."U_BGT_CardCode" = '{1}'	),0)
	) AS "GarCup"
FROM "@BGT_OCTR" T0
INNER JOIN "@BGT_CTR1" T1 ON T0."DocEntry" = T1."DocEntry"
WHERE
	T0."DocEntry" = {0};