CREATE PROCEDURE [dbo].[Group_SelectByCourseId]
	@CourseId int
AS
	SELECT Id,
		   Name,
		   PaymentPerMonth,
		   Timetable
	FROM [Group] G
	WHERE CourseId = @CourseId AND IsDeleted = 0
RETURN 0
