-- =============================================
-- Author:		Alan Lai
-- Create date: Unknown
-- Description:	Download instructors for student service.
-- Usage:
/*
	USE StudentService
	GO

	EXEC [dbo].[usp_DownloadInstructors] @IsDebug = 1
	GO
*/
-- Modifications:
--	2019-05-14 by kjt: Split out term code where clause portion so that it might be
--	used by all five of the download stored procedures.
--
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadInstructors]
	( @IsDebug bit = 0)
AS
BEGIN
	DECLARE @TSQL varchar(MAX) = ''

	SELECT @TSQL = '
	DECLARE @tmp TABLE (
		termcode int,
		crn int,
		firstname varchar(50),
		lastname varchar(50),
		pidm int,
		loginid varchar(20),
		email varchar(50),
		primaryind bit
	)

	insert into @tmp
	select * from openquery (sis, ''
		select zsvinst_term_code, zsvinst_crn
			, spriden_first_name, spriden_last_name, spriden_pidm
			, lower(wormoth_login_id) loginid, emails.address
			, (case when zsvinst_primary_ind = ''''Y'''' then 1 else 0 end) as primaryind
		from zsvinst
			inner join spriden on zsvinst_id = spriden_id
			left outer join wormoth on wormoth_pidm = spriden_pidm
			left outer join (
				select goremal_pidm as pidm, goremal_email_address as address
				from goremal
				where goremal_status_ind = ''''A''''
					and goremal_emal_code = ''''UCD''''
			) Emails on Emails.pidm = spriden_pidm
		where zsvinst_term_code in ('
	
	SELECT @TSQL += (SELECT [dbo].[udf_TermCodeFilterString]())

	SELECT @TSQL +=	  '		  )
		  and zsvinst_id is not null
		  and wormoth_acct_type = ''''Z''''
		  and wormoth_acct_status = ''''A''''
		  and spriden_change_ind is null
		  and WORMOTH_ACTIVITY_DATE = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''''Z'''' and iw.wormoth_acct_status = ''''A'''' )
	'')
'
	SELECT @TSQL += '
	truncate table Instructors
	truncate table CourseInstructors

	insert into Instructors
	select distinct 
		loginid, firstname, lastname, email, pidm 
	from @tmp

	insert into CourseInstructors
	SELECT
		 [termcode]
		,[crn]
		,[loginid]
		,[primaryind]
	FROM (
			SELECT *, 
				ROW_NUMBER() OVER (
					PARTITION BY 
						 [termcode]
						,[crn]
						,[loginid] 
					ORDER BY 
						[primaryind] DESC) AS RowNumber
			FROM @tmp
		  ) source
	WHERE RowNumber = 1
	ORDER BY 
		[termcode],[crn],[loginid]
'
	IF @IsDebug = 1
		PRINT @TSQL
	ELSE
		EXEC (@TSQL)

	RETURN 0
END