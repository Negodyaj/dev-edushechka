CREATE TABLE [Material]
(
    Id        int           NOT NULL IDENTITY (1,1),
    Content   nvarchar(max) NOT NULL,
    Link      nvarchar(200) NOT NULL,
    IsDeleted bit           NOT NULL DEFAULT '0',
    CONSTRAINT [PK_MATERIAL] PRIMARY KEY CLUSTERED
        ([Id] ASC) WITH (IGNORE_DUP_KEY = OFF)
)