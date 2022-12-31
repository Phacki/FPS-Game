using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string idPrefix = "Player";
    private static Dictionary<string, PlayerManager> users = new Dictionary<string, PlayerManager>();

    public static void CreateUniqueUser(string networkID, PlayerManager user)
    {
        string userID = idPrefix + networkID;
        users.Add(userID, user);
        user.transform.name = userID;
    }

    public static void DeleteUniqueUser(string userID)
    {
        users.Remove(userID);
    }
}
