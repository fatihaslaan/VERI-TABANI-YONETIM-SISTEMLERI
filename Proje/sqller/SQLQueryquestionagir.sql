Create procedure questionagir @questiontitle_id int
As
	Select * From Questiontitle Where questiontitle_id=@questiontitle_id
Go