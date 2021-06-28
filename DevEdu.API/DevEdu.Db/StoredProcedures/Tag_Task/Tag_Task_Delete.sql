CREATE PROCEDURE [dbo].[Tag_Task_Delete]
	@TagId int ,
	@TaskId int
AS
	Delete from [dbo].[Tag_Task] 
	Where TagId = @TagId and TaskId = @TaskId
