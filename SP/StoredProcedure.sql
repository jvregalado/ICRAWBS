USE [InozaConferenceDB]
GO
/****** Object:  Table [dbo].[appSysUsers]    Script Date: 08/17/2022 7:03:00 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[appSysUsers](
	[PK_appSysUsers] [int] IDENTITY(1000,1) NOT NULL,
	[emailadd] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[userlevel] [varchar](50) NULL,
 CONSTRAINT [PK_appSysUsers] PRIMARY KEY CLUSTERED 
(
	[PK_appSysUsers] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[bookingMstr]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bookingMstr](
	[PK_bookingMstr] [int] IDENTITY(1000,1) NOT NULL,
	[EmailAdd] [varchar](50) NULL,
	[LocationName] [varchar](100) NULL,
	[BookingType] [varchar](50) NULL,
	[DateTimeFrom] [datetime] NULL,
	[DateTimeTo] [datetime] NULL,
	[BookingReason] [varchar](max) NULL,
	[BookingStatus] [varchar](50) NULL,
	[BookingRemarks] [varchar](200) NULL,
	[BookingDateTime] [datetime] NULL,
	[BookingAttachments] [varchar](max) NULL,
	[UpdatedDateTime] [datetime] NULL,
	[ReservedDateTime] [datetime] NULL,
	[ConfirmDateTime] [datetime] NULL,
	[ConfirmationImage] [varbinary](50) NULL,
 CONSTRAINT [PK_bookingMstr] PRIMARY KEY CLUSTERED 
(
	[PK_bookingMstr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[empMstr]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[empMstr](
	[PK_EmpMstr] [int] IDENTITY(1000,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[SuffixName] [varchar](50) NULL,
	[EmailAdd] [varchar](100) NULL,
	[MobileNo] [varchar](20) NULL,
	[EmployeeID] [varchar](50) NULL,
	[Password] [varchar](max) NULL,
	[Userlevel] [varchar](50) NULL,
	[Active] [bit] NULL,
	[ConferenceRoomAccess] [bit] NULL,
	[WorkStationAccess] [bit] NULL,
	[ParkingAccess] [bit] NULL,
	[EndodedDateTime] [datetime] NULL,
	[UpdatedDateTime] [datetime] NULL,
 CONSTRAINT [PK_empMstr] PRIMARY KEY CLUSTERED 
(
	[PK_EmpMstr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[locationMstr]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[locationMstr](
	[PK_locationMstr] [int] IDENTITY(1000,1) NOT NULL,
	[LocationName] [varchar](100) NULL,
	[LocDescription] [varchar](max) NULL,
	[LocType] [varchar](100) NULL,
	[LocStatus] [varchar](100) NULL,
	[EndodedDateTime] [datetime] NULL,
	[UpdatedDateTime] [datetime] NULL,
 CONSTRAINT [PK_locationMstr] PRIMARY KEY CLUSTERED 
(
	[PK_locationMstr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddConferenceBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	AddConferenceBooking
-- =============================================
CREATE PROCEDURE [dbo].[AddConferenceBooking]
	@EmailAdd varchar(50),
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 datetime,
	@BookingReason varchar(max),
	@BookingStatus varchar(50),
	@BookingRemarks varchar(200)

AS
BEGIN
	/*if exists (select * from bookingMstr where (BookingStatus <> 'Booked' or BookingStatus <> 'Reserved') and LocationName = @LocationName and ((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (DateTimeTo BETWEEN @DateTime1 and @DateTime2)))
		BEGIN
			update bookingMstr set
			LocationName = @LocationName,
			DateTimeFrom = @DateTime1,
			DateTimeTo = @DateTime2,
			BookingReason = @BookingReason,
			BookingStatus = @BookingStatus,
			BookingRemarks = @BookingRemarks,
			UpdatedDateTime = CURRENT_TIMESTAMP
		END

	ELSE*/
		BEGIN
		insert into bookingMstr 
		(EmailAdd,
		LocationName,
		BookingType,
		DateTimeFrom,
		DateTimeTo,
		BookingReason,
		BookingStatus,
		BookingRemarks,
		BookingDateTime,
		ReservedDateTime)
		values
		(@EmailAdd,
		@LocationName,
		'Conference Room',
		@DateTime1,
		@DateTime2,
		@BookingReason,
		@BookingStatus,
		@BookingRemarks,
		CURRENT_TIMESTAMP,
		CURRENT_TIMESTAMP)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[AddEmployee]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 18/07/2018
