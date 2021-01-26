Create procedure questlerimibul @personid int
As
	Select value from Questiontitle where person_id=@personid
Go