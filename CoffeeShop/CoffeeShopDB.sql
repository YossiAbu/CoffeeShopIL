Create Database dbOnlineStore
use dbOnlineStore

CREATE TABLE [dbo].[Users] (
    [UserId]   INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [Email]    VARCHAR (50) NULL,
    [Password] VARCHAR (50) NULL,
    [RoleType] INT          NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);




CREATE TABLE [dbo].[Categories] (
    [CatId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([CatId] ASC)
);




CREATE TABLE [dbo].[Products] (
    [ProID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)   NULL,
    [Description] VARCHAR (50)   NULL,
    [Unit]        INT            NULL,
    [Image]       VARCHAR (1000) NULL,
    [CatId]       INT            NULL,
    PRIMARY KEY CLUSTERED ([ProID] ASC),
    CONSTRAINT [FK_tblProducts_tblCategories] FOREIGN KEY ([CatId]) REFERENCES [dbo].[Categories] ([CatId])
);



CREATE TABLE [dbo].[Invoices] (
    [InvoiceId]   INT          IDENTITY (1, 1) NOT NULL,
    [UserId]      INT          NULL,
    [Bill]        INT          NULL,
    [Payment]     VARCHR (50) NULL,
    [InvoiceDate] DATE         NULL,
	[Status]	  TINYINT	   NULL,
    PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);


CREATE TABLE [dbo].[Orders] (
    [OrderId]   INT           IDENTITY (1, 1) NOT NULL,
    [ProID]     INT           NULL,
    [Contact]   VARCHAR (50)  NULL,
    [Unit]      INT           NULL,
    [Qty]       INT           NULL,
    [Total]     INT           NULL,
    [OrderDate] DATE          NULL,
    [InvoiceId] INT           NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC),
    FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoices] ([InvoiceId])
);





CREATE TABLE dbo.Tables
(
	tableId	int	identity(1,1) not null,
	numSeats int null,
	area	varchar(50) null,
	primary key(tableId)
);



CREATE TABLE dbo.Sits
(
	sitId	int identity(1,1) not null,
	available tinyint	not null,
	tableId int not null,
	userId	int	null,
	primary key(sitId),
	FOREIGN key(tableId) REFERENCES dbo.Tables(tableId),
	foreign key(userId) references dbo.Users(UserId)
);


create view userInvoices as
select Invoices.InvoiceId,  Users.Name as 'Customer', 
 Invoices.Bill,Invoices.Payment, Invoices.InvoiceDate
 from Invoices
inner join Users on Users.UserId = Invoices.UserId
where Invoices.UserId = Users.UserId

/* get all product for admin  */
create view viewallproduct as
select Products.ProID, Products.Name, Categories.Name as 'Category', Products.Description, Products.Unit, Products.Image
from Products
inner join Categories on Categories.CatId = Products.CatId

/* overall order's */
create view getOrders as 
select Invoices.InvoiceId,Users.UserId, Users.Name as 'user', Invoices.Bill,Invoices.Payment
, Invoices.InvoiceDate,Invoices.Status  from Invoices
inner join Users on Users.UserId = Invoices.UserId

/* user's order  */
create view getallorderuser as
select Invoices.InvoiceId,Users.UserId, Users.Name as 'user', Invoices.Bill,Invoices.Payment
, Invoices.InvoiceDate,Invoices.Status  from Invoices
inner join Users on Users.UserId = Invoices.UserId
where Invoices.Status = 1

/* get table sits */
create view getTableSits AS
select Sits.SitId,Sits.tableId,Sits.available
from Sits
inner join Tables on Tables.tableId = Sits.tableId


