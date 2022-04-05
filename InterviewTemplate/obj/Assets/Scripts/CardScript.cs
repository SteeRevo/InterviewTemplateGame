using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script for card prefab, contains functions to change image, suit, rank, and is held or not
public class CardScript : MonoBehaviour
{

    public string cardnum;
    public int cardVal;
    public char cardSuit;

    //used to check is card is held
    public bool isHeld = false;

    
    public GameObject holdText;
    private Image image;


    void Awake()
    {
        //assigns image variable
        image = this.GetComponentInChildren<Image>();
        holdText = this.transform.GetChild(1).gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        //adds listener to button
        Button cardButton = this.GetComponent<Button>();
        cardButton.onClick.AddListener(OnCardPressed);
        
        //assigns cardval
        updateCardVal();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //changes image and value to give one
    public void changeCard(string card)
    {
        Sprite mySprite = Resources.Load<Sprite>("Art/Cards/img_card_" + card);
        image.sprite = mySprite;
        updateCardVal();
    }

    //changes held or not held, updates hold text
    private void OnCardPressed()
    {
        

        isHeld = !isHeld;
        holdText.SetActive(isHeld);
        
        
    }

    //changes rank and suit of card based off image sprite name
    private void updateCardVal()
    {
        string temp = image.sprite.name;
        int cardTemp = int.Parse(temp.Substring(temp.Length - 2));

        //ace is counted as high
        if(cardTemp == 1){
            cardTemp = 14;
        }
        cardVal = cardTemp;
        cardSuit = temp[temp.Length - 3];
        
    }

    
}
