using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	///
	/// Manages UI including button events and updates to text fields
	/// Manages card UI
	public class UIManager : MonoBehaviour
	{

		//for getting GameManager script
		private GameObject Gmanager;
		private GameManager gameScript;

		//
		private GameObject resultScreen;
		private bool game_over = false;

		Sprite loadcard;

		private bool secondTurn = false;

		[SerializeField]
		private Text currentBalanceText = null;

		[SerializeField]
		private Text winningText = null;

		[SerializeField]
		private Button betButton = null;

		[SerializeField]
		private Button NewDealButton = null;

		[SerializeField]
		private GameObject card1 = null;

		[SerializeField]
		private GameObject card2 = null;

		[SerializeField]
		private GameObject card3 = null;

		[SerializeField]
		private GameObject card4 = null;

		[SerializeField]
		private GameObject card5 = null;

		private int currentbalance;
		List<GameObject> cardhand;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Awake()
		{

			//gets GameManager script
			Gmanager = GameObject.Find("GameManager");
			gameScript = Gmanager.GetComponent<GameManager>();

			//initalizes balance
			currentbalance = gameScript.currentBalance;
			string balance = currentbalance.ToString();
			currentBalanceText.text = "Balance: " + balance + " Credits";  

			
			//creates list of cards 
			cardhand = new List<GameObject>{
				card1,
				card2,
				card3,
				card4,
				card5
			};

			//creates randomized card hand
			// foreach(GameObject cardx in cardhand){
			// 	string drawnCard = gameScript.DrawCard();
			// 	cardx.GetComponent<CardScript>().changeCard(drawnCard);
			// }

			// testing cards
			cardhand[0].GetComponent<CardScript>().changeCard("s11");
			cardhand[1].GetComponent<CardScript>().changeCard("d11");
			cardhand[2].GetComponent<CardScript>().changeCard("c11");
			cardhand[3].GetComponent<CardScript>().changeCard("d01");
			cardhand[4].GetComponent<CardScript>().changeCard("c01");
			


			
			
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
			//adds listener functions to bet and new deal button
			betButton.onClick.AddListener(OnBetButtonPressed);
			NewDealButton.onClick.AddListener(NewDealButtonPressed);
		}

		//-//////////////////////////////////////////////////////////////////////
		///
		/// Event that triggers when bet button is pressed
		/// 
		private void OnBetButtonPressed()
		{

			//if no more credits, updates balance with game over
			if(gameScript.currentBalance == 0)
			{
				updateWinText(0);
			}
			//checks if this is the first pass
			if(secondTurn == false && gameScript.currentBalance > 0){
				//subtracts 100 credits from balance
				gameScript.balanceBet();
				
				
				//checks to see if any cards are being hard, cards not held are replaced with randomized cards
				foreach(GameObject cardx in cardhand)
				{
					string drawnCard = gameScript.DrawCard();
					if(cardx.GetComponent<CardScript>().isHeld == false){
						cardx.GetComponent<CardScript>().changeCard(drawnCard);
					}
					
				}

				//receives credits earned from hand from GameManager
				int gained = gameScript.checkHand();

				
				secondTurn = true;

				//updates current balance text and the win text
				updateBalance();
				updateWinText(gained);
			}


		}

		
		//button pressed to completely restart deck
		private void NewDealButtonPressed()
		{
			//checks if game over has occured, otherwise resets and replaces all cards
			//cancels all holds
			if(secondTurn == true && gameScript.currentBalance > 0 && game_over == false){
				foreach(GameObject cardx in cardhand)
				{
					string drawnCard = gameScript.DrawCard();
					cardx.GetComponent<CardScript>().changeCard(drawnCard);
					cardx.GetComponent<CardScript>().isHeld = false;
					cardx.GetComponent<CardScript>().holdText.SetActive(false);
					
					
				}
				secondTurn = false;

				//shuffles cards back into deck
				gameScript.deckReset();
			}
			//if game over occurs, updates text, resets balance, resets all cards
			else if(game_over == true)
			{
				Debug.Log("here");
				secondTurn = false;
				gameScript.currentBalance = 500;
				updateWinText(0);
				game_over = false;
				foreach(GameObject cardx in cardhand)
				{
					string drawnCard = gameScript.DrawCard();
					cardx.GetComponent<CardScript>().changeCard(drawnCard);
					cardx.GetComponent<CardScript>().isHeld = false;
					cardx.GetComponent<CardScript>().holdText.SetActive(false);
					
					
				}
				updateBalance();

				//shuffles cards back into deck
				gameScript.deckReset();
				
				
			}
		}

		//updates balance text
		private void updateBalance()
		{
			int cardbalance = gameScript.currentBalance;
			string balance = cardbalance.ToString();
			currentBalanceText.text = "Balance: " + balance + " Credits";
		}

		//updates win/lose text
		private void updateWinText(int x)
		{
			//if no credits remaining
			if(x == 0 && gameScript.currentBalance == 0)
			{
				winningText.text = "Game Over";
				game_over = true;
			}

			//is game is reset
			else if(x == 0 && game_over == true)
			{
				winningText.text = "Bet 100 credits";
			}

			//if hand did not reward any credits
			else if(x == 0 && game_over == false)
			{
				winningText.text = "No hand";
			}
			
			//if jacks or better hand has occured
			else
			{
				winningText.text = "Jacks or Better! You won " + x + " credits back";
			}
			
		}
	}
}