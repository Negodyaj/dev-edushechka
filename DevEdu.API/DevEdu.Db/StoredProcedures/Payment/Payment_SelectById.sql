CREATE PROCEDURE [dbo].[Payment_SelectById]
	@Id		int
AS
BEGIN
	SELECT
		p.Id, 
		p.Date, 
		p.Sum, 
		p.UserId, 
		p.IsPaid,
		p.IsDeleted,
		u.Id,
		u.FirstName,
		u.LastName,
		u.GitHubAccount,
		u.Photo
	FROM dbo.Payment p
		inner join [User] u on u.Id=p.UserId
	WHERE p.Id=@Id
END