/*Ballot table:
	BallotId int AUTO_INCREMENT,
	VoterId int,
	ElectionId int,
	PRIMARY KEY (BallotId),
	FOREIGN KEY (VoterId) REFERENCES Voter(VoterId),
	FOREIGN KEY (ElectionId) REFERENCES Election(ElectionId)
*/

DELIMITER //
DROP PROCEDURE IF EXISTS afetzner.add_ballot //
DROP PROCEDURE IF EXISTS afetzner.get_ballot_info_from_voter_license //
//

CREATE PROCEDURE afetzner.add_ballot(
		IN varVoterLicenseNumber varchar(9),
        IN varElectionId int,
		OUT varBallotId int)
BEGIN
	INSERT INTO Ballot (VoterId, ElectionId)
	VALUES ((SELECT VoterId FROM Voter WHERE Voter.LicenseNumber = varLicenseNumber),
			(SELECT ElectionId FROM Election WHERE Election.ElectionId = varElectionId);
	SET varBallotId = LAST_INSERT_ID();
END //
    
	/* query for voter to view their recorded ballot from a previous election
	 will need to edit these to make them accessible by voter license or
	 write separate queries to retrieve voterid via license */
CREATE PROCEDURE afetzner.get_ballot_info_from_voter_id (
		IN varVoterId int,
		IN varElectionId int,
		OUT varCandidateLastName varchar(64),
		OUT varCandidateFirstName varchar(32),
		OUT varVote int)

	BEGIN
		SELECT Candidate.FirstName, Candidate.LastName, CandidateElection.Vote
		INTO varCandidateLastName, varCandidateFirstName, varVote 
		FROM Ballot
		JOIN CandidateElection
		ON Ballot.BallotId = CandidateElection.BallotId
		JOIN Candidate
		ON CandidateElection.CandidateId = Candidate.CandidateId
		WHERE Ballot.VoterId = varVoterId
		AND Ballot.ElectionId = varElectionId
		GROUP BY Ballot.ElectionId;
	END //
	
	DELIMITER ;
