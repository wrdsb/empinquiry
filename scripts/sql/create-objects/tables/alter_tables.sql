ALTER TABLE hd_empinquiry_user
ADD Id INT IDENTITY(1,1) PRIMARY KEY;

ALTER TABLE hd_empinquiry_audit
ADD Id INT IDENTITY(1,1) PRIMARY KEY;

ALTER TABLE hd_empinquiry_audit
ALTER COLUMN inquiry_date DATETIME NULL;



