-- =============================================
-- Author:		Alan Lai
-- Create date: Unknown
-- Description:	Download all course meeting sections for student service.
-- Usage:
/*
	USE StudentService
	GO

	EXEC [dbo].[usp_DownloadSections] @IsDebug = 1
	GO
*/
-- Modifications:
--	2019-05-14 by kjt: Split out term code where clause portion so that it might be
--	used by all five of the download stored procedures.
--
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadSections]
	( @IsDebug bit = 0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TSQL varchar(MAX) = ''

	SELECT @TSQL = '
	truncate table Sections

    insert into Sections
	select distinct newid(), ssrmeet_term_code, ssrmeet_crn, stvschd_desc
		, cast(ssrmeet_start_date as date) startdate, cast(ssrmeet_end_date as date) enddate
		, dbo.ConvertToTime(ssrmeet_begin_time) begintime
		, dbo.ConvertToTime(ssrmeet_end_time) endtime
		, daysofweek
	from openquery(sis, ''
		select ssrmeet_term_code, ssrmeet_crn, stvschd_desc
			, ssrmeet_start_date, ssrmeet_end_date
			, ssrmeet_begin_time, ssrmeet_end_time
			, nvl(ssrmeet_mon_day, '''''''') || nvl(ssrmeet_tue_day, '''''''') || nvl(ssrmeet_wed_day, '''''''') || nvl(ssrmeet_thu_day, '''''''') || nvl(ssrmeet_fri_day, '''''''') daysofweek
		from ssrmeet
			inner join stvschd on ssrmeet_schd_code = stvschd_code
		where ssrmeet_term_code in (' 
		
	SELECT @TSQL += (SELECT [dbo].[udf_TermCodeFilterString]())
		
	SELECT @TSQL += '			 )
	'')
'
	IF @IsDebug = 1
		PRINT @TSQL
	ELSE
		EXEC (@TSQL)
                                 
END
