USE [HDHRP]
GO

/*
Author:		Meenakshi Durairaj 
Date:		2026-JUN-05
Purpose:	Table to store smartphone orders of the employee
*/


SET ANSI_NULLS ON
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hd_empinquiry_smartphone]') AND type in (N'U'))
DROP TABLE [dbo].[hd_empinquiry_smartphone]
GO



CREATE TABLE hd_empinquiry_smartphone
(   
      employee_id VARCHAR(9) NOT NULL    
    , employee_name VARCHAR(90) NOT NULL
    , order_date DATE NULL
    , phone_number VARCHAR(50) NULL
    , tier VARCHAR(10) NULL   -- Tier1/Tier2/Tier3/Tier4
    , ordered_item VARCHAR(20) NULL  -- Sim / Phone
    , rogers_account_created BIT NULL
    , board_contribution_paid BIT NULL
    , next_eligible_date DATE NULL
    , form_link VARCHAR(1000) NULL
    , notes VARCHAR(MAX) NULL
    , created_date DATETIME DEFAULT GETDATE()
    , Id INT IDENTITY(1,1) PRIMARY KEY
);

-- TODO - Need to modify the table to add the below columns in future

CREATE TABLE hd_empinquiry_smartphone
(
      Id INT IDENTITY(1,1) PRIMARY KEY

      , employee_id VARCHAR(9) NOT NULL
      , employee_name VARCHAR(90) NOT NULL
      , job_description VARCHAR(150) NULL

      , order_date DATE NULL
      , phone_number VARCHAR(50) NULL

      , tier VARCHAR(10) NULL            -- Tier1/Tier2/Tier3/Tier4
      , ordered_item VARCHAR(20) NULL    -- Sim / Phone

      , rogers_account_created BIT NULL
      , board_contribution_paid BIT NULL

      , next_eligible_date DATE NULL

      , form_link VARCHAR(1000) NULL
      , notes VARCHAR(MAX) NULL

      , created_by VARCHAR(50) NULL
      , created_date DATETIME NOT NULL DEFAULT GETDATE()

      , modified_by VARCHAR(50) NULL
      , modified_date DATETIME NULL
);





GO
