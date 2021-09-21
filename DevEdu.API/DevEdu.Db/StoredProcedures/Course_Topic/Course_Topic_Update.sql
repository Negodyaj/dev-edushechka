Create Procedure [dbo].[Course_Topic_Update]
  @tblCourseTopic Course_TopicType readonly 
As
Begin
  Update dbo.Course_Topic
  Set 
	TopicId = src.TopicId,
	Position = src.Position

  From 
	@tblCourseTopic src
  Where 
	dbo.Course_Topic.CourseId = src.CourseId
	and
	dbo.Course_Topic.TopicId = src.TopicId
End