CREATE PROCEDURE dbo.StudentRaiting_Insert
	@UserId int,
	@GroupId int,
	@RaitingTypeId int,
	@Raiting int,
	@ReportingPeriodNumber int
AS
BEGIN
	INSERT INTO dbo.StudentRaiting (UserId, GroupId, RaitingTypeId, Raiting, ReportingPeriodNumber)
	VALUES (@UserId, @GroupId, @RaitingTypeId, @Raiting, @ReportingPeriodNumber)
	SELECT @@IDENTITY
END