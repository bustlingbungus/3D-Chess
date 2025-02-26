using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool move_forwards,
                move_backwards,
                move_left,
                move_right,
                move_up,
                move_down,
                select,
                cancel,
                pan_up,
                pan_down,
                pan_left,
                pan_right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GatherInput()
    {
        // clear previous input flags
        reset_flags();

        if (Input.GetAxis("move_fb") > 0f) move_forwards = true;
        else if (Input.GetAxis("move_fb") < 0f) move_backwards = true;
        if (Input.GetAxis("move_lr") > 0f) move_left = true;
        else if (Input.GetAxis("move_lr") < 0f) move_right = true;

        // reset axes so input is only pressed
        Input.ResetInputAxes();
    }

    private void reset_flags()
    {
        move_forwards = move_backwards = move_left = move_right =
        move_up = move_down = select = cancel = 
        pan_up = pan_down = pan_left = pan_right = false;
    }
}
