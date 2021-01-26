Create procedure voted @questid int
As
	Update Votedown Set value=value+1 Where questiontitle_id=@questid
Go