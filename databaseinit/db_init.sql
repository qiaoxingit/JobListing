CREATE TABLE USER (
    ID BINARY(16) PRIMARY KEY,
    EMAIL VARCHAR(255) NOT NULL UNIQUE ,
    FIRST_NAME VARCHAR(255) NOT NULL,
    LAST_NAME VARCHAR(255) NOT NULL,
    USERNAME VARCHAR(255) NOT NULL,
    PASSWORD VARCHAR(255) NOT NULL,
    ROLE INT NOT NULL
);

INSERT INTO USER (ID, EMAIL, FIRST_NAME, LAST_NAME, USERNAME, PASSWORD, ROLE) VALUES
    (UNHEX(REPLACE('550e8400-e29b-41d4-a716-446655440000', '-', '')), 'john.doe@example.com', 'John', 'Doe', 'johndoe', '1dmQjwKwx1B/qZQnLDPNhBceKsW1Wqa4w5uvM5rdCdM=', 0), #123
    (UNHEX(REPLACE('5b73bebe-eb4a-428f-8f21-94ce51e07b6e', '-', '')), 'jane.smith@example.com', 'Jane', 'Smith', 'janesmith', '0gz/IkuVCail6EuVuIsyQN+XFQmcT8H7pBbFYHXi3PE=', 0), #aab
    (UNHEX(REPLACE('62d168e8-d023-429c-885d-e1841e374140', '-', '')), 'admin@example.com', 'Admin', 'User', 'adminuser', 'tbEV/6XvTcYymxwJaHOfmoL9gnwG0CUc/xlNJa/046M=', 1), #789
    (UNHEX(REPLACE('9f042e5d-c566-48a5-935d-275dedad2bfc', '-', '')), 'emily.jones@example.com', 'Emily', 'Jones', 'emilyj', 'u9Iqd4rG0zw8WbByptLiGMz1HUcEoiEt/AmCia4qWgs=', 1), #def
    (UNHEX(REPLACE('93e27c43-db96-4e1f-8ccd-329e6340e939', '-', '')), 'michael.brown@example.com', 'Michael', 'Brown', 'mikeb', 'QFlTjJYHRoLsAEUo9wBpQfgj+MFhy9VIBvJ7eHmsoTo=', 0); #lmn

CREATE TABLE JOB (
    ID BINARY(16) PRIMARY KEY,
    TITLE VARCHAR(255) NOT NULL,
    DESCRIPTION TEXT NOT NULL,
    DATE_POSTED DATETIME NOT NULL,
    DATE_EXPIRED DATETIME NOT NULL,
    POSTED_BY_USER BINARY(16) NOT NULL,
    FOREIGN KEY (POSTED_BY_USER) REFERENCES USER(ID)
);

INSERT INTO JOB (ID, TITLE, DESCRIPTION, DATE_POSTED, DATE_EXPIRED, POSTED_BY_USER) VALUES
    (UNHEX(REPLACE('d93a92cb-75cd-4c06-ab86-881cd7c1e050', '-', '')), 'Software Engineer', 'Develop scalable web applications using Java and Spring Boot.', NOW(), DATE_ADD(NOW(), INTERVAL 2 MONTH), UNHEX(REPLACE('62d168e8-d023-429c-885d-e1841e374140', '-', ''))),
    (UNHEX(REPLACE('7e1ecd56-fe4d-4618-a9d0-5f2d089eb20a', '-', '')), 'Data Analyst', 'Analyze business data and create dashboards using Power BI and SQL.', NOW(), DATE_ADD(NOW(), INTERVAL 2 MONTH), UNHEX(REPLACE('62d168e8-d023-429c-885d-e1841e374140', '-', ''))),
    (UNHEX(REPLACE('fe59cf84-aa11-42b5-94cf-f9cd766016d4', '-', '')), 'Project Manager', 'Manage project timelines and coordinate teams.', NOW(), DATE_ADD(NOW(), INTERVAL 2 MONTH), UNHEX(REPLACE('9f042e5d-c566-48a5-935d-275dedad2bfc', '-', ''))),
    (UNHEX(REPLACE('5d408971-43b4-45be-ac9e-e03f36910b9e', '-', '')), 'Graphic Designer', 'Design marketing materials and social media content.', NOW(), DATE_ADD(NOW(), INTERVAL 2 MONTH), UNHEX(REPLACE('9f042e5d-c566-48a5-935d-275dedad2bfc', '-', ''))),
    (UNHEX(REPLACE('7e30694e-0c39-4998-92b1-4c88fc60a889', '-', '')), 'Technical Writer', 'Create documentation for APIs and software tools.', NOW(), DATE_ADD(NOW(), INTERVAL 2 MONTH), UNHEX(REPLACE('9f042e5d-c566-48a5-935d-275dedad2bfc', '-', '')));


CREATE TABLE INTERESTEDJOB (
    ID BINARY(16) PRIMARY KEY,
    USER_ID BINARY(16) NOT NULL,
    JOB_ID BINARY(16) NOT NULL,
    INDEX IDX_USER_ID (USER_ID),
    INDEX IDX_JOB_ID (JOB_ID),
    FOREIGN KEY (USER_ID) REFERENCES USER(ID),
    FOREIGN KEY (JOB_ID) REFERENCES JOB(ID)
);

INSERT INTO INTERESTEDJOB (ID, USER_ID, JOB_ID) VALUES
    (UNHEX(REPLACE('c5405071-ffb6-4b3e-ac3c-bd05e656a1da', '-', '')),UNHEX(REPLACE('550e8400-e29b-41d4-a716-446655440000', '-', '')), UNHEX(REPLACE('7e1ecd56-fe4d-4618-a9d0-5f2d089eb20a', '-', ''))),
    (UNHEX(REPLACE('dd073246-e6cf-4dbc-95d8-526d308e1669', '-', '')),UNHEX(REPLACE('550e8400-e29b-41d4-a716-446655440000', '-', '')), UNHEX(REPLACE('fe59cf84-aa11-42b5-94cf-f9cd766016d4', '-', ''))),
    (UNHEX(REPLACE('28492225-4db1-4cae-b8e9-0f04e01696e3', '-', '')),UNHEX(REPLACE('5b73bebe-eb4a-428f-8f21-94ce51e07b6e', '-', '')), UNHEX(REPLACE('5d408971-43b4-45be-ac9e-e03f36910b9e', '-', ''))),
    (UNHEX(REPLACE('78562a76-cdf4-4589-aafb-c4958b57a15c', '-', '')),UNHEX(REPLACE('5b73bebe-eb4a-428f-8f21-94ce51e07b6e', '-', '')), UNHEX(REPLACE('d93a92cb-75cd-4c06-ab86-881cd7c1e050', '-', ''))),
    (UNHEX(REPLACE('857310d9-f03b-4598-8e80-e5aea527563e', '-', '')),UNHEX(REPLACE('93e27c43-db96-4e1f-8ccd-329e6340e939', '-', '')), UNHEX(REPLACE('5d408971-43b4-45be-ac9e-e03f36910b9e', '-', ''))),
    (UNHEX(REPLACE('c170acea-de32-4fd8-85ad-3bad901e754d', '-', '')),UNHEX(REPLACE('93e27c43-db96-4e1f-8ccd-329e6340e939', '-', '')), UNHEX(REPLACE('7e30694e-0c39-4998-92b1-4c88fc60a889', '-', '')));
