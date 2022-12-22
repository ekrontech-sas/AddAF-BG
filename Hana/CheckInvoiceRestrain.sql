SELECT 
       COUNT(1) AS "Cant",
       (
          CASE 
                  WHEN (SELECT COUNT(1) FROM "@BGT_PAR6" WHERE "U_BGT_UsrId" = '{0}') = 0 THEN -1
                  ELSE 1
          END
       ) AS "Tipo"
FROM "@BGT_DINV" T0
WHERE
"U_BGT_MesFact" = MONTH(CURRENT_DATE)