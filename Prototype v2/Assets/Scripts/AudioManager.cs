using FMOD.Studio;
using UnityEngine;
using EventSystem;
using EventHandler = EventSystem.EventHandler;
using FMODUnity;

namespace Scrips
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        
        [SerializeField] private EventReference underscore;
        // [SerializeField] private float maxVolume = 0.6f;
        // [SerializeField] private float minVolume = 0;
        
        private EventHandler eventHandler;
        private GameManager gameManager;
        private EventInstance underscoreEventInstance;
        private GameObject currentTarget;
        private Vector3 targetPosition;
        
        //private Transform player;
        
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
            gameManager = GameManager.instance;

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
            eventHandler.RegisterListener<NewObjectiveEvent>(OnNewObjective);
            eventHandler.RegisterListener<GameEndEvent>(OnGameEnd);
        }

        private void OnDisable()
        {
            eventHandler.UnregisterListener<NewObjectiveEvent>(OnNewObjective);
            eventHandler.UnregisterListener<GameEndEvent>(OnGameEnd);
        }
        
        private void OnNewObjective(NewObjectiveEvent eventInfo)
        {
            currentTarget = eventInfo.newObjective;
            targetPosition = currentTarget.transform.position;
            print(eventInfo.Description);
        }

        private void OnGameEnd(GameEndEvent eventInfo)
        {
            underscoreEventInstance.setParameterByName("Panning", 0);
            underscoreEventInstance.setParameterByName("RotationVolume", 1);
        }

        private void Update()
        {
            // Get the player's 2D (XZ) position and facing direction
            Vector2 playerPosition = new Vector2(player.position.x, player.position.z);
            Vector2 playerDirection = new Vector2(player.forward.x, player.forward.z);
            
            // Get the objective's 2D (XZ) position
            Vector2 objectivePosition = new Vector2(targetPosition.x, targetPosition.z);

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
            
            // Set the FMOD parameters for stereo-panning and volune of the lead flute
            underscoreEventInstance.setParameterByName("Panning", pan);
            underscoreEventInstance.setParameterByName("RotationVolume", volume);
        }
    }
}
