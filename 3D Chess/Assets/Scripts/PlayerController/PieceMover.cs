using UnityEngine;
using System.Collections.Generic;
using System;
using Defs;

public class PieceMover : MonoBehaviour
{
    [SerializeField]
    private KeyCode nextSelection = KeyCode.RightArrow,
                    previousSelection = KeyCode.LeftArrow,
                    selectCell = KeyCode.Return,
                    cancel = KeyCode.Escape;

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

    [SerializeField]
    private CellSelector selector;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(nextSelection)) select_move(curr_idx+1);
        if (Input.GetKeyDown(previousSelection)) select_move(curr_idx-1);
        if (Input.GetKeyDown(selectCell)) SelectCurrent();
        if (Input.GetKeyDown(cancel)) selector.ExitSelection();
    }

    public void SelectCurrent()
    {
        if (curr_idx<move_options.Count) {
            Selection v = move_options[curr_idx];
            v.move_piece.Cell = v.dest;
            v.move_piece.HideMoves();
            selector.ChangeTurn();
        } else selector.ExitSelection();
    }

    // cell highlights are added to the pieces in the same order as their moves, so this can be exploited
    // to make linking cell highlights easier
    public void GetMoves(Piece piece)
    {
        move_options = new List<Selection>();
        List<Move> moves = piece.find_valid_moves();
        if (moves.Count==0) selector.ExitSelection();
        else {
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
            select_move(0);
        }
    }

    private void select_move(int next_idx)
    {
        if (curr_idx>=0 && curr_idx<move_options.Count)
            set_material_alpha(0.15f,move_options[curr_idx].indicator);
        if (next_idx<0) curr_idx=move_options.Count-1;
        else if (next_idx>=move_options.Count) curr_idx = 0;
        else curr_idx = next_idx;
        if (curr_idx>=0 && curr_idx<move_options.Count)
            set_material_alpha(0.588f,move_options[curr_idx].indicator);
    }

    private void set_material_alpha(float alpha, GameObject indicator)
    {
        if (indicator==null) Debug.Log("oopsie doopsie!");
        Material mat = indicator.GetComponent<MeshRenderer>().material;
        Color newCol = mat.color;
        newCol.a = alpha;
        mat.color = newCol;
    }
}
