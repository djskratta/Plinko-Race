using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DaBePlayerController : MonoBehaviour 
{

	public float speed;
	public Text countText;
	public Text winText;
	public AudioClip pickupSound;
	public AudioClip finishLine;

	private AudioSource source;
	private Rigidbody rb;
	private int count;
	private float timer;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		timer = 0;
		winText.text = "";
	}

	void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

		if (Input.GetKey("escape"))
			Application.Quit ();

		timer = timer + Time.deltaTime;
		if (timer >= 10) 
		{
			winText.text = "You finished with a score of: " + count.ToString ();
			StartCoroutine(ByeAfterDelay (2));
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("pickup")) 
		{
			source.PlayOneShot(pickupSound);
			other.gameObject.SetActive (false);
			count = count + 1;
			GameLoader.AddScore(1);
			countText.text = "Score: " + count.ToString ();
		}
		else if (other.gameObject.CompareTag ("finishline")) 
		{
			source.PlayOneShot(finishLine);
			other.gameObject.SetActive (false);
			count = count + 3;
			GameLoader.AddScore(3);
			countText.text = "Score: " + count.ToString ();
			winText.text = "You finished with a score of: " + count.ToString ();
		}
	}

	IEnumerator ByeAfterDelay(float time)
	{
		yield return new WaitForSeconds(time);
		GameLoader.gameOn = false;
	}
}