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







