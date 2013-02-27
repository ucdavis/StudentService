-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[ConvertToTime]
(
	-- Add the parameters for the function here
	@time varchar(4)
)
RETURNS time(0)
AS
BEGIN
	
	declare @tmp varchar(5)

	set @tmp = substring(@time, 1, 2) + ':' + substring(@time, 3, 2)

	return @tmp
		
END
