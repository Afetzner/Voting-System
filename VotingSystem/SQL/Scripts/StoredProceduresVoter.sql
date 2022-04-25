DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_voter $$
DROP PROCEDURE IF EXISTS afetzner.delete_voter $$
DROP PROCEDURE IF EXISTS afetzner.get_voter $$
DROP PROCEDURE IF EXISTS afetzner.check_voter_serial $$
DROP PROCEDURE IF EXISTS afetzner.check_user_username $$

-- Adds a voter, also adds them as a user
CREATE PROCEDURE afetzner.add_voter (
    IN `v_username` varchar(31),
    IN `v_password` varchar(31),
	IN `v_firstName` varchar(31),
	IN `v_lastName` varchar(31), 
    IN `v_serialNumber` varchar(9),
    OUT `v_collision` bool)
BEGIN
	START TRANSACTION;
	SET `v_collision` = EXISTS (SELECT 1 FROM user WHERE username = `v_username`);
    
	INSERT INTO user(username, password) 
    SELECT `v_username`, `v_password`
    WHERE NOT `v_collision`;
    
    INSERT INTO voter(first_name, last_name, serial_number, user_id) 
	SELECT `v_firstName`, `v_lastName`, `v_serialNumber`, LAST_INSERT_ID()
    WHERE NOT `v_collision`;
    COMMIT;
END 
$$

-- removes a voter (and user) and their votes
CREATE PROCEDURE afetzner.delete_voter (
	IN `v_serialNumber` varchar(9))
BEGIN
	START TRANSACTION;
	DELETE voter, user, ballot
    FROM voter 
    LEFT JOIN user ON voter.user_id = user.user_id
    LEFT JOIN ballot ON voter.voter_id = ballot.voter_id
    WHERE voter.serial_number = `v_serialNumber`;
    COMMIT;
END
$$

-- Gets a user's serial number from their login
CREATE PROCEDURE afetzner.get_voter (
	IN `v_username` varchar(31),
    IN `v_password` varchar(31),
    OUT `v_serialNumber` varchar(9),
    OUT `v_firstName` varchar(31),
    OUT `v_lastName` varchar(31))
BEGIN
	SELECT serial_number, first_name, last_name INTO `v_serialNumber`, `v_firstName`, `v_lastName`
    FROM voter 
    JOIN user ON voter.user_id = user.user_id
    WHERE user.username = `v_username` 
		AND user.password = `v_password`
	LIMIT 1;
END
$$

-- returns true if a serial number is assocaited with a voter
CREATE PROCEDURE afetzner.check_voter_serial (
	IN `v_serialNumber` varchar(9),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM voter WHERE serial_number = `v_serialNumber`);
END
$$

CREATE PROCEDURE afetzner.check_user_username (
	IN `v_username` varchar(31),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM user WHERE username = `v_username`);
END
$$
DELIMITER ;