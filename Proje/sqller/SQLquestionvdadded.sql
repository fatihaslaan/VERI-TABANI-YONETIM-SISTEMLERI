Create trigger questionvdadded
on Questiontitle
After Insert
As
Begin
		Set NOCOUNT ON 
		Declare @questid int
		Select @questid =inserted.questiontitle_id from inserted
		Insert into Votedown(questiontitle_id,value) Values(@questid,0)
End

