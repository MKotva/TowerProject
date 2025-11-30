using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts;
using Assets.Scripts.RoomControllerScripts;

public class CombatController : MonoBehaviour
{
    [SerializeField] EnemyRoom roomController;

    [Header("Actors")]
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    [Header("Arena")]
    [SerializeField] private RectTransform arenaRect;
    [SerializeField] private float moveStep = 80f;
    [SerializeField] private float jumpStepX = 120f;
    [SerializeField] private float jumpStepY = 80f;

    [Header("Stamina / Cover")]
    [SerializeField] private double coverStaminaCost = 20.0;

    [Header("UI (optional)")]
    [SerializeField] private Button[] playerActionButtons;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip blockClip;
    [SerializeField] private AudioClip missClip;
    [SerializeField] private AudioClip stepClip;
    [SerializeField] private AudioClip potionDrink;
    [SerializeField] private AudioClip Death;

    [Header("Events")]
    public UnityEvent onEnemyDefeated;

    private RectTransform playerRect;
    private RectTransform enemyRect;

    private bool isPlayerTurn = true;
    private bool combatEnded = false;

    private void Awake()
    {
        playerRect = player.GetComponent<RectTransform>();
        enemyRect = enemy.GetComponent<RectTransform>();
    }

    private void Start()
    {
        BeginPlayerTurn();

        Copy(GameManager.Instance.PlayerController, this.player);
    }

    private void Copy(PlayerController pk, PlayerController pl)
    {
        pl.Items = pk.Items;
        pl.StaminaPotions = pk.StaminaPotions;
        pl.HP = pk.HP;
        pl.Lives = pk.Lives;
        pl.HPPotions = pk.HPPotions;
        pl.StaminaPotions = pk.StaminaPotions;
        pl.Weapon = pk.Weapon;
        pl.Armor = pk.Armor;
    }

    private void BeginPlayerTurn()
    {
        if (combatEnded) return;
        isPlayerTurn = true;
        player.ExitCover();

        SetButtonsInteractable(true);
    }

    private void EndPlayerTurn()
    {
        SetButtonsInteractable(false);

        if (CheckCombatEnd()) return;

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        isPlayerTurn = false;
        enemy.ExitCover();

        yield return new WaitForSeconds(0.4f);

        EnemyChooseAction();

        if (CheckCombatEnd()) yield break;

        yield return new WaitForSeconds(0.4f);

        BeginPlayerTurn();
    }

    private bool CheckCombatEnd()
    {
        if (player.Lives <= 0)
        {
            combatEnded = true;
            return true;
        }

        if (enemy.IsDead)
        {
            combatEnded = true;
            roomController.Finish();
            enemy.enabled = false;
            Copy(this.player, GameManager.Instance.PlayerController);
            PlayDeathSfx();
            onEnemyDefeated?.Invoke();
            return true;
        }

        return false;
    }

    private void SetButtonsInteractable(bool value)
    {
        if (playerActionButtons == null) return;
        foreach (var b in playerActionButtons)
        {
            if (b != null) b.interactable = value;
        }
    }
    public void OnPlayerAttack()
    {
        if (!isPlayerTurn || combatEnded) return;

        TryAttack(player, playerRect, enemy, enemyRect);

        EndPlayerTurn();
    }

    public void OnPlayerCover()
    {
        if (!isPlayerTurn || combatEnded) return;
        player.EnterCover(coverStaminaCost);
        EndPlayerTurn();
    }

    public void OnPlayerMoveLeft()
    {
        if (!isPlayerTurn || combatEnded) return;

        MoveHorizontal(playerRect, -moveStep);
        EndPlayerTurn();
    }

    public void OnPlayerMoveRight()
    {
        if (!isPlayerTurn || combatEnded) return;

        MoveHorizontal(playerRect, moveStep);
        EndPlayerTurn();
    }

    public void OnPlayerJumpLeft()
    {
        if (!isPlayerTurn || combatEnded) return;

        Jump(playerRect, -jumpStepX);
        EndPlayerTurn();
    }

    public void OnPlayerJumpRight()
    {
        if (!isPlayerTurn || combatEnded) return;

        Jump(playerRect, jumpStepX);
        EndPlayerTurn();
    }

