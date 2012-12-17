CREATE TRIGGER insteadOf
ON Message
INSTEAD OF DELETE
AS
	DELETE Recipient
	WHERE Recipient.MessageId in(SELECT Id FROM deleted)
	DELETE Message 
	WHERE Id in (SELECT Id FROM deleted)
GO