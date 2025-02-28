using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    public bool MoveForwards { get => _axis_move_fb == 1; }
    public bool MoveBackwards { get => _axis_move_fb == -1; }
    public bool MoveRight { get => _axis_move_lr == 1; }
    public bool MoveLeft { get => _axis_move_lr == -1; }
    public bool MoveUp { get => _axis_move_ud == 1; }
    public bool MoveDown { get => _axis_move_ud == -1; }

    public bool PanRight { get => _axis_pan_lr == 1; }
    public bool PanLeft { get => _axis_pan_lr == -1; }
    public bool PanUp { get => _axis_pan_ud == 1; }
    public bool PanDown { get => _axis_pan_ud == -1; }

    public bool CycleNext { get => _axis_cycle == 1; }
    public bool CyclePrevious { get => _axis_cycle == -1; }

    public bool Select { get => _axis_select == 1; }
    public bool Cancel { get => _axis_cancel == 1; }

    private float   _axis_move_fb,
                    _axis_move_lr,
                    _axis_move_ud,
                    _axis_pan_lr,
                    _axis_pan_ud,
                    _axis_cycle,
                    _axis_select,
                    _axis_cancel;

    private Dictionary<string,bool> input_axes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetFlags(); 

        input_axes = new Dictionary<string, bool>
        {
            {"move_fb", true},
            {"move_lr", true},
            {"move_ud", true},
            {"pan_lr", true},
            {"pan_ud", true},
            {"cycle", true},
            {"select", true},
            {"cancel", true},
        };
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        ResetFlags();

        Stack<KeyValuePair<string,bool>> modifications = new Stack<KeyValuePair<string,bool>>();

        foreach (KeyValuePair<string,bool> pair in input_axes)
        {
            float raw = Input.GetAxis(pair.Key);

            // if (pair.Key == "select")
            // Debug.Log(pair.Key + " "+ string.Format("{0:N2}", raw));

            if (raw==0f) {
                get_axis_reference(pair.Key) = 0f;
                modifications.Push(new KeyValuePair<string,bool>(pair.Key,true));
            } else if (pair.Value) {
                get_axis_reference(pair.Key) = MathF.Sign(raw);
                modifications.Push(new KeyValuePair<string,bool>(pair.Key,false));
            }
        }

        while (modifications.Count > 0) {
            KeyValuePair<string,bool> mod = modifications.Pop();
            input_axes[mod.Key] = mod.Value;
        }
    }

    private void ResetFlags()
    {
        _axis_move_fb = 0f;
        _axis_move_lr = 0f;
        _axis_move_ud = 0f;
        _axis_pan_lr = 0f;
        _axis_pan_ud = 0f;
        _axis_cycle = 0f;
        _axis_select = 0f;
        _axis_cancel = 0f;
    }

    private ref float get_axis_reference(string name)
    {
        switch (name)
        {
            case "move_fb": return ref _axis_move_fb;
            case "move_lr": return ref _axis_move_lr;
            case "move_ud": return ref _axis_move_ud;
            case "pan_lr": return ref _axis_pan_lr;
            case "pan_ud": return ref _axis_pan_ud;
            case "cycle": return ref _axis_cycle;
            case "select": return ref _axis_select;
            case "cancel": return ref _axis_cancel;
        }
        return ref _axis_move_fb;
    }
}
