CREATE PROCEDURE [dbo].[Lesson_Update]
    @Id int,
    @AdditionalMaterials nvarchar(255),
    @LinkToRecord nvarchar(150),
    @Date datetime,
    @Name nvarchar(150)
AS
BEGIN
    UPDATE dbo.Lesson
    SET AdditionalMaterials = @AdditionalMaterials,
        LinkToRecord        = @LinkToRecord,
        Date                = @Date,
        Name                = @Name
    WHERE Id = @Id
END