select ifnull((SELECT count(*) FROM "ZZZ_PRUEBAS_ADDON"."ODRF" T0 WHERE T0."U_BGT_ContAsoc" = '{0}' 
and T0."U_NUM_GUIA" = '{1}' ),-1) as "TotalNoGuia",
(select "Code" from "ZZZ_PRUEBAS_ADDON"."@BGT_RECEPMAT"where "U_BGT_Contract"='{0}' 
and "U_BGT_DocEntry"=((select "draftKey" from "ZZZ_PRUEBAS_ADDON"."ODLN" where "DocEntry"={2}))) as "Code" from dummy;
