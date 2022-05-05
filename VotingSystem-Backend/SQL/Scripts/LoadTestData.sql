use afetzner;

-- Create admins
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('afetz00', 'Drowsap1', 'alexEmail@gmail.com', 'Alex', 'Fetzner', 'A99900012', true);
SET @adminAlexId = last_insert_id();
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('3akycka', 'pa55W0RD@', 'ana@gmail.com', 'Anastasiya', 'Krestovsky', 'A99900013', true);
SET @adminAnastasyiaId = last_insert_id();

-- Create voters
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('jdoe16', '2bOr!2b', 'jackDoe@gmail.com', 'Jack', 'Doe', 'V12399874', false);
SET @voterJackId = last_insert_id();
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('johndoe99', 'Cook!33ater', 'johndoe66@gmail.com', 'John', 'Doe', 'V78955412', false);
SET @voterJohnId = last_insert_id();
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('ultimateRick', '123asd99A$', 'ricksEmail@gmail.com', 'Richard', 'Brown', 'V55544463', false);
SET @voterRickId = last_insert_id();
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('Bullfighter', '#grrrrrHaha7', 'bullSara@gmail.com', 'Sarah', 'Bull', 'V12300077', false);
SET @voterSarahId = last_insert_id();
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('drichard', '#th!sIsMyPa55W0rd', 'dickRick@gmail.com', 'Dick', 'Richard', 'V80000112', false);
SET @voterDickId = last_insert_id();
INSERT INTO user (username, password, email, first_name, last_name, serial_number, is_admin)
    VALUES ('Periwinkle', 'cyanRed$brown6', 'secreteAgent@gmail.com', 'Perry', 'Platipus', 'V11122233', false);
SET @voterPerryId = last_insert_id();

-- Create ballot issues
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955500', "2020-04-19", "2022-04-21", "Lincoln City Mayor", "The mayor of the city of Lincoln. Responsibilties include vetoing or passing bills passed by city council");
SET @issueMayorId = last_insert_id();
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955501', "2020-04-19", "2023-05-19", "Lincoln City Police Chief", 
		"The mayor of the city of Lincoln. Responsibilties include managing the Lincoln city police department and community outreach program");
SET @issuePoliceId = last_insert_id();
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955502', "2020-04-19", "2023-06-19", "Corroner of the city of Lincoln", 
		"The corroner of the city of Lincoln. Offical corroner for assisting investigations by the city");
SET @issueCorronerId = last_insert_id();
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955503', "2020-04-19", "2021-06-19", "Refereandum", "Should we build an elementary school?");
SET @issueElementaryId = last_insert_id();


-- Create ballot issue options
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (0, "Dick Anderson (encumbant)", @issueMayorId, 'I78955500');
SET @mayorOptionZeroId = last_insert_id();
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (1, "Emmy Roh", @issueMayorId, 'I78955500');
SET @mayorOptionOneId = last_insert_id();
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (2, "Sammy Hunt", @issueMayorId, 'I78955500');
SET @mayorOptionTwoId = last_insert_id();

INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (0, "Teresa Ewins (encumbant)", @issuePoliceId, 'I78955501');
SET @policeOptionZeroId = last_insert_id();
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (1, "Andrew Roman", @issuePoliceId, 'I78955501');
SET @policeOptionOneId = last_insert_id();

INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (0, "Andders Mort (encumbant)", @issueCorronerId, 'I78955502');
SET @corronerOptionZeroId = last_insert_id();

INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (0, "Yes", @issueElementaryId, 'I78955503');
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (1, "No", @issueElementaryId, 'I78955503');


-- Create ballots
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
    choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240001', 'V12399874', 'I78955500', 0,
    @voterJackId, @issueMayorId, @mayorOptionZeroId);
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
	choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240002', 'V12399874', 'I78955501', 1,
    @voterJackId, @issuePoliceId, @policeOptionOneId);
    
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
    choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240003', 'V55544463', 'I78955500', 1,
    @voterRickId, @issueMayorId, @mayorOptionOneId);
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
	choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240004', 'V55544463', 'I78955501', 1,
    @voterRickId, @issuePoliceId, @policeOptionOneId);
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
	choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240005', 'V55544463', 'I78955502', 0,
    @voterRickId, @issueCorronerId, @corronerOptionZeroId);
    
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
    choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240006', 'V12300077', 'I78955500', 0,
    @voterSarahId, @issueMayorId, @mayorOptionZeroId);
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
	choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240007', 'V12300077', 'I78955502', 0,
    @voterSarahId, @issueCorronerId, @corronerOptionZeroId);
    
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
    choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240008', 'V11122233', 'I78955500', 0,
    @voterPerryId, @issueMayorId, @mayorOptionZeroId);
