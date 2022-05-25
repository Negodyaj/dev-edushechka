CREATE PROCEDURE [dbo].[Task_SelectByGroupId]
	@GroupId int
AS
BEGIN
	SELECT 
		t.Id,
		t.Name,
		t.Description,
		t.Links,
		t.IsRequired,
		t.GroupId
	FROM dbo.Task T WITH (NOLOCK)
	WHERE (t.GroupId = @GroupId AND T.IsDeleted=0)
END
