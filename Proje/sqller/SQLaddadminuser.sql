Create trigger addadminanduser
on Person
After Insert
As
Begin
		Set NOCOUNT ON 
		Declare @iadmin int
		Declare @iuser int 
		Declare @personid int
		Select @iadmin =inserted.admin from inserted
		Select @iuser=inserted.normaluser from inserted
		Select @personid =inserted.person_id from inserted
		if(@iadmin=1)
			Insert into Admin(person_id) Values(@personid)
		if(@iuser=1)
			Insert into Normaluser(person_id) Values(@personid)
End


