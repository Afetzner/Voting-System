-- Ignore errors, they're not errors. 
/* 
Voter table:
 	VoterId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	MiddleName varchar(32),
	LicenseNumber varchar(9) NOT NULL,
	PRIMARY KEY (VoterId)
*/
DELIMITER //
DROP PROCEDURE IF EXISTS afetzner.add_voter;
DROP PROCEDURE IF EXISTS afetzner.delete_voter;
DROP PROCEDURE IF EXISTS afetzner.get_voter_info_from_id;
DROP PROCEDURE IF EXISTS afetzner.get_voter_id_from_info;
//

CREATE PROCEDURE afetzner.add_voter (
	IN LastName varchar(32), 
	IN FirstName varchar(16),
	IN MiddleName varchar(16), 
	IN LicenseNumber varchar(9),
	OUT VoterId int)
BEGIN
	INSERT INTO Voter(LastName, FirstName, MiddleName, LicenseNumber) 
	VALUES (LastName, FirstName, MiddleName, LicenseNumber);
	SET VoterId = LAST_INSERT_ID();
END 
//
	

CREATE PROCEDURE afetzner.delete_voter (
		IN VoterId int)
	DELETE FROM VOTER WHERE VOTER.VoterId = VoterId;
//
    
CREATE PROCEDURE afetzner.get_voter_info_from_id(
		IN VoterId int,
        OUT LastName varchar(64),
        OUT FirstName varchar(32),
        OUT MiddleName varchar(32),
        OUT LicenseNumber varchar(9))
    SELECT lastName, firstName, middleName, licenseNumber 
    INTO LastName, FirstName, MiddleName, LicenseNumber FROM Voter 
    WHERE VoterId = VoterId
    LIMIT 1;
//
    
CREATE PROCEDURE afetzner.get_voter_id_from_info(
		IN LastName varchar(64),
		IN FirstName varchar(32),
		IN MiddleName varchar(32),
		IN LicenseNumber varchar(9),
        OUT VoterId int)
BEGIN
    SELECT VoterId INTO VoterId FROM Voter WHERE 
		Voter.LastName = LastName AND
        Voter.FirstName = FirstName AND
        Voter.MiddleName = MiddleName AND
        Voter.LicenseNumber = LicenseNumber
	LIMIT 1; 
END //
        