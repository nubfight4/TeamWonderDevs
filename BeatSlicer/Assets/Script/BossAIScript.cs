using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public static class ArrayExtensions
{
	public static T RandomItem<T>(this T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}
}

public class BossAIScript : MonoBehaviour
{
    [SerializeField]

    enum MovementPattern
    {
        MOVE_PATTERN_1 = 0,
        MOVE_PATTERN_2,
        MOVE_PATTERN_3A,
        MOVE_PATTERN_3B,
        MOVE_PATTERN_FALL,
        SET_MOVE_PATTERN
    };

    public GameObject player;

    public bool isEmergingUp = true;
    public bool isDisappearingDown = false;
    public bool isVanishing = false;

    public bool isFloatingActive = true;
    private bool bossFloatingBool;

    public float movementTimer;
    private float movementTimerTemp;

    private bool isMoving = false;

    //private string[] allBullets = {"Bullet Red", "Bullet Blue", "Bullet Green"};

    public List<GameObject> StageOnePoints;
    public List<GameObject> StageTwoPoints;

    private int selectedDestination;
    NavMeshAgent navMeshAgent;

    int[] bossStageOneMovement = new int[] {0, 1, 7}; // Removed 8, 9, 15

    int[] bossStageTwoMovementStart = new int[] {0, 2, 4, 6, 7, 8};
    int[] bossStageTwoMovementEnd = new int[] {1, 3, 5, 7, 6, 8};

    int[] bossStageThreeMovement = new int[] {7, 3, 5, 1, 16};

