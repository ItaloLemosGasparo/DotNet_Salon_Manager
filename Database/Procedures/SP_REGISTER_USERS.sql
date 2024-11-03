-- ================================================== --
-- Creator    : Ítalo Lemos
-- Date       : 03/11/2024
-- Description: To register Users
-- ================================================== --
ALTER PROCEDURE SP_REGISTER_USER
    @Name VARCHAR(100),
    @CPF VARCHAR(11),
    @Email VARCHAR(255),
    @AccessLevel INT,
    @PasswordHash VARCHAR(64),
    @PasswordSalt VARCHAR(64)   
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @UserId INT;
        INSERT INTO Users (Name, CPF, Email, AccessLevelId)
        VALUES (@Name, @CPF, @Email, @AccessLevel);

        SET @UserId = SCOPE_IDENTITY();

        INSERT INTO Passwords (UserId, PasswordHash, PasswordSalt)
        VALUES (@UserId, @PasswordHash, @PasswordSalt);

        COMMIT TRANSACTION;

        PRINT 'Usuário cadastrado com sucesso';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH;
END;