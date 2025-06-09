using UnityEngine;
using UnityEngine.UI;
using BaseGame;

public class CardIdentity : MonoBehaviour
{
    public bool showImage1 = true;

    private PokerCard pokerCardScript = GetComponent<PokerCard>();

    public int number;
    public string suit;
    public Sprite clubs2;
    public Sprite diamonds2;

    private Image childImage;

    void Start()
    {
        childImage = GetComponentInChildren<Image>();
        number = pokerCardScript.GetCardNumber();
        suit = pokerCardScript.GetSuit();

        if (childImage != null)
        {
			if (suit.equals("clubs")) {
				childImage.sprite = clubs2;
			}
			else {
				childImage.sprite = diamonds2;
			}
        }
        else
        {
            Debug.LogWarning("No Image component found in child.");
        }
    }
}
