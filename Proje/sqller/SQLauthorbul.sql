Create procedure authorbul @personid int
As
	Select personname from Person Where person_id=@personid
Go