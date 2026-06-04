using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Связи")]
    public PlayerInventory playerInventory; // Ссылка на скрипт игрока
    public GameObject monster;              // Ссылка на объект монстра
    private GameObject player;              // Ссылка на самого игрока (для дистанции)

    [Header("Звуковое сопровождение")]
    public AudioSource backgroundMusic;     // Перетащи сюда твой объект с музыкой
    [Range(0f, 1f)]
    public float startVolume = 0.1f;        // Начальная громкость (при 0 мусора)
    [Range(0f, 1f)]
    public float maxVolume = 1.0f;          // Максимальная громкость (при 5 мусора)

    [Header("Настройки")]
    public int maxTrash = 5;                // Сколько нужно собрать мусора
    public float interactDistance = 3.0f;   // Дистанция взаимодействия

    [Header("Интерфейс")]
    public GameObject hintE;                // Подсказка "Нажми E"
    public GameObject hintNeedTrash;        // Подсказка "Нужно найти мусор!"

    private int currentTrashCount = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (monster != null) monster.SetActive(false);
        if (hintE != null) hintE.SetActive(false);
        if (hintNeedTrash != null) hintNeedTrash.SetActive(false);

        // Устанавливаем начальную тихую громкость для атмосферы
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = startVolume;
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist <= interactDistance)
        {
            if (hintE != null) hintE.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerInventory.currentItem != null)
                {
                    DisposeTrash();
                    if (hintNeedTrash != null) hintNeedTrash.SetActive(false);
                }
                else
                {
                    if (hintNeedTrash != null) hintNeedTrash.SetActive(true);
                }
            }
        }
        else
        {
            if (hintE != null) hintE.SetActive(false);
            if (hintNeedTrash != null) hintNeedTrash.SetActive(false);
        }
    }

    private void DisposeTrash()
    {
        Destroy(playerInventory.currentItem.gameObject);
        playerInventory.currentItem = null;

        currentTrashCount++;
        Debug.Log("Мусор выброшен! Текущий счет: " + currentTrashCount + " / " + maxTrash);

        // МЕНЯЕМ ГРОМКОСТЬ
        if (backgroundMusic != null)
        {
            // Формула плавно поднимает громкость от startVolume до maxVolume на основе текущего прогресса
            float progress = (float)currentTrashCount / maxTrash;
            backgroundMusic.volume = Mathf.Lerp(startVolume, maxVolume, progress);
        }

        if (currentTrashCount >= maxTrash)
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        if (monster != null && !monster.activeSelf)
        {
            monster.SetActive(true);
            Debug.Log("Монстр заспавнился!");
        }
    }
}