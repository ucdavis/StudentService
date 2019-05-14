-- =============================================
-- Author:		Ken Taylor
-- Create date: May 13, 2019
-- Description:	Build the Term Code portion of the where clause
--	to be used with the various student service table loading stored 
--	procedures.
--
-- Usage:
/*

	SELECT [dbo].[udf_TermCodeFilterString]()

*/
--
-- Modifications:
--
-- =============================================
CREATE FUNCTION [dbo].[udf_TermCodeFilterString] 
( )
RETURNS varchar(500)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(500)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = N'
			 SELECT stvterm_code 
			 FROM stvterm
			 WHERE 
				(sysdate between stvterm_start_date AND stvterm_end_date) OR -- Current terms
				(stvterm.STVTERM_END_DATE between sysdate - 21 AND sysdate ) OR -- terms that closed up to 3 weeks ago
				(stvterm.STVTERM_START_DATE between sysdate AND sysdate + 21) -- terms that will open up to 3 weeks from now.
'
	RETURN @Result

END



