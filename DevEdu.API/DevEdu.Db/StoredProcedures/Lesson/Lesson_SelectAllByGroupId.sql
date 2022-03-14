CREATE PROCEDURE [dbo].[Lesson_SelectAllByGroupId]
	@GroupId int,
    @IsPublished int
AS
BEGIN
    SELECT l.Id,
           l.Date,
           l.AdditionalMaterials,
           l.IsPublished,
           u.Id,
           u.FirstName,
           u.LastName,
           u.Email,
           u.Photo,
           t.Id,
           t.Name
    FROM dbo.Lesson as l
		inner join dbo.[User] u on l.TeacherId = u.Id  

		left join dbo.Lesson_Topic lt on l.Id = lt.LessonId
		left join dbo.Topic t on t.Id = lt.TopicId
		
		inner join dbo.Group_Lesson gl on gl.LessonId = l.Id
    WHERE gl.GroupId = @GroupId and l.IsDeleted = 0 and l.IsPublished = @IsPublished
END
