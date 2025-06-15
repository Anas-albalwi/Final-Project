using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerSpawner : MonoBehaviour
{
    [Tooltip("0 ? Prefab Player1, 1 ? Prefab Player2")]
    public GameObject[] playerPrefabs;

    [Tooltip("Control scheme name as in your Input Actions asset")]
    public string controlScheme = "Gamepad";

    void Start()
    {
        //  «Ã·» ﬁ«∆„… √ÃÂ“… «·ÃÊÌ»«œ «·„ ’·…
        var pads = Gamepad.all;
        int max = Mathf.Min(playerPrefabs.Length, pads.Count);

        for (int i = 0; i < max; i++)
        {
            // ·ﬂ· ·«⁄»: «‰”Œ «·‹ prefab° «—»ÿÂ »«·ÃÂ«“° ÊÊ“¯⁄ «·‘«‘« 
            PlayerInput.Instantiate(
                playerPrefabs[i],
                controlScheme: controlScheme,
                pairWithDevice: pads[i]
            );
        }
    }
}
