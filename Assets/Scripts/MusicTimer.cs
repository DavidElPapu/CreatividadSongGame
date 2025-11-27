using System.Collections;
using UnityEngine;

public class MusicTimer : MonoBehaviour
{
    public PlayerScript player;
    public GameObject pendejoText;
    public float pendejoRate, pendejoChance;
    private Coroutine pendejoTimeCoroutine;


    private void Awake()
    {
        pendejoText.SetActive(false);
        pendejoTimeCoroutine = null;
    }

    private void Start()
    {
        InvokeRepeating("PendejearPlayer", pendejoRate, pendejoRate);
    }

    private void PendejearPlayer()
    {
        float randomNumber = Random.Range(1f, 100f);
        if (randomNumber <= pendejoChance)
        {
            player.OnSwitch();
            if (pendejoTimeCoroutine != null)
            {
                StopCoroutine(pendejoTimeCoroutine);
                pendejoTimeCoroutine = null;
            }
            pendejoTimeCoroutine = StartCoroutine(SayPendejo());
        }
    }

    private IEnumerator SayPendejo()
    {
        pendejoText.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        pendejoText.SetActive(false);
    }
}
