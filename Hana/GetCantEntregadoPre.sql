SELECT T3."LineId",
ifnull(T3."U_BGT_LineNum", T3."LineId") as "U_BGT_LineNum",
T1."ItemCode",
T3."U_BGT_CantContr",
T3."U_BGT_CantObra",
T3."U_BGT_PrecAlq" AS "Precio",
T3."U_BGT_ItemKg" AS "Peso",
T3."U_BGT_CantEntr" AS "Pendiente",
(T1."Quantity"+T3."U_BGT_CantEntreg") AS "CantEnt",
(T3."U_BGT_CantContr"-(T3."U_BGT_CantEntreg"+T1."Quantity")) AS "CantPend",
((T3."U_BGT_CantObra" + T1."Quantity")) AS "CantObra",
(T3."U_BGT_CantEntr"-(T1."Quantity"+T3."U_BGT_CantEntreg")) AS "CantContr",
(((T3."U_BGT_CantObra" + T1."Quantity") - T3."U_BGT_CantDev")*"U_BGT_PrecAlq") AS "SubObra",
(T3."U_BGT_SubTotalCon"-(((T3."U_BGT_CantObra" + T1."Quantity") - T3."U_BGT_CantDev")*"U_BGT_PrecAlq")) AS "SubEntr",
T3."U_BGT_PrecVenta" AS "PrecVta" 
FROM ODRF T0 
INNER JOIN DRF1 T1 ON T0."DocEntry" = T1."DocEntry" 
INNER JOIN "@BGT_OCTR" T2 ON T0."U_BGT_ContAsoc" = T2."DocNum"  
INNER JOIN "@BGT_CTR1" T3 ON T2."DocEntry" = T3."DocEntry" AND T1."ItemCode" = T3."U_BGT_CodAlq" 
WHERE T0."DocEntry" = '{0}' AND T1."ItemCode" LIKE 'AEA%' 
UNION 
SELECT T3."LineId",
ifnull(T3."U_BGT_LineNum", T3."LineId") as "U_BGT_LineNum",
T1."ItemCode",
T3."U_BGT_CantContr",
T3."U_BGT_CantObra",
T3."U_BGT_PrecAlq" AS "Precio",
T3."U_BGT_ItemKg" AS "Peso",
T3."U_BGT_CantEntr" AS "Pendiente",
(T1."Quantity"+T3."U_BGT_CantEntreg") AS "CantEnt",
0 AS "CantPend", 
(T1."Quantity"+T3."U_BGT_CantEntreg") AS "CantObra",
(T3."U_BGT_CantEntr"-(T1."Quantity"+T3."U_BGT_CantEntreg")) AS "CantContr",
(((T3."U_BGT_CantObra" + T1."Quantity") - T3."U_BGT_CantDev")*"U_BGT_PrecAlq") AS "SubObra",
(T3."U_BGT_SubTotalCon"-(((T3."U_BGT_CantObra" + T1."Quantity") - T3."U_BGT_CantDev")*"U_BGT_PrecAlq")) AS "SubEntr",
T3."U_BGT_PrecVenta" 
FROM ODRF T0 
INNER JOIN DRF1 T1 ON T0."DocEntry" = T1."DocEntry" 
INNER JOIN "@BGT_OCTR" T2 ON T0."U_BGT_ContAsoc" = T2."DocNum"  
INNER JOIN "@BGT_CTR1" T3 ON T2."DocEntry" = T3."DocEntry" AND T1."ItemCode" = T3."U_BGT_CodAlq" 
WHERE T0."DocEntry" =  '{0}' AND T1."ItemCode" LIKE 'LOG%'