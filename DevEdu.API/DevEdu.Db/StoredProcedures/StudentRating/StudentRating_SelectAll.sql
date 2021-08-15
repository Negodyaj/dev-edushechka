CREATE PROCEDURE dbo.StudentRating_SelectAll
	AS
BEGIN
	SELECT 
	sr.Id,
	sr.Rating,
	sr.ReportingPeriodNumber,
	g.Id,
	g.Name,
	g.StartDate,
	rt.Id,
	rt.Name,
	rt.Weight,
	u.Id,
	u.FirstName,
	u.LastName,
	u.Email,
	u.Photo,
	ug.RoleId as Id
	from dbo.StudentRating sr
	left join [dbo].[Group] g on sr.GroupId = g.Id
	left join dbo.RatingType rt on sr.RatingTypeId = rt.Id
	left join [dbo].[User] u on sr.UserId = u.Id
	left join dbo.User_Group ug on ug.UserId = u.Id and ug.GroupId = g.Id
	WHERE u.IsDeleted = 0
	AND g.IsDeleted = 0
END