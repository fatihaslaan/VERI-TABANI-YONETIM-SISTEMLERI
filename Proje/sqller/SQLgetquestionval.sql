Create procedure getquestionval @questiontitleid int
As

	Select value from Question where questiontitle_id=@questiontitleid

Go