CREATE PROCEDURE [dbo].[Tag_Topic_Delete]
	@TagId int,
	@TopicId int
AS
	DELETE FROM [dbo].[Tag_Topic]
	OUTPUT DELETED.Id
	WHERE TagId = @TagId AND TopicId = @TopicId