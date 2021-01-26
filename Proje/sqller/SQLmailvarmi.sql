Create procedure mailvarmibak @mail text
As
	Select personname from Person where Convert(varchar,personmail)=Convert(varchar,@mail)
go