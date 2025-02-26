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

    private Piece curr_piece;
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
        if (curr_idx<curr_piece.available_moves.Count) 
        {
            // get the current move
            MoveInfo v = curr_piece.available_moves[curr_idx];
            // hide the move indicators
            curr_piece.HideMoves();
            // move the piece to the selected cell
            curr_piece.TravelTo(v.cell);
            // change turns
            selector.ChangeTurn();
        } 
        else selector.ExitSelection();
    }

    // cell highlights are added to the pieces in the same order as their moves, so this can be exploited
    // to make linking cell highlights easier
    public void GetMoves(Piece piece)
    {
        if (piece.available_moves.Count==0) selector.ExitSelection();
        else {
            curr_piece = piece;
            select_move(0);
        }
    }

    private void select_move(int next_idx)
    {
        if (curr_idx>=0 && curr_idx<curr_piece.available_moves.Count)
            set_material_alpha(0.15f,curr_piece.available_moves[curr_idx].indicator);

        if (next_idx<0) curr_idx=curr_piece.available_moves.Count-1;
        else if (next_idx>=curr_piece.available_moves.Count) curr_idx = 0;
        else curr_idx = next_idx;

        if (curr_idx>=0 && curr_idx<curr_piece.available_moves.Count)
            set_material_alpha(0.588f,curr_piece.available_moves[curr_idx].indicator);
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
