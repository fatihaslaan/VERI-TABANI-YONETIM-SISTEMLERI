Create trigger questiontagadded
on Questiontitle
After Insert
As
Begin
		Set NOCOUNT ON 
		Declare @tagid int 
		Declare @questid int
		Select @tagid=inserted.tag_id from inserted
		Select @questid =inserted.questiontitle_id from inserted
		Insert into Tags(questiontitle_id,tag_id) Values(@questid,@tagid)
End

