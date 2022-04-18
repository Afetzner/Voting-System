-- Ignore errors, they're not errors. 
use afetzner;

DROP TABLE IF EXISTS Voter;
DROP TABLE IF EXISTS Administrator;
DROP TABLE IF EXISTS BallotIssue;
DROP TABLE IF EXISTS BallotIssueOption;
DROP TABLE IF EXISTS VoterChoice;

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

CREATE TABLE BallotIssue (
	BallotIssueId int AUTO_INCREMENT,
	Title varchar(64) NOT NULL,
	Description varchar(255) NOT NULL,
	IssueNumber int AUTO_INCREMENT NOT NULL,
	PRIMARY KEY (BallotIssueId)
);

CREATE TABLE BallotIssueOption (
	BallotIssueOptionId int AUTO_INCREMENT,
	BallotIssueId int,
	OptionNumber int,
	OptionDescription varchar(255) NOT NULL,
	PRIMARY KEY (BallotIssueOptionId),
	FOREIGN KEY (BallotIssueId) REFERENCES BallotIssue(BallotIssueId)
);

CREATE TABLE VoterChoice (
	VoterChoiceId int AUTO_INCREMENT,
	SerialNumber int,
	VoterId int,
	BallotIssueId int,
	BallotIssueOptionId int,
	PRIMARY KEY (VoterChoiceId),
	FOREIGN KEY (VoterId) REFERENCES Voter(VoterId),
	FOREIGN KEY (BallotIssueId) REFERENCES BallotIssue(BallotIssueId),
	FOREIGN KEY (BallotIssueOptionId) REFERENCES BallotIssueOption(BallotIssueOptionId)
);
