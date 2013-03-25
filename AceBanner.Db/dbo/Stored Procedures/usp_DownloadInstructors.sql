CREATE PROCEDURE [dbo].[usp_DownloadInstructors]
AS

truncate table Instructors
truncate table CourseInstructors

declare @tmp table (
	termcode int,
	crn int,
	firstname varchar(50),
	lastname varchar(50),
	instructorid int,
	loginid varchar(20),
	email varchar(50),
	primaryind bit
)

insert into @tmp
select * from openquery (sis, '
	select zsvinst_term_code, zsvinst_crn
		, spriden_first_name, spriden_last_name, spriden_pidm
		, lower(wormoth_login_id) loginid, emails.address
		, (case when zsvinst_primary_ind = ''Y'' then 1 else 0 end) as primaryind
	from zsvinst
		inner join spriden on zsvinst_id = spriden_id
		left outer join wormoth on wormoth_pidm = spriden_pidm
		left outer join (
			select goremal_pidm as pidm, goremal_email_address as address
			from goremal
			where goremal_status_ind = ''A''
				and goremal_emal_code = ''UCD''
		) Emails on Emails.pidm = spriden_pidm
	where zsvinst_term_code = (
		select min(stvterm_code) from stvterm
		where stvterm_end_date > sysdate
			and stvterm_trmt_code in (''Q'', ''W'') 
	  )
	  and zsvinst_id is not null
	  and wormoth_acct_type = ''Z''
	  and wormoth_acct_status = ''A''
	  and spriden_change_ind is null
	  and WORMOTH_ACTIVITY_DATE = ( select max(wormoth_activity_date ) from wormoth iw where iw.WORMOTH_PIDM = wormoth.wormoth_pidm and iw.wormoth_acct_type = ''Z'' and iw.wormoth_acct_status = ''A'' )
')

insert into Instructors
select distinct instructorid, firstname, lastname, loginid, email from @tmp

insert into CourseInstructors
select newid(), termcode, crn, instructorid, primaryind
from @tmp

RETURN 0
