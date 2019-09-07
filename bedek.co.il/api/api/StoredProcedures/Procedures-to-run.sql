/************************************************************
 * Updated by: Eyal.
 * Time: 09/08/2019 20:33:22
 ************************************************************/
 

USE [Bedek]
GO
/****** Object:  StoredProcedure [dbo].[ServiceInUser_Select_UserId]    Script Date: 05/08/2019 23:52:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/************************************************************
 * Updated by: Eyal.
 * Time: 03/08/2019 13:00:26
 ************************************************************/
 
IF OBJECT_ID('[ServiceInUser_Select_UserId]', 'P') IS NOT NULL
    DROP PROC [ServiceInUser_Select_UserId]
GO


CREATE PROCEDURE [dbo].[ServiceInUser_Select_UserId]
	@UserId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT s.ServiceId,
	       s.ServiceName,
	       s.WarrantyPeriodInMonths,
	       siu.UserId -- null if not in service
	FROM   [Service]                AS s
	       LEFT JOIN ServiceInUser  AS siu
	            ON  siu.ServiceId = s.ServiceId
	            AND siu.UserId = @UserId
	WHERE  s.IsEnable = 1
END
GO
  

-----

IF OBJECT_ID('[ServiceInHandymanInBuilding_Select_BuildingId]', 'P') IS NOT NULL
    DROP PROC ServiceInHandymanInBuilding_Select_BuildingId
GO


GO
CREATE PROCEDURE [dbo].[ServiceInHandymanInBuilding_Select_BuildingId]
	@BuildingId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT s.ServiceId,
	       s.ServiceName,
	       s.WarrantyPeriodInMonths,
	       s.IsEnable,
	       u.UserId,
	       u.FirstName,
	       u.LastName,
	       u.Email,
	       u.[Password],
	       u.Phone1,
	       u.Phone2,
	       u.IdentityCardId,
	       u.IsAcceptEmails,
	       u.DateRegistered,
	       u.Company,
	       uib.BuildingId -- null if not in ServiceInHandymanInBuilding
	       
	FROM   [Service]                 AS s
	       INNER JOIN ServiceInUser  AS siu
	            ON  s.ServiceId = siu.ServiceId
	       INNER JOIN [User]         AS u
	            ON  u.UserId = siu.UserId
	       LEFT JOIN ServiceInHandymanInBuilding AS uib
	            ON  uib.UserId = u.UserId
	            AND uib.ServiceId = s.ServiceId
	            AND uib.BuildingId = @BuildingId	            
	ORDER BY
	       s.ServiceName,u.Company
END
GO

-----

-- Add Identity to ServiceInHandymanInBuilding And Create Unique
ALTER TABLE ServiceInHandymanInBuilding
ADD ServiceInHandymanInBuildingId INT IDENTITY(1,1)
GO
CREATE UNIQUE NONCLUSTERED INDEX IX_BuildingId_ServiceId_UserId
   ON ServiceInHandymanInBuilding (BuildingId, ServiceId, UserId);
   
   
 USE [Bedek]
GO

