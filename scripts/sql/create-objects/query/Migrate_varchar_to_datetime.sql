
--Step 1: Check which rows will fail (important)

SELECT inquiry_date
FROM hd_empinquiry_audit
WHERE inquiry_date IS NOT NULL
AND TRY_PARSE(inquiry_date AS DATETIME USING 'en-US') IS NULL;

--Step 2: Convert existing VARCHAR values to DATETIME

UPDATE hd_empinquiry_audit
SET inquiry_date = TRY_PARSE(inquiry_date AS DATETIME USING 'en-US');
