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

DROP PROCEDURE IF EXISTS afetzner.add_voter;
DROP PROCEDURE IF EXISTS afetzner.remove_voter;
DROP PROCEDURE IF EXISTS afetzner.get_voter_info_from_id;
DROP PROCEDURE IF EXISTS afetzner.get_voter_id_from_info;

CREATE PROCEDURE afetzner.add_voter (
		IN varLastName varchar(32), 
		IN varFirstName varchar(16),
		IN varMiddleName varchar(16), 
		IN varLicenseNumber varchar(9))
	INSERT INTO Voter(LastName, FirstName, MiddleName, LicenseNumber) 
    VALUES (varLastName, varFirstName, varMiddleName, varLicenseNumber);

CREATE PROCEDURE afetzner.remove_voter (
		IN varVoterId int)
	DELETE FROM VOTER WHERE VOTER.VoterId = varVoterId;
    
CREATE PROCEDURE afetzner.get_voter_info_from_id(
		IN varVoterId int)
    SELECT lastName, firstName, middleName, licenseNumber FROM Voter 
    WHERE VoterId = varVoterId;
    
CREATE PROCEDURE afetzner.get_voter_id_from_info(
		IN varLastName varchar(64),
		IN varFirstName varchar(32),
		IN varMiddleName varchar(32),
		IN varLicenseNumber varchar(9),
        OUT varVoterId int)
    SELECT VoterId INTO varVoterId FROM Voter WHERE 
		Voter.LastName = varLastName AND
        Voter.FirstName = varFirstName AND
        Voter.MiddleName = varMiddleName AND
        Voter.LicenseNumber = varLicenseNumber;
        