-- Description:	AddEmployee
-- =============================================
CREATE PROCEDURE [dbo].[AddEmployee] 
	@lastname varchar(50),
	@firstname varchar(50),
	@middlename varchar(50),
	@suffixname varchar(50),
	@emailadd varchar(100),
	@mobilenumber varchar(20),
	@EmployeeID varchar(50),
	@password varchar(max),
	@userlevel varchar(50),
	@Active bit,
	@ConferenceRoomAccess bit,
	@WorkStationAccess bit,
	@ParkingAccess bit,
	@PK_EmpMstr int
AS
BEGIN
	if exists (select * from empMstr where emailadd = @emailadd)
	BEGIN
		update empMstr set
		FirstName = @firstname,
		LastName = @lastname,
		MiddleName = @middlename,
		SuffixName = @suffixname,
		EmailAdd = @emailadd,
		MobileNo = @mobilenumber,
		EmployeeID = @EmployeeID,
		Userlevel = @userlevel,
		Active = @Active,
		ConferenceRoomAccess = @ConferenceRoomAccess,
		WorkStationAccess = @WorkStationAccess,
		ParkingAccess = @ParkingAccess,
		UpdatedDateTime = CURRENT_TIMESTAMP
		where PK_EmpMstr = @PK_EmpMstr
	END

	ELSE
	BEGIN
		insert into empMstr
		(FirstName,
		LastName,
		MiddleName,
		SuffixName,
		EmailAdd,
		MobileNo,
		EmployeeID,
		Password,
		Userlevel,
		Active,
		ConferenceRoomAccess,
		WorkStationAccess,
		ParkingAccess,
		EndodedDateTime)
		values
		(@firstname,
		@lastname,
		@middlename,
		@suffixname,
		@emailadd,
		@mobilenumber,
		@EmployeeID,
		@password,
		@userlevel,
		@Active,
		@ConferenceRoomAccess,
		@WorkStationAccess,
		@ParkingAccess,
		CURRENT_TIMESTAMP);
		insert into appSysUsers
		(emailadd,
		password,
		userlevel
		)
		values
		(@emailadd,
		@password,
		@userlevel)
		
	END
