CREATE PROCEDURE dbo.Group_UpdateGroupStatus
	@GroupId int,
	@StatusId int
AS
BEGIN
	UPDATE dbo.[Group]
	SET [GroupStatusId] = @StatusId
	WHERE Id = @GroupId
END