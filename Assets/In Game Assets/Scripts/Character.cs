using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Running,
    Painting,
    Won
}

enum CharacterType
{
    Player,
    AI
}

public class Character : MonoBehaviour
{
    [SerializeField] State playerState;
    [SerializeField] CharacterType characterType;

    Vector3 startPos;
    Vector3 lastCheckpoint;

    Animator anim;
    Rigidbody rb;

    private int rank;

    List<Transform> characters = new List<Transform>();
    public GameObject characterParent;

    public Text rankText;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        lastCheckpoint = transform.position;

        EventManager.GameReset += SetDefaults;
        EventManager.GameWon += WinGame;
        EventManager.GameStarted += StartGame;


        if (characterType == CharacterType.Player)
            for (int i = 0; i < characterParent.transform.childCount; i++)
            {
                characters.Add(characterParent.transform.GetChild(i));
            }

    }

    private void Update()
    {        
        if(characterType == CharacterType.Player && playerState != State.Won)
        {
            rank = 1;
            foreach (Transform character in characters)
            {
                if (character.position.z > transform.position.z)
                    rank++;
            }

            rankText.text = rank + RankText(rank);
        }
    }

    private void StartGame()
    {
        PlayerState = State.Running;
    }

    private void SetDefaults()
    {
        transform.position = startPos;
        PlayerState = State.Idle;
        Debug.Log(playerState);
    }

    private void GoCheckpoint()
    {
        transform.position = lastCheckpoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
            GoCheckpoint();

        if(collision.collider.CompareTag("Stick"))
        {
            Vector3 _forceDirection = transform.position - collision.contacts[0].point;
            _forceDirection.y = 0;
            
            rb.AddForce(_forceDirection * 8, ForceMode.Impulse);
        }
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Only player can paint, others directly going idle state.
            if (characterType == CharacterType.Player)
            {
                PlayerState = State.Painting;
                EventManager.GoPaintingState();
            }
            else
                PlayerState = State.Idle;
        }

        if (other.CompareTag("Checkpoint"))
            lastCheckpoint = transform.position;
    }

    private void WinGame()
    {
        // Only player can paint. So winning after painting restricted only by player.
        if (characterType == CharacterType.Player)
            PlayerState = State.Won;
    }

    public State PlayerState
    {
        get { return playerState; }
        set
        {
            playerState = value;
            anim.SetInteger("state", (int)playerState);
        }
    }

    private string RankText(int rank)
    {
        if (rank == 1)
            return "st";
        else if (rank == 2)
            return "nd";
        else if (rank == 3)
            return "rd";
        else
            return "th";


    }
}
