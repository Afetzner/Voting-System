/* Voter table:
 *	 voter_id int auto_increment,
 *   user_id int,
 *   serial_number varchar(9) NOT NULL UNIQUE,
 *   first_name varchar(31) NOT NULL,
 *   last_name varchar(31) NOT NULL,
 *   PRIMARY KEY (voter_id),
 *   FOREIGN KEY (user_id) REFERENCES tab_user(user_id)
 *
 * User table:
 *   user_id int auto_increment,
 *   username varchar(31) NOT NULL,
 *   password varchar(31) NOT NULL,
 *   PRIMARY KEY (user_id)
*/

DELIMITER $$
DROP PROCEDURE IF EXISTS afetzner.add_voter $$
DROP PROCEDURE IF EXISTS afetzner.delete_voter $$
DROP PROCEDURE IF EXISTS afetzner.get_voter_serial $$
DROP PROCEDURE IF EXISTS afetzner.check_voter_serial $$

-- Adds a voter, also adds them as a user
CREATE PROCEDURE afetzner.add_voter (
    IN `username` varchar(31),
    IN `password` varchar(31),
	IN `firstName` varchar(31),
	IN `lastName` varchar(31), 
    IN `serialNumber` varchar(9))
BEGIN
	INSERT INTO User(username, password) VALUES (`username`, `password`);
    INSERT INTO Voter(first_name, last_name, serial_number, user_id) 
	VALUES (`firstName`, `lastName`, `serialNumber`, LAST_INSERT_ID());
END 
$$

CREATE PROCEDURE afetzner.delete_voter (
	IN `serialNumber` varchar(9))
BEGIN
	DELETE voter, ballot
    FROM voter JOIN ballot ON voter.voter_id = ballot.voter
    WHERE voter.serial_number = `serialNumber`;
END
$$

CREATE PROCEDURE afetzner.get_voter_serial (
	IN `username` varchar(31),
    IN `password` varchar(31),
    IN `firstName` varchar(31),
    IN `lastName` varchar(31),
    OUT `serialNumber` varchar(9))
BEGIN
	SELECT serial_number INTO `serialNumber` 
    FROM voter
    WHERE username = `username` 
		AND password = `password`
        AND first_name = `firstName`
        AND last_name = `lastName`
	LIMIT 1;
END
$$

-- returns true if a serial number is assocaited with a voter
CREATE PROCEDURE afetzner.check_voter_serial (
	IN `serialNumber` varchar(9),
    OUT `occupied` bool)
BEGIN
	SELECT count(serialNumber) > 0 INTO `occupied`
    WHERE voter.serial_number = `serialNumber`;
END
$$
DELIMITER ;