/****** Object:  Table [dbo].[ServiceCall]    Script Date: 24/08/2019 09:19:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceCall](
	[ServiceCallId] [uniqueidentifier] NOT NULL,
	[ApartmentId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[ServiceCallDocId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ServiceCall] PRIMARY KEY CLUSTERED 
(
	[ServiceCallId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ServiceCall] ADD  CONSTRAINT [DF_ServiceCall_ServiceCallId]  DEFAULT (newid()) FOR [ServiceCallId]
GO

ALTER TABLE [dbo].[ServiceCall] ADD  CONSTRAINT [DF_ServiceCall_Status]  DEFAULT (N'פתוחה') FOR [Status]
GO

ALTER TABLE [dbo].[ServiceCall]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCall_Apartments] FOREIGN KEY([ApartmentId])
REFERENCES [dbo].[Apartments] ([ApartmentId])
GO

ALTER TABLE [dbo].[ServiceCall] CHECK CONSTRAINT [FK_ServiceCall_Apartments]
GO



USE [Bedek]
GO

/****** Object:  Table [dbo].[ServiceCallDoc]    Script Date: 24/08/2019 09:17:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceCallDoc](
	[ServiceCallDocId] [uniqueidentifier] NOT NULL,
	[ServiceCallId] [uniqueidentifier] NOT NULL,
	[DocDescription] [nvarchar](max) NULL,
	[FileName] [nvarchar](200) NOT NULL,
	[FileContentType] [varchar](100) NOT NULL,
	[ServiceInHandymanInBuildingId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ServiceCallDoc]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCallDoc_ServiceCall] FOREIGN KEY([ServiceCallId])
REFERENCES [dbo].[ServiceCall] ([ServiceCallId])
GO

ALTER TABLE [dbo].[ServiceCallDoc] CHECK CONSTRAINT [FK_ServiceCallDoc_ServiceCall]
GO

ALTER TABLE [dbo].[ServiceCallDoc]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCallDoc_ServiceInHandymanInBuilding] FOREIGN KEY([ServiceInHandymanInBuildingId])
REFERENCES [dbo].[ServiceInHandymanInBuilding] ([ServiceInHandymanInBuildingId])
GO

ALTER TABLE [dbo].[ServiceCallDoc] CHECK CONSTRAINT [FK_ServiceCallDoc_ServiceInHandymanInBuilding]
GO

USE [Bedek]
GO
/****** Object:  StoredProcedure [dbo].[ServiceInHandymanInBuilding_Select_BuildingId]    Script Date: 24/08/2019 22:28:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/************************************************************
 * Last changed by Eyal
 * Time: 30/08/2019 15:16:25
 ************************************************************/
 -- [ServiceInHandymanInBuildingInServiceCall_Select_ApartmentId-ServiceCallId] 1, null
alter PROCEDURE [dbo].[ServiceInHandymanInBuildingInServiceCall_Select_ApartmentId-ServiceCallId]
	@ApartmentId INT,
	@ServiceCallId UNIQUEIDENTIFIER  = null
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE	@BuildingId int 
	DECLARE @DateOfEntrance DATETIME
	Select 
		@BuildingId = BuildingId, 
		@DateOfEntrance = DateOfEntrance
	From  Apartments where ApartmentId  = @ApartmentId
	
	SELECT s.ServiceId,
	       s.ServiceName,
	       s.WarrantyPeriodInMonths,
	       s.IsEnable,
	       u.UserId,
	       u.FirstName,
	       u.LastName,
	       u.Email,
	       u.[Password],
	       u.Phone1,
	       u.Phone2,
	       u.IdentityCardId,
	       u.IsAcceptEmails,
	       u.DateRegistered,
	       u.Company,
	       uib.BuildingId,
	       sc.ApartmentId,
		   uib.ServiceInHandymanInBuildingId,
		   @DateOfEntrance as 'DateOfEntrance'
	FROM   [Service]                 AS s
	       INNER JOIN ServiceInUser  AS siu
	            ON  s.ServiceId = siu.ServiceId
	       INNER JOIN [User]         AS u
	            ON  u.UserId = siu.UserId
	       INNER JOIN ServiceInHandymanInBuilding AS uib
	            ON  uib.UserId = u.UserId
	            AND uib.ServiceId = s.ServiceId
	            AND uib.BuildingId = @BuildingId
	       LEFT JOIN ServiceInHandymanInBuildingInServiceCall AS sihibisc
				ON sihibisc.ServiceInHandymanInBuildingId = uib.ServiceInHandymanInBuildingId
		   LEFT	JOIN ServiceCall AS sc
				ON SC.ServiceCallId = sihibisc.ServiceCallId
	ORDER BY
	       s.ServiceName,
	       u.Company
END
GO



--[ServiceInHandymanInBuildingInServiceCall_Select_BuildingId-ServiceCallId] 1,'BCA16CEA-A1D6-4B2E-BD34-7ABC1621680A'