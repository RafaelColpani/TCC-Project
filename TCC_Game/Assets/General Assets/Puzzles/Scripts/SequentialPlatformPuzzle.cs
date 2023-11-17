using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using System.Linq;
using static UnityEditor.PlayerSettings;

#region STRUCTS
[System.Serializable]
public struct IntegerPoint
{
    [Range(0, 19)] public int x;
    [Range(0, 19)] public int y;

    public IntegerPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[System.Serializable]
public class CollectibleGrid
{
    [HideInInspector] public GameObject collectibleObject;
    public IntegerPoint gridPosition;
    public int stepCount;

    [HideInInspector] public List<GameObject> StepIndicators = new List<GameObject>();
    [HideInInspector] public int fixedStepCount;
}

public class PlatformGrid
{
    public enum PlatformType
    {
        common, initial, final, trap
    }

    public GameObject platform;
    public IntegerPoint gridPosition;
    public PlatformType type;
    public bool visited;
    public bool isTrap;

    public PlatformGrid(GameObject platform, IntegerPoint gridPosition, PlatformType type = PlatformType.common)
    {
        this.platform = platform;
        this.gridPosition = gridPosition;
        this.type = type;
        this.visited = false;
        this.isTrap = false;
    }
}
#endregion

public class SequentialPlatformPuzzle : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- GRID CONSTRUCTOR -", (int)HeaderPlusColor.green)]
    [Tooltip("The prefab of each platform that will compose the grid")]
    [SerializeField] GameObject platformPrefab;
    [Tooltip("The number of rows that the grid have")]
    [SerializeField][Range(1, 20)] int rowsCount;
    [Tooltip("The number of columns that the grid have")]
    [SerializeField][Range(1, 20)] int columnsCount;
    [Tooltip("The position of the first platform (0x0) in grid. All others platforms will guide its position based on this")]
    [SerializeField] Vector3 firstPlatformPosition;
    [Tooltip("The distance that the platforms will have from each other. X is the distance in horizontal. Y is the distance in Vertical.")]
    [SerializeField] Vector2 platformDistances;
    [Tooltip("The time that the grid will respawn after loosing the puzzle")]
    [SerializeField] float gridRespawnTime;
    [Tooltip("Tells if want to construct the platforms on the awake of this script")]
    [SerializeField] bool constructOnAwake;

    [HeaderPlus(" ", "- INITIAL & FINAL PLATFORMS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The prefab of the initial platform of the puzzle. Keep it null to use the platformPrefab")]
    [SerializeField] GameObject initialPrefab;
    [Tooltip("The prefab of the final platform of the puzzle. Keep it null to use the plaformPrefab")]
    [SerializeField] GameObject finalPrefab;
    [Tooltip("The position that the inital platform will be in the grid")]
    [SerializeField] IntegerPoint initialPositionInGrid;
    [Tooltip("The position that the final platform will be in the grid")]
    [SerializeField] IntegerPoint finalPositionInGrid;

    [HeaderPlus(" ", "- TRAP PLATFORMS -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The prefab of the trap platform of the puzzle. Keep it null to use the platformPrefab")]
    [SerializeField] GameObject trapPrefab;
    [Tooltip("The prefab of the enemy that will be on trap platforms")]
    [SerializeField] GameObject trapEnemyPrefab;
    [Tooltip("The position that the enemy will be relative to its parent platform")]
    [SerializeField] Vector3 trapEnemyLocalPosition;
    [Tooltip("The position that the traps platforms will be in the grid. Leave it empty for no traps")]
    [SerializeField] List<IntegerPoint> trapsPositionInGrid;

    [HeaderPlus(" ", "- COLLECTIBLES -", (int)HeaderPlusColor.red)]
    [Tooltip("The prefab of the collectible")]
    [SerializeField] GameObject collectiblePrefab;
    [Tooltip("The prefab of the step indicator (crystal)")]
    [SerializeField] GameObject stepIndicatorPrefab;
    [Tooltip("The position that the collectible will be relative to its parent platform")]
    [SerializeField] Vector3 collectibleLocalPosition;
    [Tooltip("Define the position of each collectible and if is even (for SetActive swap). The Count of this list is the number of collectibles.")]
    [SerializeField] List<CollectibleGrid> collectiblesGrid;
    [Tooltip("The number of player's grid steps that will swap the SetActives of collectibles isEven true and false")]
    [SerializeField] [Range(1, 10)]int swapCollectiblesLimit;
    [Tooltip("The offset in Y position that the step indicators will be relative to its collectible.")]
    [SerializeField] float stepIndicatorYOffset;
    [Tooltip("The distance that the step indicators will have from each other")]
    [SerializeField] float stepIndicatorSpacing;
    #endregion

