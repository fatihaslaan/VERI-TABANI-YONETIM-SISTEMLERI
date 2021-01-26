Create trigger questionqvaladded
on Questiontitle
After Insert
As
Begin
		Set NOCOUNT ON 
		Declare @questid int
		Select @questid =inserted.questiontitle_id from inserted
		Insert into Question(questiontitle_id) Values(@questid)
End

