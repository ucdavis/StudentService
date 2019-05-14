-- =============================================
-- Author:		Alan Lai
-- Create date: Unknown
-- Description:	Download students for student service.
-- Usage:
/*
	USE StudentService
	GO

	EXEC [dbo].[usp_DownloadStudents] @IsDebug = 1
	GO
*/
-- Modifications:
--	2019-05-14 by kjt: Split out term code where clause portion so that it might be
--	used by all five of the download stored procedures.
--
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadStudents]
	( @IsDebug bit = 0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TSQL varchar(MAX) = ''

	SELECT @TSQL = '
    truncate table Students

	insert into Students
	select loginid, firstname, lastname, pidm, studentid, email, ''B''
	from openquery (sis, ''
		select lower(wormoth_login_id) as loginid 
			, spriden_first_name as firstname, spriden_last_name as lastname, spriden_pidm as pidm, spriden_id as studentid
			, emails.address as email
		from spriden
			inner join wormoth on spriden_pidm = wormoth_pidm
			left outer join (
				select goremal_pidm as pidm, goremal_email_address as address
				from goremal
				where goremal_status_ind = ''''A''''
					and goremal_emal_code = ''''UCD''''
			) Emails on Emails.pidm = spriden_pidm
		where spriden_change_ind is null
		  and spriden_pidm in (
				select sfrstcr_pidm
				from sfrstcr
				where sfrstcr_term_code in (' 

	SELECT @TSQL += (SELECT [dbo].[udf_TermCodeFilterString]())

	SELECT @TSQL += '			 )
				  and sfrstcr_rsts_code in (''''RE'''', ''''RW'''')
		  )
		  and wormoth_acct_type = ''''Z''''
		  and wormoth_acct_status = ''''A''''
		  and WORMOTH_ACTIVITY_DATE = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''''Z'''' and iw.wormoth_acct_status = ''''A'''' )
	'')
'
	IF @IsDebug = 1
		PRINT @TSQL
	ELSE
		EXEC (@TSQL)

END
