CREATE Procedure questiontitlebul @tag_id int 
As
	SELECT questiontitle_id FROM Tags WHERE tag_id = @tag_id
GO