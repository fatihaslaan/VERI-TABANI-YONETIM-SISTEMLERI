Create procedure voteu @questid int
As
	Update Voteup Set value=value+1 Where questiontitle_id=@questid
Go