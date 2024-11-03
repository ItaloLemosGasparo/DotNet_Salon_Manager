-- ================================================== --
-- Creator    : Ítalo Lemos
-- Date       : 03/11/2024
-- Description: To Find an User by Email
-- ================================================== --
ALTER PROCEDURE SP_FIND_USER_BY_EMAIL
    @Email VARCHAR(100) 
AS
BEGIN
    select * from Users US
    INNER JOIN Passwords PASS ON PASS.UserId = US.UserId
    where US.Email = 'italolemos4@gmail.com'
END;