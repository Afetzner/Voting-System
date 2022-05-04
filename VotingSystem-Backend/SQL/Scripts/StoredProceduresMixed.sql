DELIMITER $$
DROP PROCEDURE IF EXISTS get_voter_participation $$

-- get all the voter serials who voted on an issue
CREATE PROCEDURE get_voter_participation(
	IN `v_issueSerial` varchar(9))
BEGIN
	SELECT user.serial_number, user.first_name, user.last_name, user.email FROM user 
    JOIN ballot on ballot.voter_serial_number = user.serial_number 
    WHERE ballot.issue_serial_number = `v_issueSerial`;
END
$$

DELIMITER ;