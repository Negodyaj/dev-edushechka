CREATE PROCEDURE dbo.Group_UpdateById
	@Id int,
	@Name nvarchar(50),
	@CourseId int,
	@GroupStatusId int,
	@StartDate date,
	@EndDate date,
	@Timetable nvarchar(500),
	@PaymentPerMonth decimal(6,2),
	@PaymentsCount int
AS
BEGIN
	UPDATE dbo.[Group]
	SET
		Name = @Name,
		CourseId = @CourseId,
		GroupStatusId = @GroupStatusId,
		StartDate = @StartDate,
		EndDate=@EndDate,
		Timetable = @Timetable,
		PaymentPerMonth = @PaymentPerMonth,
		PaymentsCount = @PaymentsCount
	OUTPUT INSERTED.id,
		   INSERTED.Name,
		   INSERTED.CourseId,
		   INSERTED.GroupStatusId,
		   INSERTED.StartDate,
		   INSERTED.Timetable,
		   INSERTED.PaymentPerMonth,
		   inserted.PaymentsCount
	WHERE Id = @Id
END