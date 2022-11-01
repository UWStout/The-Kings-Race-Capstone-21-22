using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class GameHandler : NetworkBehaviour
{
    public Transform coundownUI;
    public Transform playerHUD;

    private static GameObject _countdownUI;
    private static GameObject _playerHUD;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(introCutscene());
    }

    IEnumerator introCutscene() {
        // Get the time to complete the intro camera fly through
        float cameraLerpTime = Camera.main.GetComponent<CPC_CameraPath>().playOnAwakeTime;

        // Wait for that many seconds - allows for time to complete the "cutscene"
        yield return new WaitForSecondsRealtime(cameraLerpTime);

        // Remove the main camera and give camera to local players
        Camera.main.gameObject.SetActive(false);

        // Get all players in the scene - runners and king
        GameObject[] playableCharacters = GameObject.FindGameObjectsWithTag("Player");

        // Iterate over them and if their network object equals the call to the rpc, enable that camera
        GameObject localPlayer = null;
        foreach (GameObject character in playableCharacters) {
            if (character.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId) {
                localPlayer = character;

                if(FindGameObjectInChildWithTag(character, "UICam") != null){
                    FindGameObjectInChildWithTag(character, "UICam").GetComponent<Camera>().enabled = true;
                
                    GameObject UICamera =  FindGameObjectInChildWithTag(character, "UICam");
                    FindGameObjectInChildWithTag(UICamera, "PlayerCam").GetComponent<Camera>().enabled = true;
                    FindGameObjectInChildWithTag(UICamera, "PlayerCam").GetComponent<AudioListener>().enabled = true;
                    if (FindGameObjectInChildWithTag(UICamera, "PlayerCam").GetComponent<PlayerCam>() != null) {
                        FindGameObjectInChildWithTag(UICamera, "PlayerCam").GetComponent<PlayerCam>().enabled = true;
                    }
                }
                else{
                    FindGameObjectInChildWithTag(character, "PlayerCam").GetComponent<Camera>().enabled = true;
                    FindGameObjectInChildWithTag(character, "PlayerCam").GetComponent<AudioListener>().enabled = true;
                    if (FindGameObjectInChildWithTag(character, "PlayerCam").GetComponent<PlayerCam>() != null) {
                        FindGameObjectInChildWithTag(character, "PlayerCam").GetComponent<PlayerCam>().enabled = true;
                    }                    
                }

            }
        }

        // Spawn the 3.2.1 coundown object
        if (IsHost) {
            SpawnCountdownServerRpc();
        }

        yield return new WaitForSecondsRealtime(5f);

        // Despawn the 3.2.1 coundown object
        if (IsHost) {
            DespawnCountdownServerRpc();
        }

        // Re-enable the runner movement
        if (localPlayer.GetComponentInChildren<MoveStateManager>() != null) {
            localPlayer.GetComponentInChildren<MoveStateManager>().enabled = true;
        }
        if (localPlayer.GetComponentInChildren<AerialStateManager>() != null) {
            localPlayer.GetComponentInChildren<AerialStateManager>().enabled = true;
        }
        if (localPlayer.GetComponentInChildren<OffenseStateManager>() != null) {
            localPlayer.GetComponentInChildren<OffenseStateManager>().enabled = true;
        }
        if (localPlayer.GetComponentInChildren<DashStateManager>() != null) {
            localPlayer.GetComponentInChildren<DashStateManager>().enabled = true;
        }
        if (localPlayer.GetComponentInChildren<NitroStateManager>() != null) {
            localPlayer.GetComponentInChildren<NitroStateManager>().enabled = true;
        }

        // Re-enable the kings movement
        if (localPlayer.GetComponent<KingMove>() != null) {
            localPlayer.GetComponent<KingMove>().enabled = true;
        }

        // Start the game timer and spawn HUDs
        if (IsHost) {
            SpawnPlayerHUDServerRpc();
        }
    }

    // Only works for 1st generation children
    public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag) {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++) { 
            if (t.GetChild(i).gameObject.tag == tag) {
                return t.GetChild(i).gameObject;
            }
        }

        // Couldn't find child with tag
        return null;
    }

    [ServerRpc]
    private void SpawnCountdownServerRpc(ServerRpcParams serverRpcParams = default) {
        _countdownUI = Instantiate(coundownUI, Vector3.zero, Quaternion.identity).gameObject;
        _countdownUI.GetComponent<NetworkObject>().Spawn(null, true);
    }

    [ServerRpc]
    private void DespawnCountdownServerRpc(ServerRpcParams serverRpcParams = default) {
        _countdownUI.GetComponent<NetworkObject>().Despawn(true);
        Destroy(_countdownUI);
    }

    [ServerRpc]
    private void SpawnPlayerHUDServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerHUD = Instantiate(playerHUD, Vector3.zero, Quaternion.identity).gameObject;
        _playerHUD.GetComponent<NetworkObject>().Spawn(null, true);
    }
}
