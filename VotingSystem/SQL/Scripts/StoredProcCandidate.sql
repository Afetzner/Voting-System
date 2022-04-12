-- Ignore errors, they're not errors
/*
Candidate table:
	CandidateId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	PRIMARY KEY (CandidateId)
*/

CREATE PROCEDURE afetzner.add_candidate(
		IN varLastName varchar(64),
        IN varFirstName varchar(32))
	INSERT INTO Candidate(LastName, FirstName)
	VALUES (varLastName, varFirstName);
    
CREATE PROCEDURE afetzner.remove_candidate(
		IN varCandidateId int)
	DELETE FROM Candidate WHERE Candidate.candidateId = varCandidateId;
	
CREATE PROCEDURE afetzner.get_candidate_id_from_info (
		IN varLastName varchar(64),
        IN varFirstName varchar(32),
        OUT varCandidateId int)
	SELECT CadidateId INTO varCandidateId FROM Candidate
	WHERE LastName = varLastName AND
		FirstName = varFirstName;
        
CREATE PROCEDURE afetzner.get_candidate_info_from_id (
		IN varCandidateId int)
	SELECT LastName, FirstName FROM Candidate
	WHERE CandidateId = varCandidateId;