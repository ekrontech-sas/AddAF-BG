SELECT 
     T1."U_BGT_Serie" AS "Serie"
FROM "@BGT_OCTR" T0 
INNER JOIN "@BGT_PAR5" T1 ON T0."U_BGT_CodSurc" = T1."U_BGT_CodSurc"
WHERE T0."DocNum" = {0}
AND T1."U_BGT_TipDoc" IN ({1})