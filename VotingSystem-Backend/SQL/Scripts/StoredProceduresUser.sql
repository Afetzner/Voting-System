DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_user $$
DROP PROCEDURE IF EXISTS afetzner.delete_user $$
DROP PROCEDURE IF EXISTS afetzner.get_user $$
DROP PROCEDURE IF EXISTS afetzner.check_user_serial $$
DROP PROCEDURE IF EXISTS afetzner.check_username $$

-- Adds a user
CREATE PROCEDURE afetzner.add_user (
    IN `v_username` varchar(31),
    IN `v_password` varchar(31),
    IN `v_email` varchar(63),
	IN `v_firstName` varchar(31),
	IN `v_lastName` varchar(31), 
    IN `v_serialNumber` varchar(9),
    IN `v_isAdmin` bool,
    OUT `v_collision` bool)
BEGIN
	SET `v_collision` = TRUE;
	START TRANSACTION;
    SET `v_collision` = EXISTS(
		SELECT 1 FROM user WHERE user.username = `v_username` OR user.email = `v_email` OR user.serial_number = `v_serialNumber`
    );
    
	INSERT INTO user(username, password, email, first_name, last_name, serial_number, is_admin) 
	SELECT `v_username`, `v_password`, `v_email`, `v_firstName`, `v_lastName`, `v_serialNumber`, `v_isAdmin`
    WHERE NOT `v_collision`;
    COMMIT;
END 
$$

-- removes a user and their votes
CREATE PROCEDURE afetzner.delete_user (
	IN `v_serialNumber` varchar(9))
BEGIN
	START TRANSACTION;
	DELETE user, ballot
    FROM user
    LEFT JOIN ballot ON user.user_id = ballot.voter_id
    WHERE user.serial_number = `v_serialNumber`;
    COMMIT;
END
$$

-- Gets a user's info from their login
CREATE PROCEDURE afetzner.get_user (
	IN `v_username` varchar(31),
    IN `v_password` varchar(31),
    
    OUT `v_email` varchar(63),
    OUT `v_firstName` varchar(31),
    OUT `v_lastName` varchar(31),
    OUT `v_serialNumber` varchar(9),
    OUT `v_isAdmin` bool,
    OUT `v_isNull` bool)
BEGIN
	SET `v_isNull` = NOT EXISTS (SELECT 1 FROM user WHERE username = `v_username` AND password = `v_password`);
	SELECT email, first_name, last_name, serial_number, is_admin INTO `v_email`, `v_firstName`, `v_lastName`, `v_serialNumber`, `v_isAdmin`
    FROM user 
    WHERE username = `v_username` AND password = `v_password`
	LIMIT 1;
END
$$

-- returns true if a serial number is assocaited with a voter
CREATE PROCEDURE afetzner.check_user_serial (
	IN `v_serialNumber` varchar(9),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM user WHERE user.serial_number = `v_serialNumber`);
END
$$

CREATE PROCEDURE afetzner.check_username (
	IN `v_username` varchar(31),
    OUT `v_occupied` bool)
BEGIN
	SET `v_occupied` = EXISTS (SELECT 1 FROM user WHERE user.username = `v_username`);
END
$$
DELIMITER ;