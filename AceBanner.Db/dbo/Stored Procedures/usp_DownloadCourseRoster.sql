-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE usp_DownloadCourseRoster
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	truncate table CourseRoster

    insert into CourseRoster
	select * from openquery (sis, '
		select sfrstcr_term_code, sfrstcr_crn, lower(wormoth_login_id) loginid
		from sfrstcr
			inner join wormoth on sfrstcr_pidm = wormoth_pidm
		where sfrstcr_term_code in ( select stvterm_code from stvterm
									where (stvterm_start_date < sysdate and stvterm_end_date > sysdate)
									   or (stvterm_start_date < sysdate and stvterm_end_date > sysdate - 14)
									)
			and sfrstcr_rsts_code in (''RE'', ''RW'')
			and wormoth_acct_type = ''Z''
			and wormoth_acct_status = ''A''
			and wormoth_activity_date = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''Z'' and iw.wormoth_acct_status = ''A'' )
	')
END
