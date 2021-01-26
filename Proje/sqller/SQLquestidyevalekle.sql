Create procedure questidyevalekle @val text,@questid int
As
	Update Question Set value=@val where questiontitle_id=@questid
Go