    #region Private Vars
    private List<List<PlatformGrid>> platformsGrid;
    private List<GameObject> enemyTraps;
    private PlatformGrid previousPlatformVisited = null;

    private ProceduralLegs playerLegs;

    private bool gridConstructed = false;

    private int collectCollectiblesCount = 0;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        foreach (var collectible in collectiblesGrid)
            collectible.fixedStepCount = collectible.stepCount;

        if (constructOnAwake)
            ConstructGrid();
    }

    private void Start()
    {
        playerLegs = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ProceduralLegs>();
    }

    private void Update()
    {
        if (PauseController.GetIsPaused()) return;
        if (!gridConstructed) return;

        // is making the puzzle -----
        var stepedPlatform = GetStepedPlatform();

        if (stepedPlatform == null) return;

        if (!IsInsideStepRules(stepedPlatform))
            LostPuzzle();

        if (!IsFinishedPuzzle()) return;

        // finished the puzzle try -----
        // lost
        if (stepedPlatform.type != PlatformGrid.PlatformType.final || collectCollectiblesCount < collectiblesGrid.Count())
        {
            LostPuzzle();
            return;
        }

        WonPuzzle();
    }
    #endregion

    #region Private Methods
    private PlatformGrid GetStepedPlatform()
    {
        if (playerLegs.GetGroundedObject() == null)
            return null;

        var groundedObject = playerLegs.GetGroundedObject();

        foreach (var row in platformsGrid)
            foreach (var platform in row)
            {
                if (platform.platform == groundedObject)
                {
                    //print($"STEPED === ROW: {platform.gridPosition.x} | COLUMN: {platform.gridPosition.y} | TYPE: {platform.type}");
                    return platform;
                }
            }

        return null;
    }

    private bool IsInsideStepRules(PlatformGrid stepedPlatform)
    {
        // pisou numa plataforma já visitada = PERDEU
        if (stepedPlatform.visited && previousPlatformVisited != stepedPlatform)
            return false;

        // comecou sem ser da plataforma inicial = PERDEU
        if (previousPlatformVisited == null && stepedPlatform.type != PlatformGrid.PlatformType.initial)
            return false;

        // pisou na plataforma final sem ter terminado = PERDEU
        if (stepedPlatform.type == PlatformGrid.PlatformType.final && !IsFinishedPuzzle(true))
            return false;

        // comecou na plataforma inicial = CONTINUA
        if (previousPlatformVisited == null && stepedPlatform.type == PlatformGrid.PlatformType.initial)
            return IsCorrectStep(stepedPlatform);

        // andou na horizontal = CONTINUA
        if ((stepedPlatform.gridPosition.x == previousPlatformVisited.gridPosition.x - 1 ||
             stepedPlatform.gridPosition.x == previousPlatformVisited.gridPosition.x + 1) &&
             stepedPlatform.gridPosition.y == previousPlatformVisited.gridPosition.y)
                return IsCorrectStep(stepedPlatform);

        // andou na VERTICAL = CONTINUA
        if ((stepedPlatform.gridPosition.y == previousPlatformVisited.gridPosition.y - 1 ||
             stepedPlatform.gridPosition.y == previousPlatformVisited.gridPosition.y + 1) &&
             stepedPlatform.gridPosition.x == previousPlatformVisited.gridPosition.x)
                return IsCorrectStep(stepedPlatform);

        // nao saiu do lugar = CONTINUA
        if (stepedPlatform == previousPlatformVisited)
            return true;

        return false;
    }

    private void SwapCollectiblesActive(CollectibleGrid collectible, bool isActive)
    {
        if (collectible.collectibleObject != null)
        {
            if (collectible.stepCount >= collectible.fixedStepCount)
                collectible.stepCount = 0;

            collectible.collectibleObject.SetActive(isActive);
        }
    }

    private bool IsFinishedPuzzle(bool excludeFinal = false)
    {
        foreach (var row in platformsGrid)
            foreach (var platform in row)
            {
                if (platform.type == PlatformGrid.PlatformType.final && excludeFinal)
                    continue;

                if (!platform.visited)
                    return false;
            }

        return true;
    }

    private void ResetPuzzleParameters()
    {
        gridConstructed = false;
        previousPlatformVisited = null;
        collectCollectiblesCount = 0;

        foreach (var collectible in collectiblesGrid)
        {
            collectible.stepCount = -1;
            collectible.StepIndicators.RemoveRange(0, collectible.StepIndicators.Count());
        }
    }

    private void LostPuzzle()
    {
        foreach (var row in platformsGrid)
            foreach (var platform in row)
            {
                platform.visited = false;
                platform.isTrap = false;
                Destroy(platform.platform);
            }

        foreach (var enemy in enemyTraps)
            Destroy(enemy);

        foreach (var collectible in collectiblesGrid)
        {
            Destroy(collectible.collectibleObject);
            collectible.collectibleObject = null;
        }

        foreach (var collectible in collectiblesGrid)
            Destroy(collectible.collectibleObject);

        ResetPuzzleParameters();

        StartCoroutine(RestartPuzzle());
    }

    private void WonPuzzle()
    {
        ResetPuzzleParameters();
        print("EBA, VOCÊ VENCEU");
    }
    #endregion

    #region Public Methods
    public void ConstructGrid()
    {
        if (HaveError())
            return;

        platformsGrid = new List<List<PlatformGrid>>();
        enemyTraps = new List<GameObject>();

        var previousPlatoformPosition = Vector3.zero;
        var platformsRow = new List<PlatformGrid>();

        // spawn platforms
        for (int row = 0; row < rowsCount; row++)
        {
            for (int column = 0; column < columnsCount; column++)
            {
                var platform = Instantiate(GetInstantiatedPlatform(row, column), this.transform);
                var platformGrid = new PlatformGrid(platform, new IntegerPoint(row, column));

                // add trap
                if (IsTrap(platformGrid.gridPosition.x, platformGrid.gridPosition.y))
                {
                    platformGrid.visited = true;
                    platformGrid.isTrap = true;

                    var enemy = Instantiate(trapEnemyPrefab, platform.transform);
                    enemyTraps.Add(enemy);

                    enemy.transform.localScale = SetChildScale(platform.transform, enemy.transform);
                    enemy.transform.localPosition = trapEnemyLocalPosition;
                }

                // add collectible
                if (GetCollectibleIndex(platformGrid.gridPosition.x, platformGrid.gridPosition.y) > -1)
                {
                    int index = GetCollectibleIndex(platformGrid.gridPosition.x, platformGrid.gridPosition.y);
                    collectiblesGrid[index].collectibleObject = Instantiate(collectiblePrefab, platform.transform);

                    var collectibleScale = collectiblesGrid[index].collectibleObject.transform.localScale;
                    var platformScale = platform.transform.localScale;
                    collectiblesGrid[index].collectibleObject.transform.localScale = new Vector3(collectibleScale.x / platformScale.x,
                                                                                                 collectibleScale.y / platformScale.y,
                                                                                                 1);
                    collectiblesGrid[index].collectibleObject.transform.localPosition = collectibleLocalPosition;
                    collectiblesGrid[index].stepCount = -1;

                    collectiblesGrid[index].collectibleObject.AddComponent<GridCollectible>();
                    collectiblesGrid[index].collectibleObject.GetComponent<GridCollectible>().AddCollectibleGrid(collectiblesGrid[index]);
                    collectiblesGrid[index].collectibleObject.AddComponent<BoxCollider2D>();

                    collectiblesGrid[index].collectibleObject.GetComponent<BoxCollider2D>().isTrigger = true;

                    // spawn step indicators
                    for (int i = 0; i < collectiblesGrid[index].fixedStepCount; i++)
                    {
                        var stepIndicator = Instantiate(stepIndicatorPrefab, platform.transform);
                        stepIndicator.transform.localScale = SetChildScale(platform.transform, stepIndicator.transform);

                        var pos = collectiblesGrid[index].collectibleObject.transform.localPosition;
                        stepIndicator.transform.localPosition = new Vector3(pos.x, pos.y + stepIndicatorYOffset, pos.z);

                        collectiblesGrid[index].StepIndicators.Add(stepIndicator);
                    }

                    collectiblesGrid[index].collectibleObject.SetActive(false);
                }

                // PLATFORM POSITIONS CHECKERS -------
                if (row == 0 && column == 0)
                    platform.transform.position = firstPlatformPosition;

                else if (column == 0)
                {
                    platform.transform.position = new Vector3(firstPlatformPosition.x,
                                                              previousPlatoformPosition.y - platformDistances.y,
                                                              previousPlatoformPosition.z);
                }

                else
                {
                    platform.transform.position = new Vector3(previousPlatoformPosition.x + platformDistances.x,
                                                              previousPlatoformPosition.y,
                                                              previousPlatoformPosition.z);
                }

                // PLATFORM TYPE CHECKERS -------
                if (row == initialPositionInGrid.x && column == initialPositionInGrid.y)
                    platformGrid.type = PlatformGrid.PlatformType.initial;
                else if (row == finalPositionInGrid.x && column == finalPositionInGrid.y)
                    platformGrid.type = PlatformGrid.PlatformType.final;

                platformsRow.Add(platformGrid);
                previousPlatoformPosition = platform.transform.position;
            }

            platformsGrid.Add(platformsRow);
            platformsRow = new List<PlatformGrid>();
        }

        AlignStepIndicatorsPosition();

        //foreach (var row in platformsGrid)
        //    foreach (var platform in row)
        //    {
        //        print($"ROW: {platform.gridPosition.x} | COLUMN: {platform.gridPosition.y} | TYPE: {platform.type}");
        //    }

        gridConstructed = true;
    }

    public void CollectedCollectible(CollectibleGrid collectableGrid)
    {
        collectCollectiblesCount++;

        Destroy(collectableGrid.collectibleObject);
        foreach (var indicator in collectableGrid.StepIndicators)
            Destroy(indicator);

        collectableGrid.StepIndicators.RemoveRange(0, collectableGrid.StepIndicators.Count());
    }
    #endregion

    #region COROUTINES
    private IEnumerator RestartPuzzle()
    {
        yield return new WaitForSeconds(gridRespawnTime);

        ConstructGrid();
    }
    #endregion

    #region AUX METHODS
    private GameObject GetInstantiatedPlatform(int row, int column)
    {
        if (initialPrefab != null && row == initialPositionInGrid.x && column == initialPositionInGrid.y)
            return initialPrefab;

        else if (finalPrefab != null && row == finalPositionInGrid.x && column == finalPositionInGrid.y)
            return finalPrefab;

        else if (trapPrefab != null && IsTrap(row, column))
            return trapPrefab;

        else
            return platformPrefab;
    }

    private bool IsTrap(int row, int column)
    {
        foreach (var position in trapsPositionInGrid)
            if (position.x == row && position.y == column)
                return true;

        return false;
    }

    private int GetCollectibleIndex(int row, int column)
    {
        for (int i = 0; i < collectiblesGrid.Count(); i++)
            if (collectiblesGrid[i].gridPosition.x == row && collectiblesGrid[i].gridPosition.y == column)
                return i;

        return -1;
    }

    private bool IsCorrectStep(PlatformGrid stepedPlatform)
    {
        previousPlatformVisited = stepedPlatform;
        stepedPlatform.visited = true;

        //TODO: Troca de shader na plataforma pisada, o GameObject esta armazenado em stepedPlatform.platform;
        print("TROCA O SHADER DA PLATAFORMA PISADA AQUI");
        var platformSR = stepedPlatform.platform.GetComponent<SpriteRenderer>();
        platformSR.color = Color.gray;

        //TODO: Troca de shader no cristal
        print("TROCA O SHADER DO CRISTAL AQUI");
        foreach (var collectible in collectiblesGrid)
        {
            if (collectible.StepIndicators.Count() <= 0) continue;

            if (collectible.stepCount == 0)
            {
                SwapCollectiblesActive(collectible, false);

                foreach (var indicator in collectible.StepIndicators)
                {
                    if (indicator == collectible.StepIndicators[0]) continue;
                    var indicatorSR = indicator.GetComponent<SpriteRenderer>();
                    indicatorSR.color = Color.white;
                }
            }

            if (collectible.stepCount >= 0)
            {
                var stepIndicatorSR = collectible.StepIndicators[collectible.stepCount].GetComponent<SpriteRenderer>();
                stepIndicatorSR.color = Color.red;
            }

            if (++collectible.stepCount >= collectible.fixedStepCount)
                SwapCollectiblesActive(collectible, true);

        }

        return true;
    }

    private Vector3 SetChildScale(Transform parent, Transform child)
    {
        return child.transform.localScale = new Vector3(child.localScale.x / parent.localScale.x, child.localScale.y / parent.localScale.y, 1);
    }

    private void AlignStepIndicatorsPosition()
    {
        foreach (var collectible in collectiblesGrid)
        {
            float midIndex = (collectible.StepIndicators.Count() - 1) / 2f;

            for (int i = 0; i < collectible.StepIndicators.Count(); i++)
            {
                if (i == midIndex) continue;
                
                float spacing = (stepIndicatorSpacing * (midIndex - i));
                var pos = collectible.StepIndicators[i].transform.localPosition;

                collectible.StepIndicators[i].transform.localPosition = new Vector3(pos.x + spacing, pos.y, pos.z);
            }

            collectible.StepIndicators.Reverse();
        }
    }
    #endregion

    //////////////////////////////////////

    #region ERRORS
    private bool HaveError()
    {
        var isError = false;

        if (platformPrefab == null)
        {
            Debug.LogError("GRID PUZZLE: platformPrefab CANNOT be null");
            isError = true;
        }

        if (rowsCount <= 0)
        {
            Debug.LogError("GRID PUZZLE: Number of rows must be greater than 0");
            isError = true;
        }

        if (columnsCount <= 0)
        {
            Debug.LogError("GRID PUZZLE: Number of columns must be grater than 0");
            isError = true;
        }

        if (initialPositionInGrid.x < 0)
        {
            Debug.LogError("GRID PUZZLE: The Horizontal (x) position of the initial platform must be positive");
            isError = true;
        }

        if (finalPositionInGrid.x < 0)
        {
            Debug.LogError("GRID PUZZLE: The Horizontal (x) position of the final platform must be positive");
            isError = true;
        }

        if (initialPositionInGrid.y < 0)
        {
            Debug.LogError("GRID PUZZLE: The Vertical (y) position of the initial platform must be positive");
            isError = true;
        }

        if (finalPositionInGrid.y < 0)
        {
            Debug.LogError("GRID PUZZLE: The Vertical (y) position of the final platform must be positive");
            isError = true;
        }

        if (initialPositionInGrid.x >= rowsCount)
        {
            Debug.LogError("GRID PUZZLE: The Maximum Horizontal (x) position of the initial platform must be lower than rowsCount");
            isError = true;
        }

        if (finalPositionInGrid.x >= rowsCount)
        {
            Debug.LogError("GRID PUZZLE: The Maximum Horizontal (x) position of the final platform must be lower than rowsCount");
            isError = true;
        }

        if (initialPositionInGrid.y >= columnsCount)
        {
            Debug.LogError("GRID PUZZLE: The Maximum Vertical (y) position of the initial platform must be lower than columnsCount");
            isError = true;
        }

        if (finalPositionInGrid.y >= columnsCount)
        {
            Debug.LogError("GRID PUZZLE: The Maximum Vertical (y) position of the final platform must be lower than columnsCount");
            isError = true;
        }

        for (int i = 0; i < collectiblesGrid.Count() - 1; i++)
            for (int j = i+1; j < collectiblesGrid.Count(); j++)
                if (collectiblesGrid[i].gridPosition.x == collectiblesGrid[j].gridPosition.x &&
                    collectiblesGrid[i].gridPosition.y == collectiblesGrid[j].gridPosition.y)
                {
                    Debug.LogError("GRID PUZZLE: Cannot have two or more collectibles in the same grid position");
                    isError = true;
                }

        return isError;
    }
    #endregion
}
