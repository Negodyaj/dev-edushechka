CREATE PROCEDURE dbo.StudentRating_Insert
	@UserID int,
	@GroupId int,
	@RatingTypeID int,
	@Rating int,
	@ReportingPeriodNumber int
AS
BEGIN
	INSERT INTO dbo.StudentRating (UserID, GroupId, RatingTypeID, Rating, ReportingPeriodNumber)
	VALUES (@UserID, @GroupId, @RatingTypeID, @Rating, @ReportingPeriodNumber)
	SELECT @@IDENTITY
END
