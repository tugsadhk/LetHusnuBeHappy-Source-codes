using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource MainAudioSource;

    public AudioClip[] AgentHitSounds;
    public AudioClip GameFinishSound;

    private float _timeCounter = 0;
    private bool _canOneShotPlay = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_canOneShotPlay)
        {
            _timeCounter += Time.deltaTime;

            if (_timeCounter >= GenericDataManager.OneShotSoundCooldownTime)
            {
                _timeCounter = 0;
                _canOneShotPlay = true;
            }
        }
    }

    public void OnAgentDead()
    {
        if (_canOneShotPlay && Random.Range(0f, 1f) <= 0.05f)
        {
            _canOneShotPlay = false;
            _timeCounter = 0;
            MainAudioSource.PlayOneShot(AgentHitSounds[Random.Range(0, AgentHitSounds.Length)]);
        }
    }

    public void OnGameFinished()
    {
        MainAudioSource.PlayOneShot(GameFinishSound);
    }
}