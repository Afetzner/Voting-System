DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_ballot $$
DROP PROCEDURE IF EXISTS afetzner.delete_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_voters_ballot $$
DROP PROCEDURE IF EXISTS afetzner.get_did_voter_participate $$
DROP PROCEDURE IF EXISTS afetzner.get_election_results $$
$$

CREATE PROCEDURE afetzner.add_ballot (
	IN `ballotSerial` varchar(9),  
    IN `voterSerial` varchar(9),
    IN `issueSerial` varchar(9),
    IN `choiceNumber` int)
BEGIN
    INSERT INTO ballot 
		(ballot_serial_number, 
		voter_serial_number,
		issue_serial_number,
		choice_number,
		voter_id,
		issue_id,
		choice_id)
	VALUES 
		(`ballotSerial`,
        `voterSerial`,
        `issueSerial`,
        `choiceNumber`,
		(SELECT voter_id FROM voter WHERE voter.serial_number = `voterSerial` LIMIT 1),
        (SELECT issue_id FROM issue WHERE issue.issue_serial = `issueSerial` LIMIT 1),
        (SELECT choice_id FROM issue_choice WHERE issue_choice.issue_serial = `issueSerial` LIMIT 1));
END
$$

CREATE PROCEDURE afetzner.delete_ballot (
	IN `ballotSerial` varchar(9))
BEGIN
	DELETE FROM ballot 
    WHERE ballot.ballot_serial_number = `ballotSerial`;
END
$$

CREATE PROCEDURE afetzner.get_voters_ballot (
	IN `voterSerial` varchar(9),
    IN `issueSerial` varchar(9),
    OUT `ballotSerial` varchar(9),
    OUT `choiceNumber` int,
    OUT `choiceTitle` varchar(127))
BEGIN
	SELECT ballot_serial_number, choice_number, choice_title
    INTO `ballotSerial`, `choiceNumber`, `choiceTitle`
    FROM issue 
		RIGHT JOIN issue_choice ON issue.issue_id = issue_choice.issue_id
		RIGHT JOIN ballot ON ballot.issue_id = issue.issue_id
	WHERE ballot.voter_serial = `voterSerial`;
END
$$
    
CREATE PROCEDURE afetzner.get_did_voter_participate (
	IN `voterSerial` varchar(9),
    IN `issueSerial` varchar(9),
    OUT `didVote` bool)
BEGIN
	SELECT count(ballot_id) > 0 
    INTO `didVote`
    FROM ballot 
    WHERE ballot.voter_serial = `voterSerial`
		AND ballot.issue_serial = `issueSerial`;
END
$$

CREATE PROCEDURE afetzner.get_election_results (
	IN `issueSerial` varchar(9))
BEGIN
	SELECT count(ballot_id) AS `count`
    FROM ballot
    WHERE ballot.issue_serial = `issueSerial`
    GROUP BY ballot.choice_id;
END
$$

DELIMITER ;