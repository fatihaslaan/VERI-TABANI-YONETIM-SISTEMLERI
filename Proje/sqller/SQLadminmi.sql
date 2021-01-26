Create procedure adminmi @personid int
As
	Select person_id from Admin Where person_id=@personid
Go