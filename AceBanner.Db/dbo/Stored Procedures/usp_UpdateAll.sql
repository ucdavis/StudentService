CREATE PROCEDURE [dbo].[usp_UpdateAll]
AS

exec usp_DownloadDepartments
exec usp_DownloadCourses
exec usp_DownloadSections
exec usp_DownloadCourseRoster
exec usp_DownloadStudents
exec usp_DownloadInstructors
exec usp_DownloadTerms

RETURN 0
