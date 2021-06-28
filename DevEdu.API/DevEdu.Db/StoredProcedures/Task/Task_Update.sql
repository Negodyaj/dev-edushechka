CREATE PROCEDURE [dbo].[Task_Update]
	@ID int,
	@Name nvarchar(255),
	@StartDate datetime,
	@EndDate datetime,
	@Description nvarchar(500),
	@Links nvarchar(500),
	@IsRequired bit
AS
	UPDATE [Task] 
	SET 
	[Name] = @Name,
	[StartDate] = @StartDate,
	[EndDate] = @EndDate,
	[Description] = @Description,
	[Links] = @Links,
	[IsRequired] = @IsRequired
	Where Id = @ID