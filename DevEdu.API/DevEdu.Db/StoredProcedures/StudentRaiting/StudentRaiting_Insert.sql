CREATE PROCEDURE dbo.StudentRaiting_Insert
	@UserID int,
	@GroupId int,
	@RaitingTypeID int,
	@Raiting int,
	@ReportingPeriodNumber int
AS
BEGIN
	INSERT INTO dbo.StudentRaiting (UserID, GroupId, RaitingTypeID, Raiting, ReportingPeriodNumber)
	VALUES (@UserID, @GroupId, @RaitingTypeID, @Raiting, @ReportingPeriodNumber)
	SELECT @@IDENTITY
END
