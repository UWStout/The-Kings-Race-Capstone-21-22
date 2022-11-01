using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    //values for runner
    [SerializeField]

    //collection that contains all the current actions a runner can perform, and what keycode it is currently bound to
    public Dictionary<string,KeyCode> bindableActions = new Dictionary<string, KeyCode>();
    //collection that helps convert a keycode to its coresponding sprite
    public Dictionary<KeyCode, Sprite> keyToSpriteDict = new Dictionary<KeyCode, Sprite>();
    //collection that converts a keycode to it's coresponding string 
    public Dictionary<KeyCode, string> keyToStringDict = new Dictionary<KeyCode, string>();
    //collection that contains all button sprites  
    public Sprite[] allSprites;

    private void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }

        //set keycodes equal to player prefs or set defaults
        KeyCode slide = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("slideKey", "Q"));
        KeyCode kick = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("kickKey", "F"));
        KeyCode dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("dashKey", "R"));
        KeyCode nitro = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("nitroKey", "LeftShift"));
        KeyCode grapple = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("grappleKey", "E"));


        //add keycodes to collection
        bindableActions.Add("slideKey", slide);
        bindableActions.Add("kickKey", kick);
        bindableActions.Add("dashKey", dash);
        bindableActions.Add("nitroKey", nitro);
        bindableActions.Add("grappleKey", grapple);


        //for keyboard
        /*        walkForward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkForwardKey", "W"));
                walkBackward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkBackwardKey", "S"));
                walkLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkLeftKey", "A"));
                walkRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkRightKey", "D"));*/
        //KeyCode jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));

        //dictionary with keycode as key and value as sprites
        keyToSpriteDict.Add(KeyCode.Q, allSprites[0]);
        keyToSpriteDict.Add(KeyCode.E, allSprites[1]);
        keyToSpriteDict.Add(KeyCode.R, allSprites[2]);
        keyToSpriteDict.Add(KeyCode.F, allSprites[3]);
        keyToSpriteDict.Add(KeyCode.LeftShift, allSprites[4]);

        //dictionary with keycode as key and value as string of key
        keyToStringDict.Add(KeyCode.Q, "Q");
        keyToStringDict.Add(KeyCode.E, "E");
        keyToStringDict.Add(KeyCode.R, "R");
        keyToStringDict.Add(KeyCode.F, "F");
        keyToStringDict.Add(KeyCode.LeftShift, "Left Shift");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
