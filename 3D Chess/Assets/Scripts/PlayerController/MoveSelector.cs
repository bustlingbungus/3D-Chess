using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using Defs;

public class MoveSelector : MonoBehaviour
{
    [SerializeField]
    private KeyCode nextSelection = KeyCode.RightArrow,
                    previousSelection = KeyCode.LeftArrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(nextSelection)) select_next();
        if (Input.GetKeyDown(previousSelection)) select_previous();
    }


    public void SelectCurrent()
    {
        if (curr_idx<move_options.Count) {
            Selection v = move_options[curr_idx];
            v.move_piece.Cell = v.dest;
            v.move_piece.HideMoves();
        }
    }

    private void select_next()
    {
        if (curr_idx<move_options.Count) {
            // lower current material alpha
            set_material_alpha(0.15f,move_options[curr_idx].indicator);
            // update index
            if (++curr_idx>=move_options.Count) curr_idx = 0;
            // raise next material alpha
            set_material_alpha(0.588f,move_options[curr_idx].indicator);
        }

    }

    private void select_previous()
    {
        if (curr_idx<move_options.Count) {
            // lower current material alpha
            set_material_alpha(0.15f,move_options[curr_idx].indicator);
            // update index
            if (--curr_idx<0) curr_idx = move_options.Count-1;
            // raise previous material alpha
            set_material_alpha(0.588f,move_options[curr_idx].indicator);
        }
    }


    private void set_material_alpha(float alpha, GameObject indicator)
    {
        if (indicator==null) Debug.Log("oopsie doopsie!");
        Material mat = indicator.GetComponent<MeshRenderer>().material;
        Color newCol = mat.color;
        newCol.a = alpha;
        mat.color = newCol;
    }



    // cell highlights are added to the pieces in the same order as their moves, so this can be exploited
    // to make linking cell highlights easier
    public void GetMoves(Piece piece)
    {
        move_options = new List<Selection>();
        List<Move> moves = piece.find_valid_moves();
        foreach (Move move in moves) move_options.Add(new Selection(move.cell, piece));

        int i = 0;
        foreach (Transform child in piece.transform) {
            if (child.tag=="Cell Highlight") {
                Selection v = move_options[i];
                v.indicator = child.gameObject;
                move_options[i] = v;
                if (++i>=move_options.Count) break;
            }
        }
        curr_idx = 0;
        select_next();
    }

    private struct Selection {
        public Selection(Cell Dest=null, Piece piece=null, GameObject Indicator=null) {
            move_piece = piece; dest = Dest; indicator = Indicator; 
        }
        public Piece move_piece;
        public Cell dest;
        public GameObject indicator;
    }
    private List<Selection>  move_options;
    private int curr_idx = 0;
}
