Create procedure getvotedown @questiontitleid int
As
	Select value from Votedown where questiontitle_id=@questiontitleid
Go