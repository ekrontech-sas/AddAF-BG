
select T1."USERID", T1."USER_CODE", T1."U_NAME", T1."SUPERUSER",
	ifnull(T2."UserLink", T1."USERID") as "UserLink", ifnull(T2."PermId",'BAG_GRTS_CLTS') as "PermId",
	ifnull(T2."Permission", case when T1."SUPERUSER"='Y' then 'F' else 'R' end) as "Permission"
from "ZZZ_PRUEBAS_ADDON"."OUSR" T1
left join "ZZZ_PRUEBAS_ADDON"."USR3" T2 on T1."USERID" = T2."UserLink" and "PermId"='BAG_GRTS_CLTS'
where "USER_CODE" = '{0}'