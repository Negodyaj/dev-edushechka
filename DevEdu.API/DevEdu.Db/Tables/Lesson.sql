﻿CREATE TABLE [Lesson]
(
    Id                  int      NOT NULL IDENTITY (1,1),
    Date                datetime NOT NULL,
    AdditionalMaterials nvarchar(500),
    TeacherId           int      NOT NULL,
    LinkToRecord        nvarchar(150),
    IsDeleted           bit      NOT NULL DEFAULT '0',
    CONSTRAINT [PK_LESSON] PRIMARY KEY CLUSTERED
        (
         [Id] ASC
            ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Lesson] WITH CHECK ADD CONSTRAINT [Lesson_fk0] FOREIGN KEY ([TeacherId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Lesson] CHECK CONSTRAINT [Lesson_fk0]
GO