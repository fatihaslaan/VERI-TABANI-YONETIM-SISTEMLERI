Create procedure addquestion @val text, @personid int, @tagid int
As
	Insert into Questiontitle(value,person_id,tag_id) Values(@val,@personid,@tagid)
Go