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
		select sfrstcr_term_code, sfrstcr_crn, sfrstcr_pidm
		from sfrstcr
		where sfrstcr_term_code = ( select min(stvterm_code) from stvterm
									where stvterm_end_date > sysdate
									  and stvterm_trmt_code in (''Q'', ''W'') 
								  )
		  and sfrstcr_rsts_code in (''RE'', ''RW'')
	')
END
