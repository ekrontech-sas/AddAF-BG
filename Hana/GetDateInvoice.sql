﻿SELECT  
	LAST_DAY(CURRENT_DATE) AS "Ultimo dia mes",  
	ADD_MONTHS(ADD_DAYS(LAST_DAY(CURRENT_DATE), 1), -1) AS "Primer dia mes", 
	(DAYS_BETWEEN (ADD_MONTHS(ADD_DAYS(LAST_DAY(CURRENT_DATE), 1), -1), LAST_DAY(CURRENT_DATE))+1) AS "Cant Dias" 
FROM DUMMY;