CREATE PROCEDURE dbo.Group_Update
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
    WHERE Id = @Id
END