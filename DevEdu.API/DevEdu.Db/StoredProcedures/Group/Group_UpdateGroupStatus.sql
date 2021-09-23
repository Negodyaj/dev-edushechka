CREATE PROCEDURE dbo.Group_UpdateGroupStatus
	@GroupId int,
	@StatusId int
AS
BEGIN
	UPDATE dbo.[Group]
	SET [GroupStatusId] = @StatusId
	OUTPUT INSERTED.[id],
		   INSERTED.[GroupStatusId] as GroupStatus
	WHERE Id = @GroupId
END   