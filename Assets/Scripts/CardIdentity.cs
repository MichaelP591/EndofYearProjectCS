using UnityEngine;
using UnityEngine.UI;
using BaseGame;

public class CardIdentity : MonoBehaviour
{
	[SerializeField] Sprite[] deckOfCards;
	public int number;
    public string suit;
    public Sprite clubs2;
    public Sprite diamonds2;

    private Image childImage;

    void Start()
    {
		PokerCard pokerCardScript = GetComponent<PokerCard>();
		clubs2 = deckOfCards[0];
		Image childImage = GetComponentInChildren<Image>();
        number = pokerCardScript.GetCardNumber();
        suit = pokerCardScript.GetSuit();

        if (childImage != null)
        {
			if (suit.Equals("clubs")) {
				childImage.sprite = clubs2;
				Debug.Log("clubs");
			}
			else {
				childImage.sprite = diamonds2;
				Debug.Log("diamonds");
			}
        }
        else
        {
            Debug.LogWarning("No Image component found in child.");
        }
    }
}
