DELIMITER $$
DROP PROCEDURE IF EXISTS get_voter_participation $$

-- get all the voter serials who voted on an issue
CREATE PROCEDURE get_voter_participation(
	IN `v_issueSerial` varchar(9))
BEGIN
	SELECT voter.serial_number, voter.firstName, voter.lastName, voter.email FROM voter 
    JOIN ballot on ballot.voter = voter.serial_number 
    WHERE ballot.issue = `v_issueSerial`;
END
$$

DELIMITER ;