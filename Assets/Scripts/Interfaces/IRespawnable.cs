using UnityEngine;

public interface IRespawnable {
	void Respawn();
	void SetRespawnPosition(Vector3 position);
    Vector3 GetRespawnPosition();
}