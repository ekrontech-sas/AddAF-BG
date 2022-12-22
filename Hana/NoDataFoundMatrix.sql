SELECT
	ROW_NUMBER() OVER () AS "RowNumber",
	'No existen registros coincidentes' AS "Mensaje"
FROM DUMMY