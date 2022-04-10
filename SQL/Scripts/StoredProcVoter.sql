DROP PROCEDURE IF EXISTS afetzner.add_voter;
DROP PROCEDURE IF EXISTS afetzner.remove_voter;
DROP PROCEDURE IF EXISTS afetzner.select_voter_from_id;
DROP PROCEDURE IF EXISTS afetzner.select_voter_from_info;

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
    
CREATE PROCEDURE afetzner.select_voter_from_id(
		IN varVoterId int)
    SELECT * FROM Voter WHERE Voter.VoterId = varVoterId;
    
CREATE PROCEDURE afetzner.select_voter_from_info(
		IN varLastName varchar(64),
		IN varFirstName varchar(32),
		IN varMiddleName varchar(32),
		IN varLicenseNumber varchar(9))
    SELECT VoterId FROM Voter WHERE 
		Voter.LastName = varLastName AND
        Voter.FirstName = varFirstName AND
        Voter.MiddleName = varMiddleName AND
        Voter.LicenseNumber = varLicenseNumber;