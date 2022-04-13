-- Ignore errors, they're not errors. 
/* Voter table:
 	VoterId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	MiddleName varchar(32),
	LicenseNumber varchar(9) NOT NULL,
	PRIMARY KEY (VoterId)
*/


DELIMITER //
DROP PROCEDURE IF EXISTS afetzner.add_voter //
DROP PROCEDURE IF EXISTS afetzner.delete_voter //
DROP PROCEDURE IF EXISTS afetzner.get_voter_id_from_info //
DROP PROCEDURE IF EXISTS afetzner.get_voter_info_from_id //

CREATE PROCEDURE afetzner.add_voter (
	IN varLastName varchar(32), 
	IN varFirstName varchar(16),
	IN varMiddleName varchar(16), 
	IN varLicenseNumber varchar(9),
	OUT varVoterId int)
BEGIN
	INSERT INTO Voter(LastName, FirstName, MiddleName, LicenseNumber) 
	VALUES (varLastName, varFirstName, varMiddleName, varLicenseNumber);
	SET varVoterId = LAST_INSERT_ID();
END 
//
	
CREATE PROCEDURE afetzner.delete_voter (
		IN varVoterId int)
BEGIN
	DELETE FROM Voter WHERE Voter.VoterId = varVoterId;
END //

CREATE PROCEDURE afetzner.get_voter_id_from_info(
		IN varLastName varchar(64),
		IN varFirstName varchar(32),
		IN varMiddleName varchar(32),
		IN varLicenseNumber varchar(9),
        OUT varVoterId int)
BEGIN
    SELECT VoterId INTO varVoterId FROM Voter WHERE 
		Voter.LastName = varLastName AND
        Voter.FirstName = varFirstName AND
        Voter.MiddleName = varMiddleName AND
        Voter.LicenseNumber = varLicenseNumber
	LIMIT 1; 
END //
 
CREATE PROCEDURE afetzner.get_voter_info_from_id(
		IN varVoterId int,
        OUT varLastName varchar(64),
        OUT varFirstName varchar(32),
        OUT varMiddleName varchar(32),
        OUT varLicenseNumber varchar(9))
BEGIN
    SELECT lastName, firstName, middleName, licenseNumber 
    INTO varLastName, varFirstName, varMiddleName, varLicenseNumber FROM Voter 
    WHERE Voter.VoterId = varVoterId
    LIMIT 1;
END //
    
DELIMITER ;