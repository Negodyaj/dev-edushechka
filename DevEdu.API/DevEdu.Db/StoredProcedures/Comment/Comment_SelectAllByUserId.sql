CREATE PROCEDURE dbo.Comment_SelectAllByUserId
	@UserId int
AS
BEGIN
	SELECT 
		c.Id,
		c.UserId,
		c.Text,
		c.Date,
		c.IsDeleted,
		u.Id,
		u.FirstName,
		u.LastName,
		u.Patronymic,
		u.Email,
		u.Username,
		u.Password,
		u.IsDeleted,
		u.RegistrationDate,
		u.ContractNumber,
		u.BirthDate,
		u.GitHubAccount,
		u.Photo,
		u.PhoneNumber,
		u.ExileDate,
		u.CityId as id,
		ur.RoleId as id
	FROM dbo.Comment c
		inner join [User] u on u.Id=c.UserId
		inner join User_Role ur on ur.UserId=u.Id
	WHERE (c.UserId = @UserId AND c.IsDeleted=0)
END