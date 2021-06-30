CREATE PROCEDURE dbo.Group_SelectAll
AS
BEGIN
	SELECT 
	Id, CourseId, GroupStatusId, StartDate, Timetable, PaymentPerMonth
	FROM dbo.[Group]
	WHERE (IsDeleted=0)
END