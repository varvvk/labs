using UnityEngine;

public class task5 : MonoBehaviour
{
    public Vector3 startPos;
    public Quaternion startRot;

    Vector3 needpos = new Vector3(3f, 0, 0);
    Quaternion needrot = Quaternion.Euler(0, -90, 0);

    public float speed = 1f;
    float offset;

    bool move = false;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (move)
        {
            offset = offset + speed * Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, needpos, offset);
            transform.rotation = Quaternion.Slerp(startRot, needrot, offset);

            if (offset >= 1f)
                move = false;
        }
    }

    public void moveToCube()
    {
        offset = 0f;
        move = true;
    }

    public void moveBack()
    {
        offset = 0f;
        move = false;

        transform.position = startPos;
        transform.rotation = startRot;
    }
}