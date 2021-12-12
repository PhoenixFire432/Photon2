using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlocksController : MonoBehaviour
{
    public float speed;
    public float sensitivity;

    private Vector2 input;
    private Vector3 direction;
    private const string mouse_x = "Mouse X";
    private const string mouse_y = "Mouse Y";
    private float rot_x = 0;
    private float rot_y = 0;

    [Header("Debug Values")]
    public float x = 0;
    public float y = 0;

    private void Update()
    {
        // translate
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction = new Vector3(input.x, 0, input.y);

        this.gameObject.transform.Translate(direction * speed * Time.deltaTime);

        // rotate
        x = Input.GetAxis(mouse_x);
        y = Input.GetAxis(mouse_y);
        rot_x += x * sensitivity;
        rot_y -= y * sensitivity;

        transform.eulerAngles = new Vector3(rot_y, rot_x, 0);
    }
}
