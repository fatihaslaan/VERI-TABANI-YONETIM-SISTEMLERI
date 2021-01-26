Create procedure questionnamedenidbul @questionname text
As
	Select questiontitle_id from Questiontitle where CONVERT(varchar,value)=CONVERT(varchar,@questionname)
Go