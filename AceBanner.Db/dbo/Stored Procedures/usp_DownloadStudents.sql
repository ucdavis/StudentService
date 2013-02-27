﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE usp_DownloadStudents
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    truncate table Students

	insert into Students
	select *
	from openquery (sis, '
		select spriden_pidm, spriden_id, spriden_first_name, spriden_last_name
			, lower(wormoth_login_id) loginid, emails.address
		from spriden
			inner join wormoth on spriden_pidm = wormoth_pidm
			left outer join (
				select goremal_pidm as pidm, goremal_email_address as address
				from goremal
				where goremal_status_ind = ''A''
					and goremal_emal_code = ''UCD''
			) Emails on Emails.pidm = spriden_pidm
		where spriden_change_ind is null
		  and spriden_pidm in (
				select sfrstcr_pidm
				from sfrstcr
				where sfrstcr_term_code = ( select min(stvterm_code) from stvterm
											where stvterm_end_date > sysdate
											  and stvterm_trmt_code in (''Q'', ''W'') 
										  )
				  and sfrstcr_rsts_code in (''RE'', ''RW'')
		  )
		  and wormoth_acct_type = ''Z''
		  and wormoth_acct_status = ''A''
		  and WORMOTH_ACTIVITY_DATE = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''Z'' and iw.wormoth_acct_status = ''A'' )
	')

END