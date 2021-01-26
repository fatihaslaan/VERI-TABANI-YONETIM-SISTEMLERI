Create Procedure getlocation @locationid int
As
	Select location from Location where location_id=@locationid
Go