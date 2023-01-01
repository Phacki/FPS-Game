using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string idPrefix = "Player";
    private static Dictionary<string, takeDamage> users = new Dictionary<string, takeDamage>();

    public static void CreateUniqueUser(string networkID, takeDamage user)
    {
        string userID = idPrefix + networkID;
        users.Add(userID, user);
        user.transform.name = userID;
    }

    public static void DeleteUniqueUser(string userID)
    {
        users.Remove(userID);
    }

    public static takeDamage GetUser (string userID)
    {
        return users[userID];
    }
}
