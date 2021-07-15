CREATE PROCEDURE dbo.StudentRaiting_Update
	@Id int,
	@Raiting int,
	@ReportingPeriodNumber int
AS
BEGIN
	UPDATE dbo.StudentRaiting 
	SET 
	Raiting = @Raiting,
	ReportingPeriodNumber = @ReportingPeriodNumber
	Where Id = @Id
END