﻿-- =============================================
-- Author:		Alan Lai
-- Create date: 
-- Description:	Download all courses for the active term, or recently closed term.  
-- If the term has opened in the last 1 days, don't yet download course information (sysdate - 1).
-- Usage:
/*

USE StudentService
	GO

	EXEC [dbo].[usp_DownloadCourses] @IsDebug = 1
	GO

*/
-- Modifications:
--	2019-05-14 by kjt: Converted to dynamic SQL so that universal term code portion of where clause
--	  could be pulled from udf_TermCodeFilterString and used by all stored procedures requiring it.
--
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadCourses]
	( @IsDebug bit = 0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TSQL varchar(MAX) = ''

	SELECT @TSQL = '
	truncate table Courses

    insert into Courses
	select distinct ssbsect_term_code, ssbsect_crn, ssbsect_subj_code, ssbsect_crse_numb
		, ssbsect_seq_numb, scbcrse_title, scbcrse_dept_code, students_needing_grades, ssbsect_gradable_ind
	from openquery (sis, ''
		select ssbsect_term_code
			, ssbsect_crn
			, ssbsect_subj_code
			, ssbsect_crse_numb
			, ssbsect_seq_numb
			, course.scbcrse_dept_code
			, course.scbcrse_title
			, (
				select count(*)
  				from sfrstcr
					where sfrstcr_crn = ssbsect_crn 
						and sfrstcr_term_code = ssbsect_term_code
						and sfrstcr_rsts_code =''''RE''''
						and sfrstcr_grde_code is null
						and ssbsect_gradable_ind = ''''Y''''
			) students_needing_grades
			, ssbsect_gradable_ind
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
		where ssbsect_term_code in (' 

	SELECT @TSQL += (SELECT [dbo].[udf_TermCodeFilterString]())

	SELECT @TSQL += '			 )                   
		  and course.scbcrse_dept_code is not null
	'')
'
	IF @IsDebug = 1
		PRINT @TSQL
	ELSE
		EXEC (@TSQL)

END
