CREATE PROCEDURE [dbo].[usp_DownloadUnex]

AS
	
	declare @students table (
		mothraid varchar(10),
		pidm int,
		loginid varchar(20),
		firstname varchar(50),
		lastname varchar(50),
		email varchar(50),
		term int,
		crn int
	)

	insert into @students 
	select * from openquery (unex,'
		select mothraid, pidm, loginid, firstname, lastname, email, term, crn
		from myweb_user.ext_course_evals
	')

	insert into students (pidm, firstname, lastname, loginid, email, [type]) 
	select distinct pidm, firstname, lastname, loginid, email, 'U' from @students

	insert into CourseRoster (LoginId, crn, termcode)
	select distinct loginid, crn, term from @students

	--merge students as t
	--using (select distinct pidm, firstname, lastname, loginid, email from @students) as s
	--on t.pidm = s.pidm
	--when not matched then
	--	insert (pidm, firstname, lastname, loginid, email) values (s.pidm, s.firstname, s.lastname, s.loginid, s.email);

	--merge courseroster as t
	--using (select distinct pidm, crn , term from @students) as s
	--on (t.pidm = s.pidm and t.crn = s.crn and t.termcode = t.term)
	--when not matched then 
	--	insert (pidm, crn termcode) values (s.pidm, s.crn, s.term);

RETURN 0
