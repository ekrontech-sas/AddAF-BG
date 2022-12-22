select ifnull((SELECT sum(T2."Quantity") FROM "ODRF" T0  join "DRF1" T2 on T0."DocEntry"=T2."DocEntry" WHERE T0."U_BGT_ContAsoc" = '{0}'  
and T0."U_NUM_GUIA" = '{1}' ),-1) as "TotalNoGuia", 
(select "Code" from "@BGT_RECEPMAT"where "U_BGT_Contract"='{0}'  
and "U_BGT_DocEntry" = {2}) as "Code", 
(select "DocEntry" from "@BGT_OCTR" where "DocNum"  ='{0}'  ) as "DocEntryCto" 
from dummy;
