using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

	public GameObject camera;

	public GameObject explosion;
	public GameObject playerBody;
	public GameObject playerGun;
	public GameObject respawnPanel;
	public Text respawnPanelText;
	public GameObject ExplosionSound;
	public GameObject damagePanel;
	public Text healthText;

	public GameObject gameOverCanvas;
	public Text gameOverText;

	public GameObject[] gameObjectsToDisableOnGameOver;

	[SyncVar]
	private int team; // 0 = blue 1 = red
	public int Team
	{
		get { return team; }
		set { team = value; }
	}

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

	// UI health bar
	//public GameObject HealthBar;

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

	public void Awake(){
		//teamSelectionCanvas.SetActive (true);
		//HealthBar.GetComponent<BarScript> ().MaxValue = maxHealth;
		//UpdateHealthBar (maxHealth);
		UpdateHealthText (maxHealth);
	}

    public void Setup()
    {
		gameObject.GetComponentInChildren<crosshairscript> ().enabled = true;
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    //void Update()
    //{
    //    if (!isLocalPlayer)
    //        return;

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(99999);
    //    }
    //}

	void Update(){
		if (!isLocalPlayer)
		 	return;

		TimerScript.instance.CmdIsTimerOver ();
		TimerScript.instance.CmdTimerZero ();

		if (TimerScript.instance.showGameOverCanvas) {

			camera.GetComponent<crosshairscript> ().enabled = false;

			for (int i = 0; i < gameObjectsToDisableOnGameOver.Length; i++)
			{
				gameObjectsToDisableOnGameOver[i].SetActive(false);
			}

			gameOverCanvas.SetActive (true);
			if (Int32.Parse(ScoreManager.instance.redScoreText.text) > Int32.Parse(ScoreManager.instance.blueScoreText.text)) {
				gameOverText.color = Color.red;
				gameOverText.text = "Red Team Wins";
			} else if (Int32.Parse(ScoreManager.instance.redScoreText.text) < Int32.Parse(ScoreManager.instance.blueScoreText.text)) {
				gameOverText.color = Color.blue;
				gameOverText.text = "Blue Team Wins";
			} else {
				gameOverText.color = Color.white;
				gameOverText.text = "Draw Game";
			}
		}
	}


    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
            return;

		StartCoroutine (ShowDamagePanel());
        currentHealth -= _amount;

		UpdateHealthText (currentHealth);
		//UpdateHealthBar (currentHealth);

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

	IEnumerator ShowDamagePanel(){
		damagePanel.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		damagePanel.SetActive (false);
	}

    private void Die()
    {
        isDead = true;

		camera.GetComponent<crosshairscript> ().enabled = false;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + "is DEAD!");

        StartCoroutine(Respawn());
		Debug.Log (team);
		ScoreManager.instance.CmdUpdateTeamScore (team);
		Debug.Log (team);
    }

	public void TakeDamage(int _amount){
		if (isDead)
			return;

		StartCoroutine (ShowDamagePanel());
		currentHealth -= _amount;

		//UpdateHealthBar (currentHealth);
		UpdateHealthText (currentHealth);

		Debug.Log(transform.name + " now has " + currentHealth + " health.");

		if(currentHealth <= 0)
		{
			Die();
		}
	}

    private IEnumerator Respawn()
    {
		Instantiate (explosion, gameObject.transform.position, Quaternion.identity);
		ExplosionSound.GetComponent<AudioSource> ().Play ();
		playerBody.SetActive (false);
		playerGun.SetActive (false);
		respawnPanel.SetActive (true);

		respawnPanelText.text = "Respawn in " + (GameManager.instance.matchSettings.respawnTime - 0 + 1);
        yield return new WaitForSeconds(1);

		respawnPanelText.text = "Respawn in " + (GameManager.instance.matchSettings.respawnTime - 1 + 1);
		yield return new WaitForSeconds(1);

		respawnPanelText.text = "Respawn in " + (GameManager.instance.matchSettings.respawnTime - 2 + 1);
		yield return new WaitForSeconds(1);

		respawnPanelText.text = "Respawn in " + (GameManager.instance.matchSettings.respawnTime - 3 + 1);
		yield return new WaitForSeconds(1);

		respawnPanelText.text = "Respawn in " + (GameManager.instance.matchSettings.respawnTime - 4 + 1);
		yield return new WaitForSeconds(1);

		playerBody.SetActive (true);
		playerGun.SetActive (true);
		respawnPanel.SetActive (false);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawned");
		UpdateHealthText (maxHealth);
		//UpdateHealthBar (maxHealth);
    }

    public void SetDefaults()
    {
		isDead = false;

		camera.GetComponent<crosshairscript> ().enabled = true;

        currentHealth = maxHealth;
		UpdateHealthText (currentHealth);
		//UpdateHealthBar (currentHealth);

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

	/*
	public void UpdateHealthBar(int health){
		if (HealthBar != null) {
			HealthBar.GetComponent<BarScript> ().MaxValue = Mathf.Clamp (health, 0, maxHealth);
			HealthBar.GetComponent<BarScript> ().Value = Mathf.Clamp (health, 0, maxHealth);
			HealthBar.GetComponent<BarScript> ().ChangeHealthBar (health);
		}
	}
	*/

	public void UpdateHealthText(int health){
		healthText.text = health.ToString ();
	}

	[ClientRpc]
	public void RpcShowPlayer(string playerID, int no)
	{
		Player player = GameManager.GetPlayer (playerID);
		player.transform.GetChild (1).transform.GetChild (0).gameObject.SetActive (true);
		player.transform.GetChild (2).transform.GetChild (no).gameObject.SetActive (true);

		Debug.Log ("callerd");
	}
}
