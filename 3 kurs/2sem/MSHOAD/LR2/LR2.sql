CREATE INDEX idx_post_author
ON Post(author_id);

CREATE VIEW UserPosts AS
SELECT u.username, p.content_text, p.post_date
FROM [User] u
JOIN Post p ON u.user_id = p.author_id;

CREATE SEQUENCE message_seq
START WITH 1
INCREMENT BY 1;

CREATE TRIGGER trg_post_date
ON Post
AFTER INSERT
AS
BEGIN
    UPDATE Post
    SET post_date = GETDATE()
    WHERE post_id IN (SELECT post_id FROM inserted);
END;