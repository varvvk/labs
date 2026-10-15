-- User
CREATE TABLE [User] (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100),
    email NVARCHAR(100),
    password_hash NVARCHAR(255),
    date_of_birth DATETIME,
    registration_date DATETIME
);

-- FriendRequest
CREATE TABLE FriendRequest (
    request_id INT IDENTITY(1,1) PRIMARY KEY,
    sender_id INT,
    receiver_id INT,
    status NVARCHAR(50),
    request_date DATETIME,
    FOREIGN KEY (sender_id) REFERENCES [User](user_id),
    FOREIGN KEY (receiver_id) REFERENCES [User](user_id)
);

-- Post
CREATE TABLE Post (
    post_id INT IDENTITY(1,1) PRIMARY KEY,
    author_id INT,
    content_text NVARCHAR(MAX),
    post_date DATETIME,
    privacy_setting NVARCHAR(50),
    FOREIGN KEY (author_id) REFERENCES [User](user_id)
);

-- Comment
CREATE TABLE Comment (
    comment_id INT IDENTITY(1,1) PRIMARY KEY,
    post_id INT,
    author_id INT,
    comment_text NVARCHAR(MAX),
    comment_date DATETIME,
    FOREIGN KEY (post_id) REFERENCES Post(post_id),
    FOREIGN KEY (author_id) REFERENCES [User](user_id)
);

-- Message
CREATE TABLE Message (
    message_id INT IDENTITY(1,1) PRIMARY KEY,
    sender_id INT,
    receiver_id INT,
    message_text NVARCHAR(MAX),
    sent_date DATETIME,
    read_status NVARCHAR(50),
    FOREIGN KEY (sender_id) REFERENCES [User](user_id),
    FOREIGN KEY (receiver_id) REFERENCES [User](user_id)
);

-- Group
CREATE TABLE [Group] (
    group_id INT IDENTITY(1,1) PRIMARY KEY,
    creator_id INT,
    group_name NVARCHAR(100),
    creation_date DATETIME,
    privacy_setting NVARCHAR(50),
    FOREIGN KEY (creator_id) REFERENCES [User](user_id)
);

-- GroupMember
CREATE TABLE GroupMember (
    group_member_id INT IDENTITY(1,1) PRIMARY KEY,
    group_id INT,
    user_id INT,
    join_date DATETIME,
    role NVARCHAR(50),
    FOREIGN KEY (group_id) REFERENCES [Group](group_id),
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);