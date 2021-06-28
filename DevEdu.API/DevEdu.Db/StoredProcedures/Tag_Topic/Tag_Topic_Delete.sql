CREATE PROCEDURE [dbo].[Tag_Topic_Delete]
	@TagId int,
	@TopicId int
AS
	DELETE FROM [dbo].[Tag_Topic]
	WHERE TagId = @TagId AND TopicId = @TopicId