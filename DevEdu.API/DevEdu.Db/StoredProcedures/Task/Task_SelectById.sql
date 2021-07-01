CREATE PROCEDURE dbo.Task_SelectById
	@Id int
AS
BEGIN
	SELECT Id, Name, StartDate, EndDate, Description, Links, IsRequired from dbo.Task
	WHERE 
	Id = @Id
END