﻿CREATE PROCEDURE [dbo].[Task_Insert]
	@Name nvarchar(255),
	@StartDate datetime,
	@EndDate datetime,
	@Description nvarchar(500),
	@Links nvarchar(500),
	@IsRequired bit
AS
	INSERT INTO Task ([Name], [StartDate], [EndDate], [Description], [Links], [IsRequired])
	VALUES (@Name, @StartDate, @EndDate, @Description, @Links, @IsRequired)
	SELECT @@IDENTITY