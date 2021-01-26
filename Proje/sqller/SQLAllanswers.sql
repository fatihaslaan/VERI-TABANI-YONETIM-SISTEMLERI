Create procedure allanswers @questiontitleid int 
As
	Select * from Answer where questiontitle_id=@questiontitleid
Go