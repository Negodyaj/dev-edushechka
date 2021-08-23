Create Procedure [dbo].[Course_Topic_AddMultiple]
	@tblCourseTopic Course_TopicType readonly
As
Begin
	set nocount on;
	
	insert into Course_Topic(CourseId,TopicId,Position)
	output inserted.Id
	Select CourseId,TopicId,Position From @tblCourseTopic
End