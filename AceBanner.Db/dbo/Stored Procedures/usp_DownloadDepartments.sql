-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE usp_DownloadDepartments
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	truncate table Departments

    insert into Departments
	select stvdept_code, stvdept_desc from openquery(sis, 'select stvdept_code, stvdept_desc from stvdept')

END
