CREATE PROCEDURE dbo.StudentRaiting_SelectByGroupId
		@GroupId int
	AS
BEGIN
	SELECT 
	sr.Id,
	sr.Raiting,
	sr.ReportingPeriodNumber,
	g.Id,
	g.CourseId,
	g.GroupStatusId,
	g.PaymentPerMonth,
	g.StartDate,
	g.Timetable,
	rt.Id,
	rt.Name,
	rt.Weight,
	u.Id,
	u.FirstName,
	u.LastName,
	u.Patronymic,
	u.Email,
	u.Username,
	u.RegistrationDate,
	u.ContractNumber,
	u.BirthDate,
	u.GitHubAccount,
	u.Photo,
	u.PhoneNumber,
	u.ExileDate,
	u.CityId,
	ug.RoleId as Id
	from dbo.StudentRaiting sr
	left join [dbo].[Group] g on sr.GroupId = g.Id
	left join dbo.RaitingType rt on sr.RaitingTypeId = rt.Id
	left join [dbo].[User] u on sr.UserId = u.Id
	left join dbo.User_Group ug on ug.UserId = u.Id and ug.GroupId = g.Id
	WHERE g.Id = @GroupId
	AND u.IsDeleted = 0
END