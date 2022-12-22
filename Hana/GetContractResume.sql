SELECT
	T0."DocNum" AS "Numero Contrato",
	T0."DocEntry" AS "Numero Interno",
	T0."U_BGT_ProjectName" AS "Nombre de Obra",
	T0."CreateDate" AS "Fecha",
	T0."U_BGT_DocDateDesp" AS "Fecha de Primer Despacho",
	T0."U_BGT_DurCont" AS "Meses duracion contrato",
    T4."Remarks" AS "Sucursal",
	T3."Descr" AS "Estado",
	--T0."U_BGT_CardCode" AS "Socio de Negocio",
	--T1."CardName" AS "Nombre Socio de Negocio",
	T2."DocEntry" AS "Num. Interno Oferta",
	T2."DocNum" AS "Num. Oferta",
	T0."U_BGT_ValRepEqOb" AS "Valor Rep. Equip. Obra",
	T0."U_BGT_ValRepCont" AS "Valor Rep. Contrato",
	--T0."U_BGT_ValGarant" AS "Valor de Garantia",
	T0."U_BGT_SubPesObra" 
	 AS "Subtotal Peso Obra",
	T0."U_BGT_SubPesEnt" AS "Subtotal peso por Entregar"
FROM "@BGT_OCTR" T0
INNER JOIN OCRD T1 ON T0."U_BGT_CardCode" = T1."CardCode"
LEFT JOIN OQUT T2 ON T0."U_BGT_DocEntryOF" = T2."DocEntry"
LEFT JOIN UFD1 T3 ON T0."U_BGT_Status" = T3."FldValue" AND "TableID" = '@BGT_OCTR' AND "FieldID" = '15'
LEFT JOIN OUBR T4 ON T0."U_BGT_CodSurc" = T4."Name"
WHERE
	T1."CardCode" = '{0}' AND
	T0."DocEntry" <> '{1}'