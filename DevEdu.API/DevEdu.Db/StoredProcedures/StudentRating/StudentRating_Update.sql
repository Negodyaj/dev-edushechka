CREATE PROCEDURE dbo.StudentRating_Update
	@Id int,
	@Rating int,
	@ReportingPeriodNumber int
AS
BEGIN
	UPDATE dbo.StudentRating 
	SET 
	Rating = @Rating,
	ReportingPeriodNumber = @ReportingPeriodNumber
	Where Id = @Id
END