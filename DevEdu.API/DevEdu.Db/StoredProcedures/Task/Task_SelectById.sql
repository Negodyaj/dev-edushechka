CREATE PROCEDURE dbo.Task_SelectById
	@Id int
AS
	SELECT Name, StartDate, EndDate, Description, Links, IsRequired from dbo.Task
	WHERE 
	Id = @Id