    MovementPattern currentMovementPattern = MovementPattern.SET_MOVE_PATTERN;


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
        movementTimerTemp = movementTimer;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }


	void Start()
	{
        navMeshAgent.baseOffset = -2.0f;
        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

        StartCoroutine(TempMovePatternTimer());

        Debug.Log("Current Move Pattern = " + currentMovementPattern);
        Debug.Log("Press 'J' to change Movement Pattern. This message will not repeat.");
    }


	void Update()
	{
        BossMovementFunction();

        TempMovePatternChangeButton(); // Temporary Movement Pattern Change Button 'J'
    }


    void FixedUpdate()
    {
        if(isFloatingActive == true)
        {
            BossFloatingFunction();
        }
    }

	
	void BossMovementFunction()
	{
        transform.LookAt(player.transform);
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles = new Vector3(0,eulerAngles.y,0);
        transform.rotation = Quaternion.Euler(eulerAngles);

        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
        {
            navMeshAgent.speed = 0;
        }
        else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
        {
            if(selectedDestination == 5)
            {
                navMeshAgent.speed = 0;

                if(isEmergingUp != true && currentMovementPattern != MovementPattern.MOVE_PATTERN_3B)
                {
                    movementTimerTemp -= Time.deltaTime;

                    if(movementTimerTemp <= 0)
                    {
                        movementTimerTemp = movementTimer;

                        isEmergingUp = true;
                    }
                }
            }
            else if(isMoving == false && isEmergingUp != true && isDisappearingDown != true && isVanishing != true)
            {
                transform.position = StageTwoPoints[bossStageTwoMovementStart[selectedDestination]].transform.position;

                isMoving = true;
            }
            else if(isMoving == true)
            {
                navMeshAgent.speed = 8;

                navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);

                float distance = Vector3.Distance(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position,transform.position);

                if(distance <= 3.0f)
                {
                    selectedDestination = selectedDestination + 1;

                    if(selectedDestination == 5)
                    {
                        transform.position = StageTwoPoints[bossStageTwoMovementStart[selectedDestination]].transform.position;
                        navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);
                    }

                    isMoving = false;
                }
            }
        }
    }


    void BossFloatingFunction()
    {
        if(isVanishing == true)
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                navMeshAgent.baseOffset = -2.0f;

                isDisappearingDown = true;
                isVanishing = false;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                navMeshAgent.baseOffset = -2.0f;

                selectedDestination = 0;

                isDisappearingDown = true;
                isVanishing = false;
            }
        }
        else if(isEmergingUp ==  true)
        {
            navMeshAgent.baseOffset += Time.deltaTime * 2.0f;

            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(navMeshAgent.baseOffset >= 1.5f)
                {
                    isEmergingUp = false;
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(navMeshAgent.baseOffset >= 11.0f && selectedDestination == 5)
                {
                    navMeshAgent.baseOffset = 4.0f;
                    selectedDestination = 0;

                    transform.position = StageTwoPoints[selectedDestination].transform.position;

                    isEmergingUp = false;
                }
                else if(navMeshAgent.baseOffset >= 4.5f && selectedDestination != 5)
                {
                    isEmergingUp = false;
                }
            }
        }
        else if(isDisappearingDown == true)
        {
            navMeshAgent.baseOffset -= Time.deltaTime * 2.0f;

            if(navMeshAgent.baseOffset <= -1.5f)
            {
                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
                {
                    selectedDestination = bossStageOneMovement[Random.Range(0,bossStageOneMovement.Length)];
                    transform.position = StageOnePoints[selectedDestination].transform.position;
                }
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    navMeshAgent.baseOffset = 4.0f;
                    selectedDestination = 0;
                    transform.position = StageTwoPoints[selectedDestination].transform.position;
                }
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    transform.position = StageOnePoints[bossStageThreeMovement[selectedDestination]].transform.position;
                }

                isEmergingUp = true;
                isDisappearingDown = false;
            }
        }
        else
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(navMeshAgent.baseOffset >= 2.0f)
                {
                    bossFloatingBool = false;
                }
                else if(navMeshAgent.baseOffset <= 1.2f)
                {
                    bossFloatingBool = true;
                }

                if(bossFloatingBool == true)
                {
                    navMeshAgent.baseOffset += Time.deltaTime * 0.25f;
                }
                else if(bossFloatingBool == false)
                {
                    navMeshAgent.baseOffset -= Time.deltaTime * 0.25f;
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(navMeshAgent.baseOffset >= 4.5f)
                {
                    bossFloatingBool = false;
                }
                else if(navMeshAgent.baseOffset <= 3.7f)
                {
                    bossFloatingBool = true;
                }

                if(bossFloatingBool == true)
                {
                    navMeshAgent.baseOffset += Time.deltaTime * 0.25f;
                }
                else if(bossFloatingBool == false)
                {
                    navMeshAgent.baseOffset -= Time.deltaTime * 0.25f;
                }
            }
        }
    }


    /// Temporary Functions Below

    
    void TempMovePatternChangeButton() // Press 'J' To Change Between Movement Patterns 
    {
         if(Input.GetKeyDown(KeyCode.J))
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                isDisappearingDown = true;
                isEmergingUp = false;
                isVanishing = false;
                isMoving = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                isDisappearingDown = false;
                isEmergingUp = false;
                isVanishing = true;
                isMoving = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                isDisappearingDown = true;
                isEmergingUp = false;
                isVanishing = false;
                isMoving = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

                isDisappearingDown = false;
                isEmergingUp = false;
                isVanishing = true;
                isMoving = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }


    private IEnumerator TempMovePatternTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(5,11));

            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                selectedDestination = bossStageOneMovement[Random.Range(0,bossStageOneMovement.Length)];
                navMeshAgent.SetDestination(StageOnePoints[selectedDestination].transform.position);

                isDisappearingDown = true;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                navMeshAgent.SetDestination(StageOnePoints[selectedDestination].transform.position);

                selectedDestination = selectedDestination + 1;

                if(selectedDestination == 5)
                {
                    selectedDestination = 0;
                }

                int randNum = Random.Range(0,6);

                if(randNum == 1)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                    Debug.Log("Current Move Pattern = " + currentMovementPattern);
                }

                isDisappearingDown = true;
            }
        }
    }
}
