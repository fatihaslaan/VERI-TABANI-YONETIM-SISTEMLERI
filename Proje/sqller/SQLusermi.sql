Create procedure usermi @personid int
As
	Select person_id from Normaluser Where person_id=@personid
Go