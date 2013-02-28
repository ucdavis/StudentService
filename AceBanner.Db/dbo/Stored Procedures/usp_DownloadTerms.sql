CREATE PROCEDURE [dbo].[usp_DownloadTerms]

AS

	truncate table terms

	insert into terms 
	select * from openquery(sis, '
		select stvterm_code, stvterm_desc, stvterm_start_date, stvterm_end_date
		from stvterm
		where stvterm_end_date > sysdate
			and stvterm_trmt_code in (''Q'', ''W'')
	')

RETURN 0
