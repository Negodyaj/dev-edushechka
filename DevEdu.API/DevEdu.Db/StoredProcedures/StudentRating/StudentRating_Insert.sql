CREATE PROCEDURE dbo.StudentRating_Insert
	@UserId int,
	@GroupId int,
	@RatingTypeId int,
	@Rating int,
	@ReportingPeriodNumber int
AS
BEGIN
	INSERT INTO dbo.StudentRating (UserId, GroupId, RatingTypeId, Rating, ReportingPeriodNumber)
	VALUES (@UserId, @GroupId, @RatingTypeId, @Rating, @ReportingPeriodNumber)
	SELECT @@IDENTITY
END
