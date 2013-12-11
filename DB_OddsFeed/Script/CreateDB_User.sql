 
USE [master]
GO
/****** Object:  Database [bodb_Maintenance]    Script Date: 02/27/2010 12:32:01 ******/
CREATE DATABASE [bodb_Maintenance] ON  PRIMARY 
( NAME = N'bodb_Maintenance', FILENAME = N'R:\IBC_DB\bodb_Maintenance\bodb_Maintenance.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB ),  
  FILEGROUP [DATA] 
( NAME = N'DATA', FILENAME = N'R:\IBC_DB\bodb_MaintenanceT\DATA.ndf' , SIZE = 131072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 32768KB ), 
 FILEGROUP [INDEX] 
( NAME = N'INDEX', FILENAME = N'R:\IBC_DB\bodb_Maintenance\INDEX.ndf' , SIZE = 131072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 32768KB ),  
 FILEGROUP [StatData] 
( NAME = N'StatData', FILENAME = N'R:\IBC_DB\bodb_Maintenance\StatData.ndf' , SIZE = 131072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 16384KB ), 
 FILEGROUP [StatIndex] 
( NAME = N'StatIndex', FILENAME = N'R:\IBC_DB\bodb_Maintenance\StatIndex.ndf' , SIZE = 131072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 16384KB ),  
 FILEGROUP [UserLogData] 
( NAME = N'UserLogData', FILENAME = N'R:\IBC_DB\bodb_Maintenance\UserLogData.ndf' , SIZE = 131072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 16384KB ), 
 FILEGROUP [UserLogIndex] 
( NAME = N'UserLogIndex', FILENAME = N'R:\IBC_DB\bodb_Maintenance\UserLogIndex.ndf' , SIZE = 131072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 16384KB )
 LOG ON 
( NAME = N'bodb_Maintenance_log', FILENAME = N'R:\IBC_DB\bodb_Maintenance\bodb_Maintenance_log.ldf' , SIZE = 123648KB , MAXSIZE = 2048GB , FILEGROWTH = 10%),
( NAME = N'bodb_Maintenance_log2', FILENAME = N'G:\IBC_DB\bodb_Maintenance\bodb_Maintenance_log2.ldf' , SIZE = 16384KB , MAXSIZE = 5120000KB , FILEGROWTH = 32768KB )

GO

GO
EXEC dbo.sp_dbcmptlevel @dbname=N'bodb_Maintenance', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bodb_Maintenance].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [bodb_Maintenance] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET ARITHABORT OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [bodb_Maintenance] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bodb_Maintenance] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bodb_Maintenance] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET  ENABLE_BROKER 
GO
ALTER DATABASE [bodb_Maintenance] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bodb_Maintenance] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bodb_Maintenance] SET  READ_WRITE 
GO
ALTER DATABASE [bodb_Maintenance] SET RECOVERY FULL 
GO
ALTER DATABASE [bodb_Maintenance] SET  MULTI_USER 
GO
ALTER DATABASE [bodb_Maintenance] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bodb_Maintenance] SET DB_CHAINING OFF 

USE [bodb_Maintenance]
GO
/****** Object:  User [bodbAdminNet]    Script Date: 10/28/2009 15:59:39 ******/
GO
CREATE USER [bodbAdminNet] FOR LOGIN [bodbAdminNet] WITH DEFAULT_SCHEMA=[dbo]
USE [bodb_Maintenance]
GO
/****** Object:  User [vnteam]    Script Date: 10/28/2009 16:02:30 ******/
GO
CREATE USER [vnteam] FOR LOGIN [vnteam] WITH DEFAULT_SCHEMA=[dbo]