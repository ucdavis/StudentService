CREATE VIEW [dbo].[vCourses]
	AS 

select termcode, crn, [subject], coursenumb, sequence, name
	,  isnull((
		select top 1 do.departmentid 
		from departmentoverrides do 
		where (do.[subject] = courses.[subject] and do.coursenumb = courses.coursenumb)
		   or (do.[subject] = courses.[subject] and do.coursenumb is null)
		order by case when do.coursenumb is null then 1 else 0 end
		), courses.departmentid) departmentid
from courses
