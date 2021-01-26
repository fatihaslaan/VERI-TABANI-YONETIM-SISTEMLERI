Create procedure getvoteup @questiontitleid int
As
	Select value from Voteup where questiontitle_id=@questiontitleid
Go