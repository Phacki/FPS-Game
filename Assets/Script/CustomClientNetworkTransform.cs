using Unity.Netcode.Components;

public class CustomClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    /// <summary>
    /// Used for players.
    /// You might migrate your player motion code into this.
    ///
    /// </summary>
    public override void OnNetworkSpawn()
    {
        // Assign the spawn location prior to invoking base.OnNetworkSpawn
        // in order to synchronize all clients with the player's spawned position
        // NOTE: This is a temporary work around to assure the player spawns in a valid
        // location.  You can use the "Teleport" method to accomplish this from the server
        // side as well.
        var spawnTransform = PlayerSpawnManager.Instance.GetSpawnPoint();

        // Update the position and rotation of the player to the spawn point's
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;

        base.OnNetworkSpawn();
    }
}
