﻿CREATE PROCEDURE dbo.Group_SelectById
	@Id int
AS
BEGIN
	SELECT 
		G.Id,
		G.Name,
		G.GroupStatusId GroupStatus,
		G.StartDate,
		G.EndDate,
		G.Timetable,
		G.PaymentPerMonth,
		C.Id,
		C.Name,
		C.Description
	FROM dbo.[Group] G
		inner join [Course] C on C.[Id] = G.[CourseId]
	WHERE (G.[Id] = @Id)
END