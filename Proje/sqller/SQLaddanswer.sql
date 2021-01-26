Create procedure addanswer @questid int ,@personid int, @val text
As
	Insert into Answer(questiontitle_id,person_id,value) Values(@questid,@personid,@val)
Go