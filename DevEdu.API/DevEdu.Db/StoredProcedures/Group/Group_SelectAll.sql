CREATE PROCEDURE dbo.Group_SelectAll
	@Role int
AS
BEGIN
	SELECT 
		G.Id,
		G.Name,
		G.GroupStatusId,
		G.StartDate,
		G.Timetable,
		G.PaymentPerMonth,
		C.Id,
		C.Name,
		C.[Description]		
	FROM dbo.[Group] G
		inner join [Course] C on C.Id = G.CourseId	  		
	WHERE (G.Id = G.Id)
END