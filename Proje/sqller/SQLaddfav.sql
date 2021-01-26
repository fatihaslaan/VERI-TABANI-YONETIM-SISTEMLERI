Create procedure addfav @personid int, @questid int
As
	Insert into Favorite(person_id,questiontitle_id) Values(@personid,@questid)
Go