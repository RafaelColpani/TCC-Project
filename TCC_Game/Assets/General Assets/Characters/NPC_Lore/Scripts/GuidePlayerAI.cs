using UnityEngine;
using KevinCastejon.MoreAttributes;

[RequireComponent(typeof(CharacterManager))]
[RequireComponent(typeof(ProceduralLegs))]
public class GuidePlayerAI : MonoBehaviour
{
    private enum GuidePlayerState
    {
        waiting, guiding, reached
    }

    #region Inspector Vars
    [HeaderPlus(" ", "- STATE -", (int)HeaderPlusColor.green)]
    [Tooltip("The actual state of the AI")]
    [SerializeField][ReadOnly] private GuidePlayerState state;

    [HeaderPlus(" ", "- PLAYER -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The players body Transform, for the AI to check the distance to the player")]
    [SerializeField] Transform playerBody;

    [HeaderPlus(" ", "- MOVEMENT -", (int)HeaderPlusColor.white)]
    [Tooltip("The speed that the AI will walk")]
    [SerializeField] float walkSpeed;

    [HeaderPlus(" ", "- WAIT STATE -", (int)HeaderPlusColor.red)]
    [Tooltip("The minimum distance that the AI have to be away from player to wait for him")]
    [SerializeField] float distanceToWaitPlayer;

    [HeaderPlus(" ", "- GUIDING STATE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The minimum distance that the AI have to be from player to guide him")]
    [SerializeField] float distanceToGuidePlayer;
    

    [HeaderPlus(" ", "- REACHED STATE -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The position in the world where considered that the AI reached your destination")]
    [SerializeField] Vector3 reachedPosition;
    #endregion

    #region Private Vars
    private MoveCommand moveCommand;

    private ProceduralLegs proceduralLegs;
    private CharacterManager characterManager;

    private Transform body;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        body = characterManager.Body;
        proceduralLegs = GetComponent<ProceduralLegs>();

        moveCommand = new MoveCommand(proceduralLegs, walkSpeed);
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        state = SetState();
        ExecuteState();
    }
    #endregion

    #region Private Methods
    #region STATE MACHINE
    private GuidePlayerState SetState()
    {
        var distanceToPlayer = (body.transform.position - playerBody.position).sqrMagnitude;
        var distanceToFinalDestination = (body.transform.position - reachedPosition).sqrMagnitude;

        // reached destination
        if (distanceToFinalDestination <= 0.1f || state == GuidePlayerState.reached)
            return GuidePlayerState.reached;

        // waiting player to approach
        else if (distanceToPlayer > distanceToWaitPlayer)
            return GuidePlayerState.waiting;

        // guide player to destination
        else if (distanceToPlayer <= distanceToGuidePlayer)
            return GuidePlayerState.guiding;

        else
            return state;
    }

    private void ExecuteState()
    {
        switch (state)
        {
            // will wait for player or finished action
            case GuidePlayerState.waiting:
                LookToPlayer();
                break;
            
            case GuidePlayerState.reached:
                LookToPlayer();
                break;

            // will guide player
            case GuidePlayerState.guiding:
                ExecuteGuiding();
                break;
        }
    }
    #endregion

    #region STATE ACTIONS
     private void ExecuteGuiding()
    {
        int value = 1;

        if (body.position.x > reachedPosition.x)
            value = -1;

        moveCommand.Execute(this.transform, value);
    }

    private void LookToPlayer()
    {
        int value = 1;
        //   if object to look is in left and is facing to right direction || if object to look is in right and is facing to left direction
        if ((body.position.x > playerBody.position.x &&  characterManager.IsFacingRight()) || 
            (body.position.x < playerBody.position.x && !characterManager.IsFacingRight()))
            value = -1;

        this.transform.localScale = new Vector3(this.transform.localScale.x * value, this.transform.localScale.y, this.transform.localScale.z);
        moveCommand.SetIsFacingRight(characterManager.IsFacingRight());
        moveCommand.CurrentSpeed = 0;
    }
    #endregion
    #endregion

    #region Public Methods
    public MoveCommand GetMoveCommand()
    {
        return this.moveCommand;
    }
    #endregion
}
