CREATE PROCEDURE dbo.Course_SelectById
	@Id int
AS
BEGIN
	SELECT 
	  C.Id,
      C.Name,
      C.Description,
      C.IsDeleted,
	  T.Id,
	  T.Name,
	  T.Duration

  FROM [dbo].[Course] C WITH (NOLOCK)
  LEFT JOIN Course_Topic CT (NOLOCK) ON CT.CourseId = C.Id
  LEFT JOIN [Topic] T WITH (NOLOCK) ON CT.TopicId = T.Id AND T.IsDeleted = 0 
	WHERE (C.Id = @Id)
END