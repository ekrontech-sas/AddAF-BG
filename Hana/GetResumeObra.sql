SELECT
	T0."OpprId",
	T0."OpenDate",
	T0."U_BAG_MGN_CUAL" AS "Magnitud Cualitativa",
	T0."U_BAG_MGN_CUAN" AS "Magnitud Cuantitativa",
	T0."U_BAG_SIS_CONS" AS "Sistema Constructivo",
	T0."U_BAG_ETP_OBR"  AS "Etapa de Obra",
	0 AS "Numero de Periodos Facturados",
	0 AS "% Equipo en obra",
	0 AS "Facturado total a la fecha",
	0 AS "No. Egresos"
FROM OOPR T0
INNER JOIN OPR1 T1 ON T1."OpprId" = T0."OpprId" AND T1."ObjType"='23'
LEFT JOIN OQUT T2 on T2."DocEntry" = T1."DocId"
WHERE
	T2."DocEntry" = '{0}'