SELECT
	ROW_NUMBER() OVER (ORDER BY T0."DocNum") AS "RowNumber",
	T0."DocNum" AS "Numero Contrato",
	T0."DocEntry" AS "Interno Contrato",
	CURRENT_DATE AS "Fecha Contrato",
	CURRENT_DATE AS "Fecha Facturacion",
	CAST('' AS NVARCHAR(250)) AS "Comentarios",
	CAST('' AS NVARCHAR(100)) AS "Propietario",
	CAST('' AS NVARCHAR(40)) AS "Numero Guia Remision",
	CAST('' AS NVARCHAR(40)) AS "Transportista",
	CAST('' AS NVARCHAR(30)) AS "Placa Camion",
	CAST('' AS NVARCHAR(100)) AS "Interno Propietario"
FROM "@BGT_OCTR" T0
WHERE 
	T0."DocNum" IN ({0})
ORDER BY T0."DocNum"