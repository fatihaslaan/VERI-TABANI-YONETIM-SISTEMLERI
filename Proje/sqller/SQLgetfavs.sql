Create procedure getfavs @personid int
As 
	Select questiontitle_id from Favorite where person_id=@personid
Go