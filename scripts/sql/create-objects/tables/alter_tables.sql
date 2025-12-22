ALTER TABLE hd_empinquiry_user
ADD Id INT IDENTITY(1,1) PRIMARY KEY;

ALTER TABLE hd_empinquiry_audit
ADD Id INT IDENTITY(1,1) PRIMARY KEY;

ALTER TABLE hd_empinquiry_audit
ALTER COLUMN inquiry_date DATETIME NULL;

ALTER TABLE hd_empinquiry_audit
ADD inquiry_date_dt DATETIME NULL;

UPDATE hd_empinquiry_audit
SET inquiry_date_dt = TRY_PARSE(inquiry_date AS DATETIME USING 'en-US');

ALTER TABLE hd_empinquiry_audit
DROP COLUMN inquiry_date_dt;


