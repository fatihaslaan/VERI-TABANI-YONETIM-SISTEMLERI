USE [Bisorumvar]
GO
/****** Object:  StoredProcedure [dbo].[kayit]    Script Date: 16.12.2019 11:17:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[kayit] @personname text, @personsurname text, @personmail text, @personpassword text, @locationid int,@normaluser int, @admin int
As
	INSERT INTO Person (personname,personsurname,personmail,personpassword,location_id,normaluser,admin) VALUES (@personname,@personsurname,@personmail,@personpassword,@locationid,@normaluser,@admin)
