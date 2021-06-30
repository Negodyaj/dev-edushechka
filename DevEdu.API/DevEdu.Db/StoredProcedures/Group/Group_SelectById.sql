CREATE PROCEDURE dbo.Group_SelectById
	@Id int
AS
BEGIN
	SELECT 
	Id, CourseId, GroupStatusId, StartDate, Timetable, PaymentPerMonth
	FROM dbo.[Group]
	WHERE (Id = @Id)
END