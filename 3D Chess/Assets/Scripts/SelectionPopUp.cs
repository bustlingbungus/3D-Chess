using TMPro;
using UnityEngine;
using Defs;

public class SelectionPopUp : MonoBehaviour
{
    [SerializeField]
    private TMP_Text pieceInfo;
    [SerializeField]
    private GameObject popUp;

    public void SetPiece(Piece piece)
    {
        popUp.SetActive(true);
        gameObject.SetActive(true);
        
        string ptype, pcolour = piece.Colour==TeamColour.White? "White ":"Black ";
        switch (piece.Type)
        {
            case PieceType.Pawn: ptype = "Pawn"; break;
            case PieceType.Rook: ptype = "Rook"; break;
            case PieceType.Knight: ptype = "Knight"; break;
            case PieceType.Bishop: ptype = "Bishop"; break;
            case PieceType.Queen: ptype = "Queen"; break;
            case PieceType.King: ptype = "King"; break;
            default: ptype = "Unknown Piece"; break;
        }

        pieceInfo.SetText("Selected Piece:\n" + pcolour + ptype);
    }

    public void Hide()
    {
        popUp.SetActive(false);
        gameObject.SetActive(false);
    }
}
