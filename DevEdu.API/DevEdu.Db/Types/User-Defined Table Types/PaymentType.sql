Create Type [dbo].[PaymentType] as Table(
	[Date] [datetime]  not null,
	[Sum] decimal(6, 2) not null,
	UserId [int] not null,
	IsPaid [bit] not null
)