CREATE PROCEDURE [dbo].[Material_Insert]
@Content nvarchar(max)
AS
INSERT INTO [Material] (Content)
VALUES (@Content)
SELECT @@IDENTITY
