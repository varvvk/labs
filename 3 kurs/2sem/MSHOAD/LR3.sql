ALTER TABLE GroupMember
ADD hierarchy_node hierarchyid;

CREATE PROCEDURE GetDescendants
    @node hierarchyid
AS
BEGIN
    SELECT 
        group_member_id,
        user_id,
        hierarchy_node.ToString() AS NodePath,
        hierarchy_node.GetLevel() AS Level
    FROM GroupMember
    WHERE hierarchy_node.IsDescendantOf(@node) = 1
    ORDER BY hierarchy_node;
END;

CREATE PROCEDURE AddChildNode
    @parentNode hierarchyid,
    @group_id INT,
    @user_id INT,
    @role NVARCHAR(50)
AS
BEGIN
    DECLARE @newNode hierarchyid;

    -- Находим последнего потомка
    SELECT @newNode = MAX(hierarchy_node)
    FROM GroupMember
    WHERE hierarchy_node.GetAncestor(1) = @parentNode;

    -- Генерируем новый узел
    SET @newNode = @parentNode.GetDescendant(@newNode, NULL);

    INSERT INTO GroupMember (group_id, user_id, join_date, role, hierarchy_node)
    VALUES (@group_id, @user_id, GETDATE(), @role, @newNode);
END;

CREATE PROCEDURE MoveSubtree
    @nodeToMove hierarchyid,
    @newParent hierarchyid
AS
BEGIN
    DECLARE @newParentLastChild hierarchyid;
    DECLARE @newRoot hierarchyid;

    -- Последний потомок нового родителя
    SELECT @newParentLastChild = MAX(hierarchy_node)
    FROM GroupMember
    WHERE hierarchy_node.GetAncestor(1) = @newParent;

    -- Новый путь
    SET @newRoot = @newParent.GetDescendant(@newParentLastChild, NULL);

    -- Обновляем всю ветку
    UPDATE GroupMember
    SET hierarchy_node = hierarchy_node.GetReparentedValue(@nodeToMove, @newRoot)
    WHERE hierarchy_node.IsDescendantOf(@nodeToMove) = 1;
END;



INSERT INTO [User] (username, email, password_hash, date_of_birth, registration_date)
VALUES ('admin', 'admin@mail.com', 'hash', GETDATE(), GETDATE());

INSERT INTO [User] (username, email, password_hash, date_of_birth, registration_date)
VALUES ('user1', 'user1@mail.com', 'hash', GETDATE(), GETDATE());

INSERT INTO [Group] (creator_id, group_name, creation_date, privacy_setting)
VALUES (1, 'Test Group', GETDATE(), 'public');

INSERT INTO GroupMember (group_id, user_id, join_date, role, hierarchy_node)
VALUES (2, 1, GETDATE(), 'admin', hierarchyid::GetRoot());


DECLARE @root hierarchyid;
SET @root = hierarchyid::GetRoot();

EXEC AddChildNode 
    @parentNode = @root,
    @group_id = 2,
    @user_id = 2,
    @role = 'member';


DECLARE @node hierarchyid;
SET @node = hierarchyid::GetRoot();

EXEC GetDescendants @node;


DECLARE @nodeToMove hierarchyid;
DECLARE @newParent hierarchyid;

SET @nodeToMove = hierarchyid::Parse('/1/');
SET @newParent = hierarchyid::Parse('/2/');

EXEC MoveSubtree 
    @nodeToMove = @nodeToMove,
    @newParent = @newParent;