use afetzner;

-- Create admins
INSERT INTO user (username, password) VALUES ('afetz00', 'Drowsap1!');
INSERT INTO admin (user_id, serial_number) VALUES (last_insert_id(), 'A99900012');
SET @adminAlexId = last_insert_id();
INSERT INTO user (username, password) VALUES ('3akycka', 'pa55W0RD@');
INSERT INTO admin (user_id, serial_number) VALUES (last_insert_id(), 'A99900013');
SET @adminAnastasyiaId = last_insert_id();

-- Create voters
INSERT INTO user (username, password) VALUES ('jdoe16', '2bOr!2b');
INSERT INTO voter (user_id, first_name, last_name, serial_number) VALUES (last_insert_id(), 'Jane', 'Doe', 'V12399874');
SET @voterJaneId = last_insert_id();
INSERT INTO user (username, password) VALUES ('johndoe99', 'Cook!33ater');
INSERT INTO voter (user_id, first_name, last_name, serial_number) VALUES (last_insert_id(), 'John', 'Doe', 'V78955412');
SET @voterJohnId = last_insert_id();
INSERT INTO user (username, password) VALUES ('ultimateRick', '123asd99A$');
INSERT INTO voter (user_id, first_name, last_name, serial_number) VALUES (last_insert_id(), 'Richard', 'Brown', 'V55544463');
SET @voterRickId = last_insert_id();
INSERT INTO user (username, password) VALUES ('Bullfighter', '#grrrrrHaha7');
INSERT INTO voter (user_id, first_name, last_name, serial_number) VALUES (last_insert_id(), 'Sarah', 'Bull', 'V12300077');
SET @voterSarahId = last_insert_id();
INSERT INTO user (username, password) VALUES ('drichard', 'th!sIsMyPa55W0rd');
INSERT INTO voter (user_id, first_name, last_name, serial_number) VALUES (last_insert_id(), 'Dick', 'Richard', 'V80000112');
SET @voterDickId = last_insert_id();
INSERT INTO user (username, password) VALUES ('Periwinkle', 'cyanRed$brown6');
INSERT INTO voter (user_id, first_name, last_name, serial_number) VALUES (last_insert_id(), 'Perry', 'Platipus', 'V11122233');
SET @voterPerryId = last_insert_id();

-- Create ballot issues
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955500', "2020-04-19", "2020-05-19", "Lincoln City Mayor", "The mayor of the city of Lincoln. Responsibilties include vetoing or passing bills passed by city council");
SET @issueMayorId = last_insert_id();
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955501', "2020-04-19", "2020-05-19", "Lincoln City Police Chief", 
		"The mayor of the city of Lincoln. Responsibilties include managing the Lincoln city police department and community outreach program");
SET @issuePoliceId = last_insert_id();
INSERT INTO issue (serial_number, start_date, end_date, title, description)
	VALUES ('I78955502', "2020-04-19", "2020-06-19", "Corroner of the city of Lincoln", 
		"The corroner of the city of Lincoln. Offical corroner for assisting investigations by the city");
SET @issueCorronerId = last_insert_id();

-- Create ballot issue options
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (0, "Dick Anderson (encumbant)", @issueMayorId, 'I78955500');
SET @mayorOptionZeroId = last_insert_id();
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (1, "Emmy Roh", @issueMayorId, 'I78955500');
SET @mayorOptionOneId = last_insert_id();
INSERT INTO issue_option (option_number, title, issue_id, issue_serial)
VALUES (2, "Lord Farquad", @issueMayorId, 'I78955500');
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

-- Create ballots
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
    choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240001', 'V12399874', 'I78955500', 0,
    @voterJaneId, @issueMayorId, @mayorOptionZeroId);
INSERT INTO ballot (ballot_serial_number, voter_serial_number, issue_serial_number, 
	choice_number, voter_id, issue_id, choice_id)
VALUES ('B11240002', 'V12399874', 'I78955501', 1,
    @voterJaneId, @issuePoliceId, @policeOptionOneId);
    
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
