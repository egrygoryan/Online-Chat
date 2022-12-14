-- *Provide your DB name*
USE [Chat]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 20-Sep-22 1:51:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConversationRooms]    Script Date: 20-Sep-22 1:51:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConversationRooms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ConversationRooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConversationRoomUser]    Script Date: 20-Sep-22 1:51:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConversationRoomUser](
	[RoomsId] [int] NOT NULL,
	[UsersId] [int] NOT NULL,
 CONSTRAINT [PK_ConversationRoomUser] PRIMARY KEY CLUSTERED 
(
	[RoomsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 20-Sep-22 1:51:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[FromUserId] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[ReplyToId] [int] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20-Sep-22 1:51:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220907103744_Init', N'6.0.9')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220913111940_renameTable', N'6.0.9')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220919102014_RepliedColumn', N'6.0.9')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220920104206_renameColumnReplieToId', N'6.0.9')
GO
SET IDENTITY_INSERT [dbo].[ConversationRooms] ON 

INSERT [dbo].[ConversationRooms] ([Id], [RoomName]) VALUES (1, N'private for 1st and 2nd')
INSERT [dbo].[ConversationRooms] ([Id], [RoomName]) VALUES (2, N'public for all users')
SET IDENTITY_INSERT [dbo].[ConversationRooms] OFF
GO
INSERT [dbo].[ConversationRoomUser] ([RoomsId], [UsersId]) VALUES (1, 4)
INSERT [dbo].[ConversationRoomUser] ([RoomsId], [UsersId]) VALUES (2, 4)
INSERT [dbo].[ConversationRoomUser] ([RoomsId], [UsersId]) VALUES (1, 5)
INSERT [dbo].[ConversationRoomUser] ([RoomsId], [UsersId]) VALUES (2, 5)
INSERT [dbo].[ConversationRoomUser] ([RoomsId], [UsersId]) VALUES (2, 6)
GO
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([Id], [Content], [FromUserId], [RoomId], [ReplyToId]) VALUES (1, N'Hi, second', 4, 1, NULL)
INSERT [dbo].[Messages] ([Id], [Content], [FromUserId], [RoomId], [ReplyToId]) VALUES (2, N'Hi, first. How are you?', 5, 1, NULL)
INSERT [dbo].[Messages] ([Id], [Content], [FromUserId], [RoomId], [ReplyToId]) VALUES (3, N'Hey guys, are you here?', 6, 2, NULL)
SET IDENTITY_INSERT [dbo].[Messages] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [UserName]) VALUES (4, N'first')
INSERT [dbo].[Users] ([Id], [UserName]) VALUES (5, N'second')
INSERT [dbo].[Users] ([Id], [UserName]) VALUES (6, N'third')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Messages] ADD  DEFAULT ((0)) FOR [RoomId]
GO
ALTER TABLE [dbo].[ConversationRoomUser]  WITH CHECK ADD  CONSTRAINT [FK_ConversationRoomUser_ConversationRooms_RoomsId] FOREIGN KEY([RoomsId])
REFERENCES [dbo].[ConversationRooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ConversationRoomUser] CHECK CONSTRAINT [FK_ConversationRoomUser_ConversationRooms_RoomsId]
GO
ALTER TABLE [dbo].[ConversationRoomUser]  WITH CHECK ADD  CONSTRAINT [FK_ConversationRoomUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ConversationRoomUser] CHECK CONSTRAINT [FK_ConversationRoomUser_Users_UsersId]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_ConversationRooms_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[ConversationRooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_ConversationRooms_RoomId]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users_FromUserId] FOREIGN KEY([FromUserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users_FromUserId]
GO
