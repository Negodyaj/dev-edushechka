CREATE PROCEDURE [dbo].[Tag_Task_Delete]
	@TagID int ,
	@TaskID int
AS
	Delete from [dbo].[Tag_Task] 
	Where TagId = @TagID and TaskID = @TaskID
