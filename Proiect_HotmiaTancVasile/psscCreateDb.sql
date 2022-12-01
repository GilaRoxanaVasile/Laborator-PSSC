 
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](7) NOT NULL,
	[Stoc] [int] NOT NULL,
	[Pret] [decimal] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Client](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](20) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[PhoneNumber] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO	


CREATE TABLE [dbo].[OrderHeader](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[TotalPrice] [decimal] NOT NULL,
	[PaymentOption] [varchar](20) NOT NULL,
	[CardDetails] [varchar](50), 
 CONSTRAINT [PK_OrderHeader] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderLine](
	[OrderLineId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL, // schimb cu product code sau il adaug si pe asta
	[Code] [varchar](7) NOT NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED 
(
	[OrderLineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Bill](
	[BillId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[BillingAddress] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Bill] PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO	


CREATE TABLE [dbo].[Shipping](
	[ShippingId] [int] IDENTITY(1,1) NOT NULL,
	[BillId] [int] NOT NULL,
	[PackageState] [varchar](30) NOT NULL,
	[ZipCode] [varchar](30) NOT NULL,
	[ShippingAddress] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Shipping] PRIMARY KEY CLUSTERED 
(
	[ShippingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO	

ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD CONSTRAINT [FK_Order_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO

ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD CONSTRAINT [FK_Order_Client] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([ClientId])
GO

ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD CONSTRAINT [FK_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrderHeader] ([OrderId])
GO

ALTER TABLE [dbo].[Bill]  WITH CHECK ADD CONSTRAINT [FK_Order_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([ClientId])
GO

ALTER TABLE [dbo].[Shipping]  WITH CHECK ADD CONSTRAINT [FK_Order_ShippingId] FOREIGN KEY([BillId])
REFERENCES [dbo].[Bill] ([BillId])
GO


ALTER TABLE [dbo].[OrderLine] CHECK CONSTRAINT [FK_Order_Product]
GO

ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_Order_Client]
GO

ALTER TABLE [dbo].[OrderLine] CHECK CONSTRAINT [FK_Order_OrderId]
GO

ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Order_ClientId]
GO

ALTER TABLE [dbo].[Shipping] CHECK CONSTRAINT [FK_Order_ShippingId]
GO