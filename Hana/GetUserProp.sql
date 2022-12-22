select "empID", "lastName", "firstName", "middleName"
from "ZZZ_PRUEBAS_ADDON"."OHEM" where "userId"=(select T1."USERID" from "ZZZ_PRUEBAS_ADDON"."OUSR" T1
where "USER_CODE" = '{0}')
