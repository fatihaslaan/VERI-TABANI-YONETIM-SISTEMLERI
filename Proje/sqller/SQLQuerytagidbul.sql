CREATE Procedure tagidbul @tag_name text
AS
	SELECT tag_id FROM Tag WHERE CONVERT(VARCHAR, tagname) = CONVERT(VARCHAR,@tag_name)
GO