END
GO
/****** Object:  StoredProcedure [dbo].[AddLocation]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 25/07/2018
-- Description:	AddLocation
-- =============================================
CREATE PROCEDURE [dbo].[AddLocation] 
	@LocationName varchar(50),
	@Description varchar(100),
	@Type varchar(100),
	@Status varchar(100)
AS
BEGIN
	if exists (select * from locationMstr where locationname = @LocationName)
	BEGIN
		update locationMstr
		set
		LocationName = @LocationName,
		LocDescription = @Description,
		LocType = @Type,
		LocStatus = @Status,
		UpdatedDateTime = CURRENT_TIMESTAMP
		where LocationName = @LocationName
	END

	ELSE
	BEGIN
		insert into locationMstr
		(LocationName,
		LocDescription,
		LocType,
		LocStatus,
		EndodedDateTime)
		values
		(@LocationName,
		@Description,
		@Type,
		@Status,
		CURRENT_TIMESTAMP)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[AddParkingBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	AddParkingBooking
-- =============================================
CREATE PROCEDURE [dbo].[AddParkingBooking]
	@EmailAdd varchar(50),
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 datetime,
	@BookingReason varchar(max),
	@BookingStatus varchar(50),
	@BookingRemarks varchar(200)
	--@ConfirmationImage VarBinary(50)

AS
BEGIN
	/*if exists (select * from bookingMstr where (BookingStatus <> 'Booked' or BookingStatus <> 'Reserved') and LocationName = @LocationName and ((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (DateTimeTo BETWEEN @DateTime1 and @DateTime2)))
		BEGIN
			update bookingMstr set
			LocationName = @LocationName,
			DateTimeFrom = @DateTime1,
			DateTimeTo = @DateTime2,
			BookingReason = @BookingReason,
			BookingStatus = @BookingStatus,
			BookingRemarks = @BookingRemarks,
			UpdatedDateTime = CURRENT_TIMESTAMP
			--@ConfirmationImage = @ConfirmationImage
		END

	ELSE*/
		BEGIN
		insert into bookingMstr 
		(EmailAdd,
		LocationName,
		BookingType,
		DateTimeFrom,
		DateTimeTo,
		BookingReason,
		BookingStatus,
		BookingRemarks,
		BookingDateTime,
		ReservedDateTime)
		--ConfirmationImage)
		values
		(@EmailAdd,
		@LocationName,
		'Parking',
		@DateTime1,
		@DateTime2,
		@BookingReason,
		@BookingStatus,
		@BookingRemarks,
		CURRENT_TIMESTAMP,
		CURRENT_TIMESTAMP)
		--@ConfirmationImage)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[AddWorkStationBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	AddWorkStationBooking
-- =============================================
CREATE PROCEDURE [dbo].[AddWorkStationBooking]
	@EmailAdd varchar(50),
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 datetime,
	@BookingReason varchar(max),
	@BookingStatus varchar(50),
	@BookingRemarks varchar(200)

AS
BEGIN
	/*if exists (select * from bookingMstr where (BookingStatus <> 'Booked' or BookingStatus <> 'Reserved') and LocationName = @LocationName and ((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (DateTimeTo BETWEEN @DateTime1 and @DateTime2)))
		BEGIN
			update bookingMstr set
			LocationName = @LocationName,
			DateTimeFrom = @DateTime1,
			DateTimeTo = @DateTime2,
			BookingReason = @BookingReason,
			BookingStatus = @BookingStatus,
			BookingRemarks = @BookingRemarks,
			UpdatedDateTime = CURRENT_TIMESTAMP
		END

	ELSE*/
		BEGIN
		insert into bookingMstr 
		(EmailAdd,
		LocationName,
		BookingType,
		DateTimeFrom,
		DateTimeTo,
		BookingReason,
		BookingStatus,
		BookingRemarks,
		BookingDateTime,
		ReservedDateTime)
		values
		(@EmailAdd,
		@LocationName,
		'WorkStation',
		@DateTime1,
		@DateTime2,
		@BookingReason,
		@BookingStatus,
		@BookingRemarks,
		CURRENT_TIMESTAMP,
		CURRENT_TIMESTAMP)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[CheckConferenceBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2017
-- Description:	CheckConferenceBooking
-- =============================================
CREATE PROCEDURE [dbo].[CheckConferenceBooking] 
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 DateTime
AS
BEGIN
	select count(*) from bookingMstr
	where BookingStatus <> 'Cancelled' and BookingType ='Conference Room' and
	LocationName = @LocationName and
	--((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or
	--(DateTimeTo BETWEEN @DateTime1 and @DateTime2) or
	--((@DateTime1 >= DateTimeFrom) and (@DateTime1 < DateTimeTo)) or ((@DateTime2 >= DateTimeFrom) and (@DateTime2 < DateTimeTo))
	--((DateTimeFrom >= @DateTime1 and DatetimeFrom <= @DateTime2) or (DateTimeTo <= @DateTime1 and DatetimeTo <= @DateTime2))
	((@DateTime1 = DateTimeFrom) OR 
	(@DateTime2 = DateTimeTo) OR 
	(DateTimeFROM>@DateTime1 AND DateTimeTO<@DateTime2) OR
	(DateTimeFROM<@DateTime1 AND DateTimeTO>@DateTime1) OR
	(DateTimeFROM<@DateTime2 AND DateTimeTO>@DateTime2))
END
GO
/****** Object:  StoredProcedure [dbo].[CheckConferenceBooking_backup]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2017
-- Description:	CheckConferenceBooking
-- =============================================
CREATE PROCEDURE [dbo].[CheckConferenceBooking_backup] 
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 DateTime
AS
BEGIN
	select count(*) from bookingMstr
	where BookingStatus <> 'Cancelled' and
	LocationName = @LocationName and
	--((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or
	--(DateTimeTo BETWEEN @DateTime1 and @DateTime2) or
	((@DateTime1 >= DateTimeFrom) and (@DateTime1 < DateTimeTo)) or ((@DateTime2 >= DateTimeFrom) and (@DateTime2 < DateTimeTo))
	--((DateTimeFrom >= @DateTime1 and DatetimeFrom <= @DateTime2) or (DateTimeTo <= @DateTime1 and DatetimeTo <= @DateTime2))
END
GO
/****** Object:  StoredProcedure [dbo].[CheckLocExist]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 25/07/2017
-- Description:	CheckLocExist
-- =============================================
CREATE PROCEDURE [dbo].[CheckLocExist] 
	@LocName varchar(50)
AS
BEGIN
	select count(*) from locationMstr where locationname = @LocName
END
GO
/****** Object:  StoredProcedure [dbo].[CheckOldPassword]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ronald Abrera
-- Create date: 18/2/2019
-- Description:	CheckOldPassword
-- =============================================
CREATE PROCEDURE [dbo].[CheckOldPassword] 
	-- Add the parameters for the stored procedure here
	@OldPassword varchar(100),
	@User varchar(100)
AS
BEGIN
	select count(*) from empMstr where password = @OldPassword and emailadd = @User
END
GO
/****** Object:  StoredProcedure [dbo].[CheckParkingBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2017
-- Description:	CheckParkingBooking
-- =============================================
CREATE PROCEDURE [dbo].[CheckParkingBooking] 
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 DateTime
AS
BEGIN
	select count(*) from bookingMstr
	where BookingStatus <> 'Cancelled' and BookingType ='Parking' and
	LocationName = @LocationName and
	--((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or
	--(DateTimeTo BETWEEN @DateTime1 and @DateTime2) or
	--((@DateTime1 >= DateTimeFrom) and (@DateTime1 < DateTimeTo)) or ((@DateTime2 >= DateTimeFrom) and (@DateTime2 < DateTimeTo))
	--((DateTimeFrom >= @DateTime1 and DatetimeFrom <= @DateTime2) or (DateTimeTo <= @DateTime1 and DatetimeTo <= @DateTime2))
	((@DateTime1 = DateTimeFrom) OR 
	(@DateTime2 = DateTimeTo) OR 
	(DateTimeFROM>@DateTime1 AND DateTimeTO<@DateTime2) OR
	(DateTimeFROM<@DateTime1 AND DateTimeTO>@DateTime1) OR
	(DateTimeFROM<@DateTime2 AND DateTimeTO>@DateTime2))
END
GO
/****** Object:  StoredProcedure [dbo].[CheckParkingUserDuplicateBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2017
-- Description:	CheckConferenceBooking
-- =============================================
CREATE PROCEDURE [dbo].[CheckParkingUserDuplicateBooking] 
	@User varchar(100),
	@DateTime1 datetime,
	@DateTime2 DateTime
AS
BEGIN
	select count(*) from bookingMstr
	where BookingStatus <> 'Cancelled' and BookingType ='Parking' and
	EmailAdd = @User and
	--((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or
	--(DateTimeTo BETWEEN @DateTime1 and @DateTime2) or
	--((@DateTime1 >= DateTimeFrom) and (@DateTime1 < DateTimeTo)) or ((@DateTime2 >= DateTimeFrom) and (@DateTime2 < DateTimeTo))
	--((DateTimeFrom >= @DateTime1 and DatetimeFrom <= @DateTime2) or (DateTimeTo <= @DateTime1 and DatetimeTo <= @DateTime2))
	/*((@DateTime1 = DateTimeFrom) OR 
	(@DateTime2 = DateTimeTo) OR 
	(DateTimeFROM>@DateTime1 AND DateTimeTO<@DateTime2) OR
	(DateTimeFROM<@DateTime1 AND DateTimeTO>@DateTime1) OR
	(DateTimeFROM<@DateTime2 AND DateTimeTO>@DateTime2))*/
	(convert(varchar(10),@DateTime1, 120) = convert(varchar(10),DateTimeFrom, 120)) OR 
	(convert(varchar(10),@DateTime2, 120) = convert(varchar(10),DateTimeTo, 120)) OR 
	(convert(varchar(10),DateTimeFrom, 120)>convert(varchar(10),@DateTime1, 120) AND convert(varchar(10),DateTimeTo, 120)<convert(varchar(10),@DateTime2, 120)) OR
	(convert(varchar(10),DateTimeFrom, 120)<convert(varchar(10),@DateTime1, 120) AND convert(varchar(10),DateTimeTo, 120)>convert(varchar(10),@DateTime1, 120)) OR
	(convert(varchar(10),DateTimeFrom, 120)<convert(varchar(10),@DateTime2, 120) AND convert(varchar(10),DateTimeTo, 120)>convert(varchar(10),@DateTime2, 120))
END
GO
/****** Object:  StoredProcedure [dbo].[CheckUserExist]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 18/07/2017
-- Description:	CheckUserExist
-- =============================================
CREATE PROCEDURE [dbo].[CheckUserExist] 
	@empid varchar(50),
	@emailadd varchar(100)
AS
BEGIN
	--select count(*) from empMstr where EmployeeID = @empid or emailadd = @emailadd
	select count(*) from empMstr where emailadd = @emailadd
END
GO
/****** Object:  StoredProcedure [dbo].[CheckWorkstationBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2017
-- Description:	CheckConferenceBooking
-- =============================================
CREATE PROCEDURE [dbo].[CheckWorkstationBooking] 
	@LocationName varchar(100),
	@DateTime1 datetime,
	@DateTime2 DateTime
AS
BEGIN
	select count(*) from bookingMstr
	where BookingStatus <> 'Cancelled' and BookingType ='Workstation' and
	LocationName = @LocationName and
	--((DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or
	--(DateTimeTo BETWEEN @DateTime1 and @DateTime2) or
	--((@DateTime1 >= DateTimeFrom) and (@DateTime1 < DateTimeTo)) or ((@DateTime2 >= DateTimeFrom) and (@DateTime2 < DateTimeTo))
	--((DateTimeFrom >= @DateTime1 and DatetimeFrom <= @DateTime2) or (DateTimeTo <= @DateTime1 and DatetimeTo <= @DateTime2))
	((@DateTime1 = DateTimeFrom) OR 
	(@DateTime2 = DateTimeTo) OR 
	(DateTimeFROM>@DateTime1 AND DateTimeTO<@DateTime2) OR
	(DateTimeFROM<@DateTime1 AND DateTimeTO>@DateTime1) OR
	(DateTimeFROM<@DateTime2 AND DateTimeTO>@DateTime2))
END
GO
/****** Object:  StoredProcedure [dbo].[ConferenceRoomBookingList]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	ConferenceRoomBookingList
-- =============================================
CREATE PROCEDURE [dbo].[ConferenceRoomBookingList] 
	@DateTime1 datetime,
	@DateTime2 datetime,
	@Location varchar(250),
	@BookView varchar(250)
AS
BEGIN
	IF @Location = ''
	BEGIN
		IF @BookView = 'Reserved'
		BEGIN
			select 
			a.PK_bookingMstr,
			b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
			LocationName as 'Conference Room',
			CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
			CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
			BookingReason as 'Reason',
			BookingRemarks as 'Remarks',
			BookingStatus as 'Status'

			from
			bookingMstr a
			left join empMstr b
			on a.EmailAdd = b.EmailAdd

			where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
			and a.BookingStatus = 'Reserved' and a.BookingType = 'Conference Room' ORDER BY DateTimeFrom ASC
		END
		/*
		ELSE IF @BookView = 'Booked'
		BEGIN
			select 
			a.PK_bookingMstr,
			b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
			LocationName as 'Conference Room',
			CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
			CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
			BookingReason as 'Reason',
			BookingRemarks as 'Remarks',
			BookingStatus as 'Status'

			from
			bookingMstr a
			left join empMstr b
			on a.EmailAdd = b.EmailAdd

			where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
			and a.BookingStatus = 'Booked' and a.BookingType = 'Conference Room'
		END*/
		ELSE IF @BookView = 'Cancelled'
		BEGIN
			select 
			a.PK_bookingMstr,
			b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
			LocationName as 'Conference Room',
			CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
			CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
			BookingReason as 'Reason',
			BookingRemarks as 'Remarks',
			BookingStatus as 'Status'

			from
			bookingMstr a
			left join empMstr b
			on a.EmailAdd = b.EmailAdd

			where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
			and a.BookingStatus = 'Cancelled' and a.BookingType = 'Conference Room' ORDER BY DateTimeFrom ASC
		END
	END
	ELSE
	BEGIN
	IF @BookView = 'Reserved'
		BEGIN
			select 
			a.PK_bookingMstr,
			b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
			LocationName as 'Conference Room',
			CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
			CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
			BookingReason as 'Reason',
			BookingRemarks as 'Remarks',
			BookingStatus as 'Status'

			from
			bookingMstr a
			left join empMstr b
			on a.EmailAdd = b.EmailAdd

			where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
			and a.BookingStatus = 'Reserved' and a.BookingType = 'Conference Room' and a.LocationName= @Location ORDER BY DateTimeFrom ASC
		END
		/*ELSE IF @BookView = 'Booked'
		BEGIN
			select 
			a.PK_bookingMstr,
			b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
			LocationName as 'Conference Room',
			CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
			CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
			BookingReason as 'Reason',
			BookingRemarks as 'Remarks',
			BookingStatus as 'Status'

			from
			bookingMstr a
			left join empMstr b
			on a.EmailAdd = b.EmailAdd

			where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
			and a.BookingStatus = 'Booked' and a.BookingType = 'Conference Room'
		END*/
		ELSE IF @BookView = 'Cancelled'
		BEGIN
			select 
			a.PK_bookingMstr,
			b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
			LocationName as 'Conference Room',
			CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
			CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
			BookingReason as 'Reason',
			BookingRemarks as 'Remarks',
			BookingStatus as 'Status'

			from
			bookingMstr a
			left join empMstr b
			on a.EmailAdd = b.EmailAdd

			where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
			and a.BookingStatus = 'Cancelled' and a.BookingType = 'Conference Room' and a.LocationName= @Location ORDER BY DateTimeFrom ASC
		END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[EmployeeFullName]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 07/18/18
-- Description:	Employee Full Name
-- =============================================
CREATE PROCEDURE [dbo].[EmployeeFullName]
	@emailadd varchar(50)
AS
BEGIN
	SELECT firstname + ' ' + left(middlename,1) + ' ' + lastname,
	Userlevel
	from empMstr where emailadd = @emailadd
END
GO
/****** Object:  StoredProcedure [dbo].[EmployeeList]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 18/07/18
-- Description:	Employee List
-- =============================================
CREATE PROCEDURE [dbo].[EmployeeList] 
	@EmpName varchar(50)
AS
BEGIN
	SELECT 
	PK_EmpMstr,
	firstname + ' ' + ISNULL(LEFT(middlename,1),'') + ' ' + lastname + ' ' + suffixname as 'Employee Name',
	emailadd as 'E-mail Address',
	mobileno as 'Mobile Number',
	Active,
	ConferenceRoomAccess as 'Conference Room Access',
	WorkStationAccess as 'WorkStation Access',
	ParkingAccess as 'Parking Accesss'
	from empMstr
	where (firstname like '%'+@empname+'%' or 
	lastname like '%'+@empname+'%' or
	middlename like '%'+@empname+'%')
	and emailadd <> 'admin'
END
GO
/****** Object:  StoredProcedure [dbo].[LocationNameList]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 27/07/2018
-- Description:	LocationNameList
-- =============================================
CREATE PROCEDURE [dbo].[LocationNameList] 

AS
BEGIN
	select Locationname from locationMstr
END
GO
/****** Object:  StoredProcedure [dbo].[LocList]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 07/25/18
-- Description:	Location List
-- =============================================
CREATE PROCEDURE [dbo].[LocList] 
	@LocName varchar(50)
AS
BEGIN
	SELECT 
	PK_locationMstr,
	LocatioNname as 'Location Name',
	LocDescription as 'Description',
	LocType as 'Type',
	LocStatus as 'Status'
	from locationMstr
	where locationname like '%'+@LocName+'%'
END
GO
/****** Object:  StoredProcedure [dbo].[ParkingBookingList]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	ConferenceRoomBookingList
-- =============================================
CREATE PROCEDURE [dbo].[ParkingBookingList] 
	@DateTime1 datetime,
	@DateTime2 datetime,
	@BookView varchar(250)
AS
BEGIN
	IF @BookView = 'Reserved'
	BEGIN
		select 
		a.PK_bookingMstr,
		b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
		LocationName as 'Parking',
		CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
		CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
		BookingReason as 'Reason',
		BookingRemarks as 'Remarks',
		BookingStatus as 'Status'
		from
		bookingMstr a
		left join empMstr b
		on a.EmailAdd = b.EmailAdd

		where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
		and a.BookingStatus = 'Reserved' and a.BookingType = 'Parking'
	END
	ELSE IF @BookView = 'Booked'
	BEGIN
		select 
		a.PK_bookingMstr,
		b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
		LocationName as 'Parking',
		CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
		CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
		BookingReason as 'Reason',
		BookingRemarks as 'Remarks',
		BookingStatus as 'Status'
		from
		bookingMstr a
		left join empMstr b
		on a.EmailAdd = b.EmailAdd

		where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
		and a.BookingStatus = 'Booked' and a.BookingType = 'Parking'
	END
	ELSE IF @BookView = 'Cancelled'
	BEGIN
		select 
		a.PK_bookingMstr,
		b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
		LocationName as 'Parking',
		CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
		CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
		BookingReason as 'Reason',
		BookingRemarks as 'Remarks',
		BookingStatus as 'Status'
		from
		bookingMstr a
		left join empMstr b
		on a.EmailAdd = b.EmailAdd

		where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
		and a.BookingStatus = 'Cancelled' and a.BookingType = 'Parking'
	END
END
GO
/****** Object:  StoredProcedure [dbo].[ReadBookingDetails]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	ReadBookingDetails
-- =============================================
CREATE PROCEDURE [dbo].[ReadBookingDetails]
	@PK_bookingMstr int
AS
BEGIN
	select LocationName,
	DateTimeFrom,
	DateTimeTo,
	BookingReason,
	BookingRemarks
	from
	bookingMstr
	where PK_bookingMstr = @PK_bookingMstr
END
GO
/****** Object:  StoredProcedure [dbo].[ReadEmpDetails]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 24/07/2018
-- Description:	ReadEmployeeDetatails
-- =============================================
CREATE PROCEDURE [dbo].[ReadEmpDetails]
	@PK_EmpMstr int
AS
BEGIN
	select firstname,
	middlename,
	lastname,
	suffixname,
	emailadd,
	mobileno,
	EmployeeID,
	Userlevel,
	Active,
	ConferenceRoomAccess,
	WorkStationAccess,
	ParkingAccess
	from empMstr
	where PK_EmpMstr = @PK_EmpMstr
END
GO
/****** Object:  StoredProcedure [dbo].[ReadLocationDetatails]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 25/07/2018
-- Description:	ReadLocationDetatails
-- =============================================
CREATE PROCEDURE [dbo].[ReadLocationDetatails]
	@PK_locationMstr int
AS
BEGIN
	select LocationName,
	LocDescription,
	LocType,
	LocStatus
	from locationMstr
	where PK_locationMstr = @PK_locationMstr
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ChangePassword]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 15/2/2019
-- Description:	Change Password
-- =============================================
CREATE PROCEDURE [dbo].[SP_ChangePassword]
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(250),
	@NewPassword VARCHAR(250)
AS
BEGIN
	UPDATE empMstr SET Password = @NewPassword WHERE EmailAdd = @Username
	UPDATE appSysUsers SET Password = @NewPassword WHERE emailadd = @Username
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateBooking]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	UpdateBooking
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBooking]
	@PK_bookingMstr int,
	@StatusUpdate varchar(50)
AS
BEGIN
	update bookingMstr
	set
	BookingStatus = @StatusUpdate,
	ConfirmDateTime = CURRENT_TIMESTAMP
	where PK_bookingMstr = @PK_bookingMstr
END
GO
/****** Object:  StoredProcedure [dbo].[WorkStationBookingList]    Script Date: 08/17/2022 7:03:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CDI
-- Create date: 30/07/2018
-- Description:	WorkStationBookingList
-- =============================================
CREATE PROCEDURE [dbo].[WorkStationBookingList] 
	@DateTime1 datetime,
	@DateTime2 datetime,
	@BookView varchar(250)
AS
BEGIN
	IF @BookView = 'Reserved'
	BEGIN
		select 
		a.PK_bookingMstr,
		b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
		LocationName as 'WorkStation',
		CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
		CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
		BookingReason as 'Reason',
		BookingRemarks as 'Remarks',
		BookingStatus as 'Status'

		from
		bookingMstr a
		left join empMstr b
		on a.EmailAdd = b.EmailAdd

		where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
		and a.BookingStatus = 'Reserved' and a.BookingType = 'WorkStation'
	END
	ELSE IF @BookView = 'Booked'
	BEGIN
		select 
		a.PK_bookingMstr,
		b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
		LocationName as 'WorkStation',
		CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
		CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
		BookingReason as 'Reason',
		BookingRemarks as 'Remarks',
		BookingStatus as 'Status'

		from
		bookingMstr a
		left join empMstr b
		on a.EmailAdd = b.EmailAdd

		where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
		and a.BookingStatus = 'Booked' and a.BookingType = 'WorkStation'
	END
	ELSE IF @BookView = 'Cancelled'
	BEGIN
		select 
		a.PK_bookingMstr,
		b.firstname + ' ' + left(b.middlename,1) + ' ' + b.lastname as 'Employee Name',
		LocationName as 'WorkStation',
		CONVERT(VARCHAR(20), DateTimeFrom, 100) as 'From',
		CONVERT(VARCHAR(20), DateTimeTo, 100) as 'To',
		BookingReason as 'Reason',
		BookingRemarks as 'Remarks',
		BookingStatus as 'Status'

		from
		bookingMstr a
		left join empMstr b
		on a.EmailAdd = b.EmailAdd

		where ((a.DateTimeFrom BETWEEN @DateTime1 and @DateTime2) or (a.DateTimeTo BETWEEN @DateTime1 and @DateTime2))
		and a.BookingStatus = 'Cancelled' and a.BookingType = 'WorkStation'
	END
END
GO
