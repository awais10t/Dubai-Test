using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//.....
public class ScoreManager : MonoBehaviour     //Added Score Code in this Class
{
    public static ScoreManager Instance { get; private set; } // Singleton instance

    [Header("Score Settings")]
    public int baseMatchScore = 100;         // Points for a successful match
    public int baseMismatchPenalty = -20;   // Points deducted for a mismatch
    public int maxTimeBonus = 50;           // Maximum bonus for fast matches
    public float timeBonusThreshold = 10f;  // Max time for earning bonus

    [Header("Combo Settings")]
    public int comboMultiplier = 2;         // Score multiplier for combos
    public int comboResetTime = 3;          // Time to reset combo if no match

    [Header("UI References")]
    public Text scoreText;                  // UI Text for current score
    public Text highScoreText;              // UI Text for high score
    public Text comboText;                  // UI Text for combo streak

    private int currentScore = 0;           // Current score
    private int currentComboCount = 0;      // Current combo streak
    private float lastMatchTime = 0f;       // Time of the last match
    private int highScore = 0;              // High score

    private const string HighScoreKey = "HighScore"; // Key for PlayerPrefs

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateUI();
    }

    private void Update()
    {
        // Reset combo if time exceeds the limit
        if (Time.time - lastMatchTime > comboResetTime && currentComboCount > 0)
        {
            ResetCombo();
        }
    }

    public void AddMatchScore(float matchTime)
    {
        // Calculate time-based bonus
        int timeBonus = Mathf.Clamp(maxTimeBonus - Mathf.FloorToInt(matchTime / timeBonusThreshold * maxTimeBonus), 0, maxTimeBonus);

        // Increment combo count
        currentComboCount++;
        lastMatchTime = Time.time;

        // Calculate total score for the match
        int totalMatchScore = baseMatchScore + timeBonus + (baseMatchScore * (currentComboCount - 1) * comboMultiplier);
        currentScore += totalMatchScore;

        // Update high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
        }

        UpdateUI();

        Debug.Log($"Match! Combo: {currentComboCount}, Time Bonus: {timeBonus}, Total Match Score: {totalMatchScore}, Current Score: {currentScore}");
    }

    public void AddMismatchPenalty()
    {
        // Apply penalty
        currentScore += baseMismatchPenalty;

        // Ensure score doesn't drop below zero
        if (currentScore < 0)
        {
            currentScore = 0;
        }

        // Reset combo streak
        ResetCombo();

        UpdateUI();

        Debug.Log($"Mismatch! Penalty: {baseMismatchPenalty}, Current Score: {currentScore}");
    }

    private void ResetCombo()
    {
        Debug.Log($"Combo Reset! Previous Combo: {currentComboCount}");
        currentComboCount = 0;
        UpdateUI();
    }

    public void ResetScore()
    {
        currentScore = 0;
        ResetCombo();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }

        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}";
        }

        if (comboText != null)
        {
            comboText.text = currentComboCount > 0 ? $"Combo: x{currentComboCount}" : "";
        }
    }
}
