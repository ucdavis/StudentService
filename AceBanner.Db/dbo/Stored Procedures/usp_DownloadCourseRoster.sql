-- =============================================
-- Author:		Alan Lai
-- Create date: 
-- Description:	Download course roster for student service.
-- Usage:
/*
	USE StudentService
	GO

	EXEC [dbo].[usp_DownloadCourseRoster] @IsDebug = 1
	GO
*/
-- Modifications:
--	2019-05-13 by kjt: Converted to dynamic SQL so that universal term code portion of where clause
--	  could be pulled from udf_TermCodeFilterString and used by all stored procedures requiring it.
--
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadCourseRoster]
	( @IsDebug bit = 0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TSQL varchar(MAX) = ''

	SELECT @TSQL = '
	truncate table CourseRoster

    insert into CourseRoster
	select * from openquery (sis, ''
		select sfrstcr_term_code, sfrstcr_crn, lower(wormoth_login_id) loginid
		from sfrstcr
			inner join wormoth on sfrstcr_pidm = wormoth_pidm
		where sfrstcr_term_code in ('

	SELECT @TSQL += (SELECT [dbo].[udf_TermCodeFilterString]())

	SELECT @TSQL += '			 )
			and sfrstcr_rsts_code in (''''RE'''', ''''RW'''')
			and wormoth_acct_type = ''''Z''''
			and wormoth_acct_status = ''''A''''
			and wormoth_activity_date = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''''Z'''' and iw.wormoth_acct_status = ''''A'''' )
	'')
'
	IF @IsDebug = 1
		PRINT @TSQL
	ELSE
		EXEC (@TSQL)

END
