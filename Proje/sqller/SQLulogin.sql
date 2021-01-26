Create procedure u_login @u_mail text,@u_password text
AS
	Select * from Person Where Convert(Varchar,personmail)=Convert(Varchar,@u_mail) and Convert(Varchar,personpassword)=Convert(Varchar,@u_password)
Go