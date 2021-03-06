WITH NamelyUser
AS (SELECT [Company email],
           [Country],
           [Region],
           [Studio Location],
           [User status],
           [Employment type]
    FROM
    (
        SELECT ReportRowId,
               ColumnValue,
               ColumnLabel
        FROM NamelyReportKeyValue
        WHERE LogDateTime =
        (
            SELECT 
                ISNULL
                ( 
                    CASE WHEN @LastProcessedDate IS NOT NULL 
				        THEN (SELECT MIN(LogDateTime) FROM NamelyReportKeyValue WHERE LogDateTime > @LastProcessedDate)
				        ELSE (SELECT MAX(LogDateTime) FROM NamelyReportKeyValue)
			        END, 
                    (SELECT MAX(LogDateTime) FROM NamelyReportKeyValue)
                )
        )
              AND ReportId = '53d717e8-3d3e-48cd-a53f-e12c007851a1'
              AND ColumnValue != ''
    ) x
    PIVOT
    (
        max(ColumnValue)
        FOR ColumnLabel IN ([Company email], [Country], [Region], [Studio Location], [User status], [Employment type])
    ) p)
SELECT COUNT([Company email]) EmailCount, Country, Region, COUNT([Company email]) * 8 as DailyStudioHours, COUNT([Company email]) * 8 * 5 as WeeklyStudioHours
FROM NamelyUser
WHERE 
[User status] = 'Active Employee' 
AND [Employment type] in ('Fixed term Contract', 'Max term Contract', 'Permanent')
AND [Region] = @Region
GROUP BY Region, Country
ORDER BY Country