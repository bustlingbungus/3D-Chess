using TMPro;
using UnityEngine;
using Defs;

/// <summary>
/// This is the behaviour script for the UI that pops up when you hover over a piece, showing information about that piece.
/// 
/// On being enabled via <c>SetPiece</c>, will update the ui to list the type and colour of said piece.
/// </summary>
public class SelectionPopUp : MonoBehaviour
{
    /// <summary> Reference to the text renderer showing piece info. </summary>
    [SerializeField]
    private TMP_Text pieceInfo;
    
    /// <summary> Reference to the parent UI object. </summary>
    [SerializeField]
    private GameObject popUp;

    /// <summary>
    /// Enables piece info UI, using the given piece's type and colour. 
    /// </summary>
    /// <param name="piece">Reference to the piece to base the UI's type and colour</param>
    public void SetPiece(Piece piece)
    {
        // enable UI
        popUp.SetActive(true);
        gameObject.SetActive(true);
        
        // convert type and colour to strings
        string ptype, 
               pcolour = piece.Colour==TeamColour.White? "White ":"Black ";

        switch (piece.Type)
        {
            case PieceType.Pawn:    ptype = "Pawn";             break;
            case PieceType.Rook:    ptype = "Rook";             break;
            case PieceType.Knight:  ptype = "Knight";           break;
            case PieceType.Bishop:  ptype = "Bishop";           break;
            case PieceType.Queen:   ptype = "Queen";            break;
            case PieceType.King:    ptype = "King";             break;
            default:                ptype = "Unknown Piece";    break;
        }

        // Set UI text
        pieceInfo.SetText("Selected Piece:\n" + pcolour + ptype);
    }

    /// <summary> Disables piece info UI. </summary>
    public void Hide()
    {
        // disable UI objects
        popUp.SetActive(false);
        gameObject.SetActive(false);
    }
}
