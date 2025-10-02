USE [HDHRP]
GO

/*
Author:		Meenakshi Durairaj 
Date:		2025-OCT-02
Purpose:	Table to store user details who is doing the employee search
*/


SET ANSI_NULLS ON
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hd_empinquiry_audit]') AND type in (N'U'))
DROP TABLE [dbo].[hd_empinquiry_audit]
GO

CREATE TABLE [dbo].[hd_empinquiry_audit] (
    employee_id VARCHAR(9),
    firstname VARCHAR(30),
    surname VARCHAR(30),   
    emailaddress VARCHAR(60),
    userid VARCHAR(13),
    Purpose VARCHAR(256),
    inquiry_date DATE
);


GO
