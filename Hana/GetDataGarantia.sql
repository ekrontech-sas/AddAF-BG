SELECT 
   T0."DocNum" AS "Numero Garantia",
   T1."U_GRT_D_PLZ" AS "Plazo en Dias",
   T1."U_GRT_D_TIP",
   T2."Descr" AS "Tipo de Garantia",
   T1."U_GRT_D_EST",
   T3."Descr" AS "Estado de Garantia",
   T1."U_GRT_D_OBS" AS "Observaciones",
   T1."U_GRT_D_VALOR" AS "Valor",
   T1."U_GRT_D_OFI" AS "Sucursal",
   '' AS "Anexo" 
FROM "@BAG_GRT_CAB" T0 
INNER JOIN "@BAG_GRT_DET" T1 ON T0."DocEntry" = T1."DocEntry" 
LEFT JOIN UFD1 T2 ON T1."U_GRT_D_TIP" = T2."FldValue" AND  T2."TableID" = '@BAG_GRT_DET' AND T2."FieldID" = 2
LEFT JOIN UFD1 T3 ON T1."U_GRT_D_EST" = T3."FldValue" AND  T3."TableID" = '@BAG_GRT_DET' AND T3."FieldID" = 3
WHERE
	T0."U_GRT_C_CLI" = '{0}' AND
	T1."U_GRT_D_EST" = 'AR'