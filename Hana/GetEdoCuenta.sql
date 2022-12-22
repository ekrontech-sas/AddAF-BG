SELECT 'Factura' AS "Tipo de Documento", 
 T0."DocEntry" AS "Numero Interno", 
 T0."DocNum" AS "Numero Factura/NC", 
 concat(concat(TO_NVARCHAR(T2."U_BGT_FecIni",'DD/MM/YYYY'), ' - '), TO_NVARCHAR(T2."U_BGT_FecFin", 'DD/MM/YYYY')) AS "Periodo Facturado Alquiler/NC", 
 T0."DocTotal" AS "Monto Facturado", 
 IFNULL((SELECT SUM(X1."SumApplied") FROM ORCT X0 INNER JOIN RCT2 X1 ON X0."DocEntry" = X1."DocNum" WHERE X1."DocEntry" = T0."DocEntry"),0) AS "Monto Abonado", 
 0 AS "Saldo Linea", 
 0 AS "Saldo Total Obra" 
FROM "OINV" T0 
INNER JOIN "@BGT_OCTR" T1 ON T0."U_BGT_ContAsoc" = T1."DocNum" 
LEFT JOIN "@BGT_DINV" T2 ON T0."DocEntry" = T2."U_BGT_DocEntryInv" 
WHERE T1."DocEntry" = '{0}' 
  UNION 
SELECT 'NC' AS "Tipo de Documento",
 T0."DocEntry" AS "Numero Interno",
 T0."DocNum" AS "Numero Factura/NC",
 '' AS "Periodo Facturado Alquiler/NC",
 T0."DocTotal" AS "Monto Facturado",
 0 AS "Monto Abonado",
 0 AS "Saldo Linea",
 0 AS "Saldo Total Obra" 
FROM "ORIN" T0 
INNER JOIN "@BGT_OCTR" T1 ON T0."U_BGT_ContAsoc" = T1."DocNum" 
WHERE T1."DocEntry" = '{0}' 