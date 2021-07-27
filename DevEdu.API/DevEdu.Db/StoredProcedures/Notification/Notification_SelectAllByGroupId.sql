CREATE PROCEDURE [dbo].[Notification_SelectAllByGroupId]
	@GroupId		int
AS
BEGIN
	SELECT
	n.Id, 
	n.Text,
	n.Date,
	n.IsDeleted,
	g.Id,
	g.Name,
	g.StartDate
	FROM dbo.Notification n
		left join [Group] g on g.Id = n.GroupId
	WHERE (n.GroupId = @GroupId AND n.IsDeleted=0)
END