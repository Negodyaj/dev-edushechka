CREATE TABLE [Material]
(
    Id        int           NOT NULL IDENTITY (1,1),
    CourseId  int           NULL,
    Content   nvarchar(max) NOT NULL,
    Link      nvarchar(200),
    IsDeleted bit           NOT NULL DEFAULT '0',
    CONSTRAINT [PK_MATERIAL] PRIMARY KEY CLUSTERED
        ([Id] ASC) WITH (IGNORE_DUP_KEY = OFF)
)
go

ALTER TABLE [Material]
    WITH CHECK ADD CONSTRAINT [Material_fk0] FOREIGN KEY ([CourseId]) REFERENCES [Course] ([Id]) ON UPDATE NO ACTION
GO
ALTER TABLE [Material]
    CHECK CONSTRAINT [Material_fk0]
GO