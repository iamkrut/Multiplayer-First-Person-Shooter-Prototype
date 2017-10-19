using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;
	public GameObject MuzzleFlash;

	public GameObject MuzzleFlashSpawnPoint;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

	private GameObject muzzleFlashParticles;

	public virtual void OnServerDisconnect(NetworkConnection conn)
	{
		NetworkServer.DestroyPlayersForConnection(conn);
		if (conn.lastError != NetworkError.Ok)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("ServerDisconnected due to error: " + conn.lastError);
			}
		}
	}

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
			CmdCallParticles ();
			GetComponent<AudioSource> ().Play ();
			//MuzzleFlash.GetComponent<ParticleSystem> ().Play ();
        }
    }
		
	[Command]
	void CmdCallParticles(){
		muzzleFlashParticles = Instantiate (MuzzleFlash, MuzzleFlashSpawnPoint.transform.position, MuzzleFlashSpawnPoint.transform.rotation);
		//MuzzleFlash.GetComponent<ParticleSystem> ().Play ();
		muzzleFlashParticles.transform.parent = MuzzleFlashSpawnPoint.transform;
		NetworkServer.Spawn (muzzleFlashParticles);
		//RpcSyncBlockOnce (muzzleFlashParticles.transform.localPosition, muzzleFlashParticles.transform.localRotation, muzzleFlashParticles, MuzzleFlashSpawnPoint);
	}
	/*
	[ClientRpc]
	public void RpcSyncBlockOnce(Vector3 localPos, Quaternion localRot, GameObject block, GameObject parent)
	{
		block.transform.parent = parent.transform;
		block.transform.localPosition = localPos;
		block.transform.localRotation = localRot;
	}
	*/


    [Client]
    void Shoot()
    {
		//MuzzleFlash.GetComponent<ParticleSystem> ().Play ();
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
			if (_hit.collider.CompareTag (PLAYER_TAG) && _hit.collider.gameObject.layer == LayerMask.NameToLayer("RemotePlayer"))
			{
				//_hit.collider.gameObject.GetComponent<Player> ().TakeDamage (weapon.damage);
				CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + "has been shot.");

        Player _player = GameManager.GetPlayer(_playerID);
		if (gameObject.GetComponent<Player> ().Team == _player.Team) { // same team
			return;
		}
        _player.RpcTakeDamage(_damage);
    }
}
