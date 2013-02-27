CREATE PROCEDURE [dbo].[usp_DownloadTerms]

AS

	merge Terms as t
	using 
		(select stvterm_code, stvterm_desc, stvterm_start_date, stvterm_end_date from openquery(sis, '
			select stvterm_code, stvterm_desc, stvterm_start_date, stvterm_end_date
			from stvterm
			where stvterm_start_date > sysdate
			  and stvterm_trmt_code in (''Q'', ''W'')
		')) s
	on t.id = s.stvterm_code
	when matched then 
		update set t.name = s.stvterm_desc, t.start = s.stvterm_start_date, t.[end] = s.stvterm_end_date
	when not matched by target then 
		insert (id, name, start, [end]) values (s.stvterm_code, s.stvterm_desc, s.stvterm_start_date, s.stvterm_end_date);

RETURN 0
