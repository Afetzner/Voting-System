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
		IN varVoterId int,
        IN varElectionId int,
		OUT varBallotId int)
BEGIN
	INSERT INTO Ballot (VoterId, ElectionId)
	VALUES (varVoterId, varElectionId);
	SET varBallotId = LAST_INSERT_ID();
END 
//
    	
CREATE PROCEDURE afetzner.delete_ballot (
		IN varBallotId int)
BEGIN
	DELETE FROM Ballot
	JOIN CandidateElection
	ON Ballot.BallotId = CandidateElection.BallotId
	WHERE Ballot.BallotId = varBallotId;
END //

CREATE PROCEDURE aftetzner.get_ballot_id_from_info(
		IN varVoterId int,
		IN varElectionId int,
		OUT varBallotId int)
BEGIN
	SELECT BallotId INTO varBallotId FROM Ballot WHERE
	Ballot.VoterId = varVoterId AND
	Ballot.ElectionId = varElectionId;
END //

CREATE PROCEDURE afetzner.get_ballot_info_from_id(
		IN varBallotId int,
		OUT varVoterId int,
		OUT varElectionId int)

	BEGIN
		SELECT Ballot.VoterId, Ballot.ElectionId
		INTO varVoterId, varElectionId
		FROM Ballot
		WHERE Ballot.BallotId = varBallotId;
	END //
	
	DELIMITER ;
