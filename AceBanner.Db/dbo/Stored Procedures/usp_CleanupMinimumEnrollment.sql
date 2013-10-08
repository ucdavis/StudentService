CREATE PROCEDURE [dbo].[usp_CleanupMinimumEnrollment]

AS

	declare @crns table (termcode int, crn int)

	insert into @crns (termcode, crn)
	select termcode,crn from courseroster group by termcode, crn having count(loginid) < 1

	insert into @crns (termcode, crn)
	select c.TermCode, c.Crn from Courses c left outer join CourseRoster cr on c.TermCode = cr.Termcode and c.Crn = cr.Crn where cr.LoginId is null

	delete ci from CourseInstructors ci
		inner join @crns crns on ci.TermCode = crns.termcode and ci.crn = crns.crn

	delete cr from CourseRoster cr
		inner join @crns crns on cr.Termcode = crns.termcode and cr.crn = crns.crn

	delete s from sections s
		inner join @crns crns on s.Termcode = crns.termcode and s.crn = crns.crn

	delete c from courses c
	inner join @crns crns on c.Termcode = crns.termcode and c.crn = crns.crn


RETURN 0
