-- Ignore errors, they're not errors
/* Candidate table:
	CandidateId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	PRIMARY KEY (CandidateId)
*/

DELIMITER //
DROP PROCEDURE IF EXISTS afetzner.add_candidate //
DROP PROCEDURE IF EXISTS afetzner.delete_candidate //
DROP PROCEDURE IF EXISTS afetzner.get_candidate_id_from_info //
DROP PROCEDURE IF EXISTS afetzner.get_candidate_info_from_id //
//

CREATE PROCEDURE afetzner.add_candidate(
		IN varLastName varchar(64),
        IN varFirstName varchar(32),
		OUT varCandidateId int)
BEGIN
	INSERT INTO Candidate(LastName, FirstName)
	VALUES (varLastName, varFirstName);
	SET varCandidateId = LAST_INSERT_ID();
END //
    
CREATE PROCEDURE afetzner.delete_candidate(
		IN varCandidateId int)
BEGIN
	DELETE FROM Candidate WHERE Candidate.candidateId = varCandidateId;
END //

CREATE PROCEDURE afetzner.get_candidate_id_from_info (
		IN varLastName varchar(64),
        IN varFirstName varchar(32),
        OUT varCandidateId int)
BEGIN
	SELECT CadidateId INTO varCandidateId FROM Candidate
	WHERE Candidate.LastName = varLastName AND
		Candidate.FirstName = varFirstName
	LIMIT 1;
END //

CREATE PROCEDURE afetzner.get_candidate_info_from_id (
		IN varCandidateId int,
		OUT varLastName varchar(32),
		OUT varFirstName varchar(16))
BEGIN
	SELECT lastName, firstName 
	INTO varLastName, varFirstName FROM Candidate
	WHERE Candidate.CandidateId = varCandidateId
	LIMIT 1;
END //
DELIMITER ;