    public void OnPlayerDrinkHpPotion()
    {
        if (!isPlayerTurn || combatEnded) return;

        if (player.HPPotions > 0)
        {
            player.HPPotions--;
            player.HP = Mathf.Min(
                (float) player.MaxLives * (float) player.LiveHP,
                (float) player.HP + (float) player.LiveHP
            );
        }

        PlayPotionSfx();
        EndPlayerTurn();
    }

    public void OnPlayerDrinkStaminaPotion()
    {
        if (!isPlayerTurn || combatEnded) return;

        if (player.StaminaPotions > 0)
        {
            player.StaminaPotions--;
            player.Endurance = Mathf.Min(
                (float) player.MaxEndurance,
                (float) (player.Endurance + 40.0)
            );
        }

        PlayPotionSfx();
        EndPlayerTurn();
    }

    private void EnemyChooseAction()
    {
        float distance = Mathf.Abs(playerRect.anchoredPosition.x - enemyRect.anchoredPosition.x);
        bool inRange = distance <= enemy.Weapon.Range;
        if (enemy.HP < enemy.LiveHP * 0.5 && enemy.HPPotions > 0)
        {
            enemy.HPPotions--;
            enemy.HP = Mathf.Min((float)(enemy.MaxLives * enemy.LiveHP),
                                 (float)(enemy.HP + enemy.LiveHP));
            Debug.Log("Enemy drinks HP potion");
            return;
        }

        bool losing = enemy.HP < player.HP;
        if (losing && enemy.Endurance >= coverStaminaCost && Random.value < 0.7f)
        {
            enemy.EnterCover(coverStaminaCost);
            Debug.Log("Enemy takes cover");
            return;
        }

        if (inRange)
        {
            TryAttack(enemy, enemyRect, player, playerRect);
            Debug.Log("Enemy attacks");
            return;
        }

        float dir = ( playerRect.anchoredPosition.x < enemyRect.anchoredPosition.x ) ? -1f : 1f;

        if (Random.value < 0.3f)
        {
            Jump(enemyRect, jumpStepX * dir);
            Debug.Log("Enemy jumps");
        }
        else
        {
            MoveHorizontal(enemyRect, moveStep * dir);
            Debug.Log("Enemy moves");
        }
    }


    private void TryAttack(Entity attacker, RectTransform attackerRect, Entity defender, RectTransform defenderRect)
    {
        float distance = Mathf.Abs(attackerRect.anchoredPosition.x - defenderRect.anchoredPosition.x);
        if (distance > attacker.Weapon.Range * 50)
        {
            Debug.Log("Attack missed (out of range).");
            PlayMissSfx();
            return;
        }

        bool blocked = defender.IsInCover;

        if (blocked)
        {
            PlayBlockSfx();
        }
        else
        {
            PlayHitSfx();
        }

        var dmg = attacker.DealDamage();
        defender.RecieveDamage(dmg);
    }

    private void MoveHorizontal(RectTransform rect, float deltaX)
    {
        var pos = rect.anchoredPosition;
        pos.x += deltaX;

        float minX = arenaRect.rect.xMin;
        float maxX = arenaRect.rect.xMax;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        rect.anchoredPosition = pos;

        PlayStepSfx();
    }

    private void Jump(RectTransform rect, float deltaX)
    {
        var pos = rect.anchoredPosition;
        pos.x += deltaX;

        float minX = arenaRect.rect.xMin;
        float maxX = arenaRect.rect.xMax;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        pos.y += jumpStepY;
        rect.anchoredPosition = pos;

        pos.y -= jumpStepY;
        rect.anchoredPosition = pos;

        PlayStepSfx();
    }

    private void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    private void PlayHitSfx() => PlaySfx(hitClip);
    private void PlayBlockSfx() => PlaySfx(blockClip);
    private void PlayMissSfx() => PlaySfx(missClip);
    private void PlayStepSfx() => PlaySfx(stepClip);
    private void PlayPotionSfx() => PlaySfx(potionDrink);
    private void PlayDeathSfx() => PlaySfx(Death);
}