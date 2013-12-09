-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadSections]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	truncate table Sections

    insert into Sections
	select distinct newid(), ssrmeet_term_code, ssrmeet_crn, stvschd_desc
		, cast(ssrmeet_start_date as date) startdate, cast(ssrmeet_end_date as date) enddate
		, dbo.ConvertToTime(ssrmeet_begin_time) begintime
		, dbo.ConvertToTime(ssrmeet_end_time) endtime
		, daysofweek
	from openquery(sis, '
		select ssrmeet_term_code, ssrmeet_crn, stvschd_desc
			, ssrmeet_start_date, ssrmeet_end_date
			, ssrmeet_begin_time, ssrmeet_end_time
			, nvl(ssrmeet_mon_day, '''') || nvl(ssrmeet_tue_day, '''') || nvl(ssrmeet_wed_day, '''') || nvl(ssrmeet_thu_day, '''') || nvl(ssrmeet_fri_day, '''') daysofweek
		from ssrmeet
			inner join stvschd on ssrmeet_schd_code = stvschd_code
		where ssrmeet_term_code in ( select stvterm_code from stvterm
										where (stvterm_start_date < sysdate and stvterm_end_date > sysdate)
										   or (stvterm_start_date < sysdate and stvterm_end_date > sysdate - 21)
									  )
	')

END
