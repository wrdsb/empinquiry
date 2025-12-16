USE [HDHRP]
GO

/*
Author:		Meenakshi Durairaj 
Date:		2025-OCT-02
Purpose:	Table will contain authorized/admin person records who is allowed to do search
*/


SET ANSI_NULLS ON
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hd_empinquiry_user]') AND type in (N'U'))
DROP TABLE [dbo].[hd_empinquiry_user]
GO

 CREATE TABLE [dbo].[hd_empinquiry_user] (
    employee_id VARCHAR(9) UNIQUE  NOT NULL
    , userid VARCHAR(13) UNIQUE  NOT NULL
    , admin BIT
);

GO
