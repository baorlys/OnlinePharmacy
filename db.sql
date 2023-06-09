USE [master]
GO
/****** Object:  Database [OnlinePharmacy]    Script Date: 3/29/2023 12:52:40 PM ******/
CREATE DATABASE [OnlinePharmacy]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OnlinePharmacy', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\OnlinePharmacy.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OnlinePharmacy_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\OnlinePharmacy_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OnlinePharmacy] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OnlinePharmacy].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OnlinePharmacy] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET ARITHABORT OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [OnlinePharmacy] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OnlinePharmacy] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OnlinePharmacy] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET  ENABLE_BROKER 
GO
ALTER DATABASE [OnlinePharmacy] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OnlinePharmacy] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OnlinePharmacy] SET  MULTI_USER 
GO
ALTER DATABASE [OnlinePharmacy] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OnlinePharmacy] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OnlinePharmacy] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OnlinePharmacy] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OnlinePharmacy] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OnlinePharmacy] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [OnlinePharmacy] SET QUERY_STORE = OFF
GO
USE [OnlinePharmacy]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 3/29/2023 12:52:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](max) NULL,
	[author] [nvarchar](100) NULL,
	[content] [nvarchar](max) NULL,
	[thumb] [varchar](max) NULL,
	[summary] [nvarchar](max) NULL,
	[tags] [varchar](50) NULL,
	[create_at] [datetime] NULL,
	[modified_at] [datetime] NULL,
	[deleted_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogTag]    Script Date: 3/29/2023 12:52:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogTag](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](30) NULL,
	[frequency] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 3/29/2023 12:52:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parentId] [int] NULL,
	[type] [nvarchar](30) NULL,
	[title] [nvarchar](30) NULL,
	[description] [nvarchar](500) NULL,
	[url] [nvarchar](max) NULL,
	[meta] [nvarchar](50) NULL,
	[hide] [bit] NULL,
	[order] [int] NULL,
	[create_at] [datetime] NULL,
	[modified_at] [datetime] NULL,
	[deleted_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 3/29/2023 12:52:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[categoryId] [int] NULL,
	[name] [nvarchar](max) NULL,
	[inventory] [int] NULL,
	[desc] [text] NULL,
	[image] [varchar](max) NULL,
	[price] [varchar](50) NULL,
	[unit] [nvarchar](20) NULL,
	[create_at] [datetime] NULL,
	[modified_at] [datetime] NULL,
	[deleted_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 3/29/2023 12:52:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parentId] [int] NULL,
	[name] [nvarchar](100) NULL,
	[meta] [nvarchar](50) NULL,
	[desc] [text] NULL,
	[create_at] [datetime] NULL,
	[modified_at] [datetime] NULL,
	[deleted_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[BlogTag] ADD  DEFAULT ((0)) FOR [frequency]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [parentId]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ('#') FOR [url]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [hide]
GO
ALTER TABLE [dbo].[ProductCategory] ADD  DEFAULT ((0)) FOR [parentId]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([categoryId])
REFERENCES [dbo].[ProductCategory] ([id])
GO
USE [master]
GO
ALTER DATABASE [OnlinePharmacy] SET  READ_WRITE 
GO
