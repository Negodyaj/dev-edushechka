CREATE PROCEDURE dbo.Group_UpdateById
    @Id int,
    @Name nvarchar(50),
    @CourseId int,
	@GroupStatusId int,
    @StartDate date,
    @Timetable nvarchar(500),
    @PaymentPerMonth decimal(6,2) 
AS
BEGIN
    UPDATE dbo.[Group]
    SET
        [Name] = @Name,
        [CourseId] = @CourseId,
        [GroupStatusId] = @GroupStatusId,
        [StartDate] = @StartDate,
        [Timetable] = @Timetable,
        [PaymentPerMonth] = @PaymentPerMonth
    OUTPUT INSERTED.[id],
           INSERTED.[Name],
           INSERTED.[CourseId],
           INSERTED.[GroupStatusId],
           INSERTED.[StartDate],
           INSERTED.[Timetable],
           INSERTED.[PaymentPerMonth]
    WHERE Id = @Id
END