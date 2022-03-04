CREATE TABLE [User] (
	Id					int				NOT NULL IDENTITY(1,1),
	FirstName			nvarchar(50)	NOT NULL,
	LastName			nvarchar(50)	NOT NULL,
	Patronymic 			nvarchar(50)	NOT NULL,
	Email				nvarchar(50)	NOT NULL UNIQUE, 
	Username			nvarchar(50)	NOT NULL,
	Password			nvarchar(200)	NOT NULL,
	RegistrationDate	datetime		NOT NULL,
	ContractNumber		nvarchar(50)	NULL,
	CityId				int				NOT NULL,
	BirthDate			date			NOT NULL,
	GitHubAccount		nvarchar(50),
	Photo				nvarchar(150),
	PhoneNumber			nvarchar(12)	NOT NULL,
	IsDeleted			bit				NOT NULL DEFAULT '0',
	ExileDate			date,
  CONSTRAINT [PK_USER] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
  )
  
GO
ALTER TABLE [User] WITH CHECK ADD CONSTRAINT [User_fk0] FOREIGN KEY ([CityId]) REFERENCES [City]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [User] CHECK CONSTRAINT [User_fk0]
GO