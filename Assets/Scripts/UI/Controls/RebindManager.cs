using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebindManager : MonoBehaviour
{
    Event keyEvent;
    KeyCode newKey;
    GameObject currentButtonObject;
    //const array of valid input
    readonly KeyCode[] valildKeys = {KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.F, KeyCode.LeftShift};


    //holds ref to all game buttons in scene (should be assigned in editor)
    public List<GameObject> allButtonObjects = new List<GameObject>();
 
    //currently waiting for input from user
    bool waitingForKey;
    bool hasPressedValidKey = false;
    // Start is called before the first frame update
    void Start()
    {
        List<KeyCode> tHolder = new List<KeyCode>();
        foreach (var item in GameManager.GM.bindableActions)
        {
            Debug.Log(item.Key + item.Value);
            tHolder.Add(item.Value);
        }


        //currently hardcoded
        allButtonObjects[0].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["grappleKey"]];
        allButtonObjects[1].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["slideKey"]];
        allButtonObjects[2].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["kickKey"]];
        allButtonObjects[3].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["dashKey"]];
        allButtonObjects[4].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["nitroKey"]];

        //if any of them are shift, set them to correct dimension
        //hardcoded, if anythem are bound to leftshift
        if (GameManager.GM.bindableActions["grappleKey"] == KeyCode.LeftShift)
        {
            allButtonObjects[0].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        }
        else if (GameManager.GM.bindableActions["slideKey"] == KeyCode.LeftShift)
        {
            allButtonObjects[1].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        }
        else if (GameManager.GM.bindableActions["kickKey"] == KeyCode.LeftShift)
        {
            allButtonObjects[2].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        }
        else if (GameManager.GM.bindableActions["dashKey"] == KeyCode.LeftShift)
        {
            allButtonObjects[3].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        }
        else if (GameManager.GM.bindableActions["nitroKey"] == KeyCode.LeftShift)
        {
            allButtonObjects[4].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
        }


    }
    // Update is called once per frame
    void Update()
    {

    }

    //Is called everytime a GUI event is triggered
    void OnGUI()
    {
        keyEvent = Event.current;
        //if is a key event
        if (keyEvent.isKey)
        {
            //if waiting for a key and the key entered is valid
            if (waitingForKey && isValidKey(keyEvent.keyCode))
            {
                //set newKey equal to current key event
                newKey = keyEvent.keyCode;
                //set waiting for key to false
                waitingForKey = false;
                //set hasPressedValidKey to true
                hasPressedValidKey = true;
            }
        }
    }
    public void SendCurrentButton(GameObject gameObject)
    {
        currentButtonObject = gameObject;
    }

    //called from buttons in the controls menu scene
    public void StartAssignment(string keyName)
    {
        //if not already waiting for a key input from user
        if (!waitingForKey)
        {
            StartCoroutine(AssignKey(keyName));
        }
    }

    //
    IEnumerator waitForKey()
    {
        //wait until user has inputed a valid key
        while (hasPressedValidKey == false)
        {
            yield return null;
        }
        //if they have gotten past this loop, a valid key was given
    }
    public bool isValidKey(KeyCode keycode)
    {
        bool hasValue = false;
        for(int i =0; i < valildKeys.Length; i++) {
            if(valildKeys[i] == keycode)
            {
                hasValue = true;
            }
        }
        return hasValue;
    }
    public bool areThereDuplicates()
    {
        //retrieve values from dictionary and put in list
        bool duplicateExists = false;
        List<KeyCode> tHolder = new List<KeyCode>();
        foreach (var item in GameManager.GM.bindableActions)
        {
            tHolder.Add(item.Value);
        }

/*        foreach (var item in tHolder)
        {
            Debug.Log(item);
        }*/


        //check list for duplicates
        for (int i=0; i < tHolder.Count - 1; i++)
        {
            for(int j=i+1; j < tHolder.Count; j++)
            {
                if(tHolder[i] == tHolder[j])
                {
                    //if there is a duplicate
                    //set flag to tru
                    duplicateExists = true;
                }
            }
        }
        return duplicateExists;
    }
    public void setAllControlsToDefaults()
    {
        //set gm buttons to default
        GameManager.GM.bindableActions["kickKey"] = KeyCode.F;
        GameManager.GM.bindableActions["slideKey"] = KeyCode.Q;
        GameManager.GM.bindableActions["dashKey"] = KeyCode.R;
        GameManager.GM.bindableActions["nitroKey"] = KeyCode.LeftShift;
        GameManager.GM.bindableActions["grappleKey"] = KeyCode.E;

        //set prefs turned off for now
       /* PlayerPrefs.SetString("kickKey", GameManager.GM.bindableActions["kickKey"].ToString());
        PlayerPrefs.SetString("slideKey", GameManager.GM.bindableActions["slideKey"].ToString());
        PlayerPrefs.SetString("dashKey", GameManager.GM.bindableActions["dashKey"].ToString());
        PlayerPrefs.SetString("nitroKey", GameManager.GM.bindableActions["nitroKey"].ToString());
        PlayerPrefs.SetString("grappleKey", GameManager.GM.bindableActions["grappleKey"].ToString());*/

        //set all sprites done (probs doesn't set scale back to default)
        allButtonObjects[0].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["grappleKey"]];
        allButtonObjects[1].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["slideKey"]];
        allButtonObjects[2].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["kickKey"]];
        allButtonObjects[3].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["dashKey"]];
        allButtonObjects[4].GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[GameManager.GM.bindableActions["nitroKey"]];
    }
    public IEnumerator AssignKey(string keyName)
    {
        //set waiting for key true
        waitingForKey = true;
        //call coroutine that will wait until a valid key is pressed
        yield return waitForKey();

        //once a valid key has been pressed, 
        switch (keyName)
        {
            //player prefs are used to keep key binds after game has closed (won't do anything until then). They are commented out for now for SGX.
            case "kick":
                GameManager.GM.bindableActions["kickKey"] = newKey;
                //replace sprite of button
                currentButtonObject.GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[newKey];

                //if button is LeftShift, set sprite to a different size
                if (newKey == KeyCode.LeftShift)
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
                }
                //else set sprite to its regular size
                else
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                }
                //PlayerPrefs.SetString("kickKey", GameManager.GM.bindableActions["kickKey"].ToString());
                //revert flag
                hasPressedValidKey = false;
                break;
            case "slide":
                GameManager.GM.bindableActions["slideKey"] = newKey;
                //replace image of button
                currentButtonObject.GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[newKey];

                //if the new button is LeftShift, resie
                if(newKey == KeyCode.LeftShift)
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
                }
                //else set to regular size
                else
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                }
                //PlayerPrefs.SetString("slideKey", GameManager.GM.bindableActions["slideKey"].ToString());
                //revert flag
                hasPressedValidKey = false;
                break;
            case "dash":
                 GameManager.GM.bindableActions["dashKey"] = newKey;
                //replace image of button
                currentButtonObject.GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[newKey];

                //if the new button is LeftShift, resize (sprite image requires it)
                if (newKey == KeyCode.LeftShift)
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
                }
                //else set to regular size
                else
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                }
                //PlayerPrefs.SetString("dashKey", GameManager.GM.bindableActions["dashKey"].ToString());
                //revert flag
                hasPressedValidKey = false;
                break;
            case "nitro":
                GameManager.GM.bindableActions["nitroKey"] = newKey;
                //replace image of button
                currentButtonObject.GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[newKey];
                //if the new button is LeftShift, resie
                if (newKey == KeyCode.LeftShift)
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
                }
                //else set to regular size
                else
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                }
                //PlayerPrefs.SetString("nitroKey", GameManager.GM.bindableActions["nitroKey"].ToString());
                //revert flag
                hasPressedValidKey = false;
                break;
            case "grapple":
                GameManager.GM.bindableActions["grappleKey"] = newKey;
                //replace image of button
                currentButtonObject.GetComponent<Image>().sprite = GameManager.GM.keyToSpriteDict[newKey];
                //if the new button is LeftShift, resie
                if (newKey == KeyCode.LeftShift)
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);
                }
                //else set to regular size
                else
                {
                    currentButtonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                }
                //PlayerPrefs.SetString("grappleKey", GameManager.GM.bindableActions["grappleKey"].ToString());
                //revert flag
                hasPressedValidKey = false;
                break;
        }
        yield return null;
    }
}
