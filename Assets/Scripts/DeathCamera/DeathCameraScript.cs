using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DeathCameraScript : MonoBehaviour
{

    [SerializeField] private AudioClip[] _deathScreams;
    
    void Start()
    {

        AudioClip chosenScream = _deathScreams[Random.Range(0, _deathScreams.Length)];
        GetComponent<AudioSource>().PlayOneShot(chosenScream);

        // Handle the reload 
        Invoke(nameof(ReloadLevelAfterDeath), 3); 

    }

    private void ReloadLevelAfterDeath()
    {

        SaveEngine.LoadSaveFile();
        SaveEngine.ReloadWorldFromLoadedSave();

    }

}
