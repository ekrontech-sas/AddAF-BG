SELECT
        MAX(T3."LineId") AS "LineId",
        MAX(ifnull(T3."U_BGT_LineNum",T3."LineId")) AS "U_BGT_LineNum",
        T1."ItemCode",
		T3."U_BGT_PrecAlq" AS "Precio",
		T3."U_BGT_ItemKg" AS "Peso",
		T3."U_BGT_CantEntr" AS "Pendiente",
        SUM(T1."Quantity")+T3."U_BGT_CantDev" AS "CantDev",
		T3."U_BGT_CantEntreg"-SUM(T1."Quantity") AS "CantPend",
		T3."U_BGT_CantEntreg" - SUM(T1."Quantity"+T3."U_BGT_CantDev")  AS "CantObraCheck",
        SUM(T3."U_BGT_CantObra" - (T1."Quantity"+T3."U_BGT_CantDev"))  AS "CantObra",
		--SUM((T3."U_BGT_CantObra" - (T1."Quantity"+T3."U_BGT_CantDev"))*"U_BGT_PrecAlq") AS "SubObra"
		((T3."U_BGT_CantEntreg" - SUM(T1."Quantity"+T3."U_BGT_CantDev"))*"U_BGT_PrecAlq") AS "SubObra"
FROM ORDN T0
INNER JOIN RDN1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN "@BGT_OCTR" T2 ON T0."U_BGT_ContAsoc" = T2."DocNum" 
INNER JOIN "@BGT_CTR1" T3 ON T2."DocEntry" = T3."DocEntry" AND T1."ItemCode" = T3."U_BGT_CodAlq"
WHERE
T0."DocEntry" = '{0}' AND
T1."ItemCode" LIKE 'AEA%'
GROUP BY 
	 T1."ItemCode",
	 T3."U_BGT_PrecAlq",
	 T3."U_BGT_CantEntr",
	 T3."U_BGT_PrecAlq",
	 T3."U_BGT_ItemKg",
	 T3."U_BGT_CantEntreg",
	 T3."U_BGT_CantDev"
UNION
SELECT
        MAX(T3."LineId") AS "LineId",
        MAX(ifnull(T3."U_BGT_LineNum",T3."LineId")) AS "U_BGT_LineNum",
        T1."ItemCode",
		T3."U_BGT_PrecAlq" AS "Precio",
		T3."U_BGT_ItemKg" AS "Peso",
		T3."U_BGT_CantEntr" AS "Pendiente",
        SUM(T1."Quantity"+T3."U_BGT_CantDev") AS "CantDev",
		 0 AS "CantPend",
		 0,
        SUM(T3."U_BGT_CantObra" + (T1."Quantity"+T3."U_BGT_CantDev"))  AS "CantObra",
		SUM((T3."U_BGT_CantObra" + (T1."Quantity"+T3."U_BGT_CantDev"))*"U_BGT_PrecAlq") AS "SubObra"
FROM ORDN T0
INNER JOIN RDN1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN "@BGT_OCTR" T2 ON T0."U_BGT_ContAsoc" = T2."DocNum" 
INNER JOIN "@BGT_CTR1" T3 ON T2."DocEntry" = T3."DocEntry" AND T1."ItemCode" = T3."U_BGT_CodAlq"
WHERE
T0."DocEntry" = '{0}' AND
T1."ItemCode" LIKE 'LOG%'
GROUP BY 
	 T1."ItemCode",
	 T3."U_BGT_PrecAlq",
	 T3."U_BGT_CantEntr",
	 T3."U_BGT_PrecAlq",
	 T3."U_BGT_ItemKg",
	 T3."U_BGT_CantEntr"

/*
SELECT
        T3."LineId",
        T3."U_BGT_LineNum",
        T1."ItemCode",
		T3."U_BGT_PrecAlq" AS "Precio",
		T3."U_BGT_ItemKg" AS "Peso",
		T3."U_BGT_CantEntr" AS "Pendiente",
        (T1."Quantity"+T3."U_BGT_CantDev") AS "CantDev",
		(T3."U_BGT_CantEntreg"-(T1."Quantity"+T3."U_BGT_CantDev")) AS "CantPend",
        (T3."U_BGT_CantObra" - (T1."Quantity"+T3."U_BGT_CantDev"))  AS "CantObra",
		((T3."U_BGT_CantObra" - (T1."Quantity"+T3."U_BGT_CantDev"))*"U_BGT_PrecAlq") AS "SubObra"
FROM ORDN T0
INNER JOIN RDN1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN "@BGT_OCTR" T2 ON T0."U_BGT_ContAsoc" = T2."DocNum" 
INNER JOIN "@BGT_CTR1" T3 ON T2."DocEntry" = T3."DocEntry" AND T1."ItemCode" = T3."U_BGT_CodAlq"
WHERE
T0."DocEntry" = '{0}' AND
T1."ItemCode" LIKE 'AEA%'
UNION
SELECT
        T3."LineId",
        T3."U_BGT_LineNum",
        T1."ItemCode",
		T3."U_BGT_PrecAlq" AS "Precio",
		T3."U_BGT_ItemKg" AS "Peso",
		T3."U_BGT_CantEntr" AS "Pendiente",
        (T1."Quantity"+T3."U_BGT_CantDev") AS "CantDev",
		 0 AS "CantPend",
        (T3."U_BGT_CantObra" + (T1."Quantity"+T3."U_BGT_CantDev"))  AS "CantObra",
		((T3."U_BGT_CantObra" + (T1."Quantity"+T3."U_BGT_CantDev"))*"U_BGT_PrecAlq") AS "SubObra"
FROM ORDN T0
INNER JOIN RDN1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN "@BGT_OCTR" T2 ON T0."U_BGT_ContAsoc" = T2."DocNum" 
INNER JOIN "@BGT_CTR1" T3 ON T2."DocEntry" = T3."DocEntry" AND T1."ItemCode" = T3."U_BGT_CodAlq"
WHERE
T0."DocEntry" = '{0}' AND
T1."ItemCode" LIKE 'LOG%'
*/