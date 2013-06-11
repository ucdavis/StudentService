-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE usp_DownloadCourses
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	truncate table Courses

    insert into Courses
	select distinct ssbsect_term_code, ssbsect_crn, ssbsect_subj_code, ssbsect_crse_numb
		, ssbsect_seq_numb, scbcrse_title, scbcrse_dept_code
	from openquery (sis, '
		select ssbsect_term_code, ssbsect_crn, ssbsect_subj_code, ssbsect_crse_numb, ssbsect_seq_numb
			, course.scbcrse_dept_code, course.scbcrse_title
		from ssbsect 
			inner join (
				select scbcrse_subj_code, scbcrse_crse_numb, scbcrse_dept_code, scbcrse_title, scbcrse_eff_term
				from scbcrse
					inner join (
						select scbcrse_subj_code subj, scbcrse_crse_numb crse, max(scbcrse_eff_term) term
						from scbcrse
						group by scbcrse_subj_code, scbcrse_crse_numb
					) maxscb on SCBCRSE_SUBJ_CODE = maxscb.subj and SCBCRSE_CRSE_NUMB = maxscb.crse and SCBCRSE_EFF_TERM = maxscb.term
			) course on ssbsect_subj_code = scbcrse_subj_code and ssbsect_crse_numb = scbcrse_crse_numb
		where ssbsect_term_code in ( select stvterm_code from stvterm
									where (stvterm_start_date < sysdate and stvterm_end_date > sysdate)
									   or (stvterm_start_date < sysdate and stvterm_end_date > sysdate - 14)
								  )
		  and course.scbcrse_dept_code is not null
	')

END
