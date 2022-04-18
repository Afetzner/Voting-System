-- Ignore errors, they're not errors. 
use afetzner;

DROP TABLE IF EXISTS Ballot;
DROP TABLE IF EXISTS CandidateElection;
DROP TABLE IF EXISTS Candidate;
DROP TABLE IF EXISTS Election;
DROP TABLE IF EXISTS Administrator;
DROP TABLE IF EXISTS Voter;

CREATE TABLE Voter (
	VoterId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	MiddleName varchar(32),
	LicenseNumber varchar(9) NOT NULL,
	PRIMARY KEY (VoterId)
);

CREATE TABLE Administrator(
	AdminId int AUTO_INCREMENT,
	UserName varchar(32) NOT NULL,
	Password varchar(32) NOT NULL,
	PRIMARY KEY (adminId)
);

CREATE TABLE Election (
	ElectionId int AUTO_INCREMENT,
	State varchar(2) NOT NULL,
	District int NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL,
	PRIMARY KEY (ElectionId)
);

CREATE TABLE Candidate (
	CandidateId int AUTO_INCREMENT,
	LastName varchar(64) NOT NULL,
	FirstName varchar(32) NOT NULL,
	PRIMARY KEY (CandidateId)
);

/* This table represents a vote on a Ballot by a Voter on an Issue (Candidate) in an Election*/
CREATE TABLE CandidateElection (
	CandidateElectionId int AUTO_INCREMENT,
	CandidateId int,
	ElectionId int,
	BallotId int,
	Vote int, /* make each vote a boolean value 0/1 or leave it open to multiple choice*/
	PRIMARY KEY (CandidateElectionId),
	FOREIGN KEY (CandidateId) REFERENCES Candidate(CandidateId),
	FOREIGN KEY (ElectionId) REFERENCES Election(ElectionId),
	FOREIGN KEY (BallotId) REFERENCES Ballot(BallotId)
);
/* This table represents a Ballot that can have multiple votes (CandidateElections) 
	on various issues (Candidates)*/
CREATE TABLE Ballot (
	BallotId int AUTO_INCREMENT,
	VoterId int,
	ElectionId int,
	PRIMARY KEY (BallotId),
	FOREIGN KEY (VoterId) REFERENCES Voter(VoterId),
	FOREIGN KEY (ElectionId) REFERENCES Election(ElectionId)
);
