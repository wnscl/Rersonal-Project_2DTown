using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [Header("Talk Ui")]
    public GameObject talkCanvas;
    public Image npcImage;
    public Text npcNameText;
    public Text npcTalkText;
    public Text scoreText;
    public Button yesButton;
    public Button noButton;
    private int talkIndex = 0;
    private string[] npcTalk = { };
    private bool firstSign;

    public Slider hpBar;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void SetInit()
    {
        firstSign = false;
        npcImage.sprite = null;
        npcNameText.text = null;
        npcTalkText.text = null;
        talkIndex = 0; 
        
    }

    public bool SendNpcSign()
    {
        return firstSign;
    }

    void OnYesClicked()
    {
        if (talkIndex == 0)
        {
            talkIndex = 1;
            ShowNpcTalkUi();
            firstSign = true;
            Debug.Log(firstSign + "  대화 신호");
            InteractionManager.Instance.DoYourWork();
        }
        else
        {
            talkIndex = 3;
            ShowNpcTalkUi();
        }
    }

    void OnNoClicked()
    {
        if (talkIndex == 1)
        {
            talkIndex = 3;
            ShowNpcTalkUi();
        }
        else if (talkIndex != 1 && talkIndex != 3)
        {
            talkIndex = 2;
            ShowNpcTalkUi();
        }
    }

    public void ShowNpcBasicUi(Sprite profile,string npcName, string[] npcTalk, bool sign)
    {
        this.npcImage.sprite = profile;
        this.npcTalk = npcTalk;
        npcNameText.text = npcName;
        npcTalkText.text = npcTalk[talkIndex];
        talkCanvas.SetActive(true);
        firstSign = sign;
    }

    public void ShowNpcTalkUi()
    {
        npcTalkText.text = npcTalk[talkIndex];
    }

    public void HideNpcUi()
    {
        talkCanvas.SetActive(false);
        SetInit();
    }

    public void UpdateHpBar(Player player)
    {
        if (hpBar != null && player != null)
        {
            hpBar.value = (float)player.Hp / 100f;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
