CREATE PROCEDURE [dbo].[User_Group_Delete]
	@ID int
AS
	DELETE [dbo].[User_Group]
WHERE Id = @ID
