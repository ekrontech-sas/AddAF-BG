SELECT
	T1."DocNum" AS "Contrato",
	T1."DocEntry" AS "Contrato Interno",
	T1."U_BGT_PerFact"  AS "Tipo de Facturacion",
	CURRENT_DATE AS "Fecha Factura",
	T1."U_BGT_CicloFact" AS "Ciclo Facturacion",
	(IFNULL((SELECT COUNT(1) FROM "@BGT_FCRD" WHERE "U_NroContrato" = T1."DocNum"),0)+1) AS "Cantidad Factura",
	T0."DocEntry" AS "Numero Documento"
FROM OINV T0
INNER JOIN "@BGT_OCTR" T1 ON T0."U_BGT_ContAsoc" = T1."DocNum"
WHERE
	T0."DocEntry" = '{0}'