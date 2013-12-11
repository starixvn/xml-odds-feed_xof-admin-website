 USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_Domain]    Script Date: 02/27/2010 16:30:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_Domain](
	[DomainID] [smallint] IDENTITY(1,1) NOT NULL,
	[DomainName] [nvarchar](300) NULL,
	[CreatedDate] [datetime] NULL,
	[IP] [varchar](23) NULL,
	[ISP] [varchar](256) NULL,
	[Creator] [smallint] NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_Domain] PRIMARY KEY CLUSTERED 
(
	[DomainID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]

GO
SET ANSI_PADDING OFF

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_DomainSetting]    Script Date: 02/27/2010 16:34:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Odds_DomainSetting](
	[PartnerID] [smallint] NOT NULL,
	[DomainID] [smallint] NOT NULL,
 CONSTRAINT [PK_DomainSetting] PRIMARY KEY CLUSTERED 
(
	[PartnerID] ASC,
	[DomainID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_IPSetting]    Script Date: 02/27/2010 16:35:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_IPSetting](
	[PartnerID] [smallint] NOT NULL,
	[IPFrom] [bigint] NOT NULL,
	[IPTo] [bigint] NULL,
	[Country] [varchar](64) NULL,
	[Region] [varchar](128) NULL,
	[City] [varchar](128) NULL,
	[ISP] [varchar](256) NULL,
 CONSTRAINT [PK_IPSetting] PRIMARY KEY CLUSTERED 
(
	[PartnerID] ASC,
	[IPFrom] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]

GO
SET ANSI_PADDING OFF

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_LogType]    Script Date: 02/27/2010 16:35:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Odds_LogType](
	[TypeID] [smallint] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]


USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_OddsProfile]    Script Date: 02/27/2010 16:36:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_OddsProfile](
	[ProfileID] [smallint] IDENTITY(1,1) NOT NULL,
	[ProfileName] [varchar](50) NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[Creator] [smallint] NULL,
	[IsDeleted] [bit] NULL,
	[LastUpdate] [datetime] NULL,
	[TimeRange] [smallint] NULL,
	[SPName] [varchar](100) NULL,
	[SPParams] [varchar](100) NULL,
 CONSTRAINT [PK_OddsProfile] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]

GO
SET ANSI_PADDING OFF

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_Partner]    Script Date: 02/27/2010 16:37:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_Partner](
	[PartnerID] [int] IDENTITY(1,1) NOT NULL,
	[SystemID] [smallint] NULL,
	[ProfileID] [smallint] NULL,
	[UserName] [varchar](50) NOT NULL,
	[Site] [nvarchar](200) NULL,
	[IsAllowed] [bit] NULL,
	[Remark] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[LastRequest] [datetime] NULL,
	[Creator] [smallint] NULL,
 CONSTRAINT [PK_OddsQuotePartner] PRIMARY KEY CLUSTERED 
(
	[PartnerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]

GO
SET ANSI_PADDING OFF

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_Statistics]    Script Date: 02/27/2010 16:38:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_Statistics](
	[PartnerID] [smallint] NULL,
	[DomainID] [smallint] NULL,
	[ProfileID] [smallint] NULL,
	[IP] [varchar](23) NULL,
	[Date] [smalldatetime] NULL
) ON [StatData]

GO
SET ANSI_PADDING OFF

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_Statistics_BK]    Script Date: 02/27/2010 16:39:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_Statistics_BK](
	[PartnerID] [smallint] NULL,
	[DomainID] [smallint] NULL,
	[ProfileID] [smallint] NULL,
	[IP] [varchar](23) NULL,
	[Date] [smalldatetime] NULL
) ON [StatData]

GO
SET ANSI_PADDING OFF

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_StatisticsSum]    Script Date: 02/27/2010 16:40:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Odds_StatisticsSum](
	[PartnerID] [smallint] NOT NULL,
	[ProfileID] [smallint] NOT NULL,
	[DomainID] [smallint] NOT NULL,
	[IsForMonth] [bit] NOT NULL,
	[Date] [smalldatetime] NOT NULL,
	[TotalRequest] [bigint] NULL,
 CONSTRAINT [PK_StatisticsSum] PRIMARY KEY CLUSTERED 
(
	[PartnerID] ASC,
	[ProfileID] ASC,
	[DomainID] ASC,
	[IsForMonth] ASC,
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [StatData]
) ON [StatData]

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_System]    Script Date: 02/27/2010 16:41:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Odds_System](
	[SystemID] [smallint] IDENTITY(1,1) NOT NULL,
	[SystemName] [nvarchar](50) NULL,
	[Creator] [smallint] NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_OddsQuoteSystem] PRIMARY KEY CLUSTERED 
(
	[SystemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DATA]
) ON [DATA]

USE [bodb_Maintenance]
GO
/****** Object:  Table [dbo].[Odds_UserLog]    Script Date: 02/27/2010 16:42:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Odds_UserLog](
	[RefNo] [int] NULL,
	[TypeID] [tinyint] NOT NULL,
	[PartnerID] [smallint] NULL,
	[Creator] [smallint] NULL,
	[LoggedDate] [smalldatetime] NOT NULL,
	[SPName] [varchar](50) NOT NULL,
	[Detail] [nvarchar](4000) NOT NULL,
	[IP] [varchar](23) NOT NULL
) ON [UserLogData]

GO
SET ANSI_PADDING OFF