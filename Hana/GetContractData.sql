SELECT
	replace(cast(cast("CreateDate" as date) as varchar(10)), '-', '') AS "CreateDate",
	"DocEntry" AS "Description"
FROM "@BGT_OCTR"
WHERE
	"DocNum" = '{0}'