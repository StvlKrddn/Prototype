using FMOD.Studio;
using UnityEngine;
using EventSystem;
using EventHandler = EventSystem.EventHandler;
using FMODUnity;

namespace Scrips
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private GameObject objective;
        [SerializeField] private Transform player;
        
        [SerializeField] private EventReference underscore;
        // [SerializeField] private float maxVolume = 0.6f;
        // [SerializeField] private float minVolume = 0;
        
        private EventHandler eventHandler;
        // private GameManager gameManager;
        // private PlayerState playerState;
        private EventInstance underscoreEventInstance;
        
        //private Transform player;
        //private Vector3 objectivePosition;
        
        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one AudioManager in the scene!");
            }
            else
            {
                instance = this;
            }

            eventHandler = EventHandler.instance;
            // playerState = PlayerState.instance;
            // gameManager = GameManager.instance;

            eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnNewObjective);
            
            // player = playerState.gameObject.transform;
            
            // GameObject firstObjective = gameManager.objectives.Peek();
            
            // eventHandler.InvokeEvent(new NewObjectiveEvent(
            //     newObjectiveNode: firstObjective,
            //     description: "First objective: " + firstObjective.GameObject.name
            // ));
            
            // Create an FMOD event instance that Unity can manipulate
            underscoreEventInstance = RuntimeManager.CreateInstance(underscore);
            
            // Get the current playback state of the event instance
            underscoreEventInstance.getPlaybackState(out PLAYBACK_STATE playbackState);
            
            // If the event instance is not currently playing...
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                // Play the event instance
                underscoreEventInstance.start();
            }
        }
        
        private void OnEnable()
        {
            eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnNewObjective);
        }

        private void OnDisable()
        {
            eventHandler.UnregisterListener<ObjectiveCompleteEvent>(OnNewObjective);
        }
        
        private void OnNewObjective(ObjectiveCompleteEvent completeEventInfo)
        {
            // currentObjective = gameManager.GetCurrentObjective();
            // if (currentObjective == null)
            // {
            //     // NO MORE OBJECTIVES
            //     return;
            // }
            // objectivePosition = currentObjective.GameObject.transform.position;
        }

        private void Update()
        {
            // Get the player's position
            Vector2 playerPosition = new Vector2(player.position.x, player.position.z);
            Vector2 playerDirection = new Vector2(player.forward.x, player.forward.z);

            Vector2 objectivePosition = new Vector2(objective.transform.position.x, objective.transform.position.z);

            // Create a normalised vector between the player's and the current objective's position
            Vector2 objectiveDirection = objectivePosition - playerPosition;

            // Calculate angle between objective and the player's direction
            var angle = Vector2.Angle(objectiveDirection, playerDirection);
                
            // Determine if the target is to the left or right of the player
            var direction = Vector2.Dot(objectiveDirection, new Vector2(player.right.x, player.right.z)) > 0 ? 1 : -1;
                
            // Calculate the pan value based on the angle and direction
            var pan = Mathf.Lerp(0, direction, angle / 90f);
                
            // Calculate the volume based on the angle
            var volume = Mathf.Lerp(0, 1, Mathf.Abs(angle - 180f) / 90f);
            
            // audioSource.panStereo = pan;
            // audioSource.volume = volume;
            underscoreEventInstance.setParameterByName("Panning", pan);
            underscoreEventInstance.setParameterByName("RotationVolume", volume);
        }
    }
}
