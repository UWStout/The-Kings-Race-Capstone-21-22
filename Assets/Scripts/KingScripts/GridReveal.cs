using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridReveal : MonoBehaviour
{
    private GameObject[] Rows;
    private float TempKingPos = 0;
    private int Index = 0;
    private int ROSCount = 110;
    private int BoxSize = 20;
    private int ValleyStr = -670;
    private int MountainStr = -2640;

    void Start() {
        List<GameObject> RowsList = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)//Creates a list of all the Rows in the Grid
        {
            RowsList.Add(child.gameObject);
        }
        Rows = RowsList.ToArray();//Puts the list into the Array
        for (int i = Rows.Length-1; i > ROSCount-1; i--) {// Disables most of the Rows that the King can't see
            Rows[i].SetActive(false);
        }
        ToggleMountainGrid(true);
    }

    public void ToggleMountainGrid(bool state) {
        Rows[Rows.Length - 2].SetActive(state);
    }

    public void GridSwitch(bool State) {//Activates or deactivates the Grid
        gameObject.SetActive(State);
    }

    //A function for enabling and disabalimg the Visability of the King's Grid as the King Moves
    public void DynGridReveal(float KingPos, float MovDir) {
        if(KingPos > ValleyStr || KingPos <= MountainStr) { //Cancels out the function if the King is in the valley or the mountain
            return;
        }
        if(TempKingPos >= KingPos + BoxSize || TempKingPos <= KingPos - BoxSize) { //Moves the Grid Everytime is King has moved 20 Units
            TempKingPos = KingPos;
            if (MovDir == 0) {
                return;
            }
            else if (MovDir >= 0) { //Moving Right
                Rows[Index].SetActive(false);//Deactivates Leftmost
                if (Index <= (Rows.Length - 1) - ROSCount) {//Prevents OfB error
                    Rows[Index + ROSCount].SetActive(true);//Activates Rightmost
                    Index++;
                }
            }
            else if (MovDir <= 0) { //Moving Left        
                Rows[Index].SetActive(true);//Activates Leftmost
                if (Index >= 0) {//Prevents OfB error
                    Rows[Index + ROSCount].SetActive(false);//Deactivates Rightmost
                    Index--;
                }
            }
            if(Index > ((Rows.Length - 1) - ROSCount)) {
                Index = (Rows.Length - 1) - ROSCount;
            }
            if(Index < 0) {
                Index = 0;
            }
        }

        //Mov Dir tells if the King is moving left (-1) right (1) or not at all (0)
        //Find where the king is, and enable and disable rows in accordance with that
        //As the king moves, find the direction, and disable and enable rows on the edges of vision (Find how many rows the King'll be able to see.)
    }
}
