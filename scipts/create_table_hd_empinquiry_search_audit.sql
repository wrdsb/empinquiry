USE [HDHRP]
GO

/*
Author:		Meenakshi Durairaj 
Date:		2025-DEC-09
Purpose:	Table to store search parameters and user details while doing search in the empinq application
*/


SET ANSI_NULLS ON
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hd_empinquiry_search_audit]') AND type in (N'U'))
DROP TABLE [dbo].[hd_empinquiry_search_audit]
GO

CREATE TABLE [dbo].[hd_empinquiry_search_audit] (
    employee_id VARCHAR(9),
    userid VARCHAR(13),
    search_parameters VARCHAR(256),
    search_date VARCHAR(30)
);


GO