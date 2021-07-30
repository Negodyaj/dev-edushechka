CREATE PROCEDURE [dbo].[Course_Topic_SelectById]
	@Id int
AS
Begin
	Select 		
		ct.Id,
		ct.CourseId,
		t.Id,
		t.Name,
		ct.Position,
		t.Duration
	From dbo.Course_Topic ct
		inner join dbo.Topic t on t.Id = ct.TopicId 
	Where (ct.Id = @Id)
End
