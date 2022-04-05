using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	/// 
	/// The main game manager
	/// 
	public class GameManager : MonoBehaviour
	{
		
		//creates balance list, should change to private and find value with function give time
		public int currentBalance = 0;

		//all card game objects
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

		//amount bet when the button is hit
		private int bet = 100;

		//list of cards
		List<GameObject> cardhand;
		
		//list of all cards
		List<string> cards = new List<string>
		{
			"c01",
			"c02",
			"c03",
			"c04",
			"c05",
			"c06",
			"c07",
			"c08",
			"c09",
			"c10",
			"c11",
			"c12",
			"c13",
			"d01",
			"d02",
			"d03",
			"d04",
			"d05",
			"d06",
			"d07",
			"d08",
			"d09",
			"d10",
			"d11",
			"d12",
			"d13",
			"h01",
			"h02",
			"h03",
			"h04",
			"h05",
			"h06",
			"h07",
			"h08",
			"h09",
			"h10",
			"h11",
			"h12",
			"h13",
			"s01",
			"s02",
			"s03",
			"s04",
			"s05",
			"s06",
			"s07",
			"s08",
			"s09",
			"s10",
			"s11",
			"s12",
			"s13",
			

		};

		//list for copying cards back into deck
		List<string> ResetDeck = new List<string>
		{
			"c01",
			"c02",
			"c03",
			"c04",
			"c05",
			"c06",
			"c07",
			"c08",
			"c09",
			"c10",
			"c11",
			"c12",
			"c13",
			"d01",
			"d02",
			"d03",
			"d04",
			"d05",
			"d06",
			"d07",
			"d08",
			"d09",
			"d10",
			"d11",
			"d12",
			"d13",
			"h01",
			"h02",
			"h03",
			"h04",
			"h05",
			"h06",
			"h07",
			"h08",
			"h09",
			"h10",
			"h11",
			"h12",
			"h13",
			"s01",
			"s02",
			"s03",
			"s04",
			"s05",
			"s06",
			"s07",
			"s08",
			"s09",
			"s10",
			"s11",
			"s12",
			"s13",
			

		};

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Awake()
		{

			//creates list of cards
			cardhand = new List<GameObject>{
				card1,
				card2,
				card3,
				card4,
				card5
			};

			//set current balance
			currentBalance = 500;
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
			// used for looking at cards
			// foreach(string x in cards){
			// 	Debug.Log(x);
			// }
		}
		
		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Update()
		{
		}

		//draws card from deck, removes card as to not appear again
		public string DrawCard()
		{
			int index = Random.Range(0, cards.Count);
			string reVal = cards[index];
			cards.RemoveAt(index);
			return reVal;
		}

		//checks what hand the player has, if any
		public int checkHand()
		{
			//creates list of card values
			List<int> cardVals = new List<int>();
			foreach(GameObject cardx in cardhand)
			{
				cardVals.Add(cardx.GetComponent<CardScript>().cardVal);
			}

			//orders list of values from smallest to largest
			cardVals.Sort();

			//gets suits of card
			List<char> cardSuits = new List<char>();
			foreach(GameObject cardx in cardhand)
			{
				cardSuits.Add(cardx.GetComponent<CardScript>().cardSuit);
			}

			
			//functions check hand, adds to balance, messages debug log and returns amount won
			bool isRoyalFlush = royal_flush(cardVals, cardSuits);
			if(isRoyalFlush)
			{
				Debug.Log("is royal flush");
				currentBalance += bet * 800;
				return bet * 800;
				
			}
	
			
			bool isStraightFlush = straight_flush(cardVals, cardSuits);
			if(isStraightFlush)
			{
				Debug.Log("is straightflush");
				currentBalance += bet * 50;
				return bet * 50;
				
			}
			

			bool isFour = four_of_a_kind(cardVals);
			if(isFour)
			{
				Debug.Log("is four");
				currentBalance += bet * 25;
				return bet * 25;
				
			}
			

			bool isFull = full_house(cardVals);
			if(isFull)
			{
				Debug.Log("is full");
				currentBalance += bet * 9;
				return bet * 9;
				
			}
			

			bool isFlush = flush(cardSuits);
			if(isFlush)
			{
				Debug.Log("is flush");
				currentBalance += bet * 6;
				return bet * 6;
				
			}
			

			bool isStraight = straight(cardVals);
			if(isStraight)
			{
				Debug.Log("is straight");
				currentBalance += bet * 4;
				return bet * 4;
				
			}
			
			bool isThree = three_of_a_kind(cardVals);
			if(isThree)
			{
				Debug.Log("is three");
				currentBalance += bet * 3;
				return bet * 3;
				
			}
			

			bool isPair = two_pair(cardVals);
			if(isPair)
			{
				Debug.Log("is two pair");
				currentBalance += bet * 2;
				return bet * 2;
				
			}
			

			bool isJacks = jacks_or_better(cardVals);
			if(isJacks)
			{
				Debug.Log("is jacks");
				currentBalance += bet;
				return bet;
				
			}
			
			//returns 0 if no hand is found
			return 0;

			

			// checks card suits and values
			// foreach(char c in cardSuits){
			// 	Debug.Log(c);
			// }
			// foreach(int i in cardVals){
			// 	Debug.Log(i);
			// }


				
		}

		//resets deck
		public void deckReset()
		{
			cards = new List<string>(ResetDeck);
			
		}


		//checks if royal flush has occured
		private bool royal_flush(List<int> cardNums, List<char> cardSuits)
		{
			List<int> royal_flush_check = new List<int>
			{
				10,11,12,13,14
			};

			bool inOrder = Enumerable.SequenceEqual(cardNums, royal_flush_check);
			return inOrder && flush(cardSuits);

		}

		//checks for straight flush, looks at suit and card order
		private bool straight_flush(List<int> cardNums, List<char> cardSuits)
		{
			if(straight(cardNums) && flush(cardSuits)){
				return true;
			}
			return false;
		}

		//checks if card is found 4 times
		private bool four_of_a_kind(List<int> cardNums)
		{
			foreach(int i in cardNums)
			{
				int count = 0;
				for(int j = 0; j < 5; j++)
				{
					if(i == cardNums[j])
					{
						count++;
					}
					if(count == 4){
						return true;
					}
				}
			}
			return false;
		}

		//checks for full house using three of a kind and two pair
		private bool full_house(List<int> cardNums)
		{
			if(three_of_a_kind(cardNums) && two_pair(cardNums) )
			{
				return true;
			}
			return false;
		}

		//checks for flush using card suits
		private bool flush(List<char> cardSuits)
		{
			char c = cardSuits[0];
			foreach(char suit in cardSuits)
			{
				if(suit != c){
					return false;
				}
			}
			return true;
		}

		//checks if cards are in order
		private bool straight(List<int> cardNums)
		{
			// bool ace_check = false;
			// if(cardNums[0] == 1 && cardNums[1] == 10){
			// 	ace_check = true;
			// }
			for(int i = 0; i < 4; i++){ 
				if(cardNums[i] + 1 != cardNums[i + 1]){
					return false;
				}
			}
			return true;
		}

		//checks for three of a kind
		private bool three_of_a_kind(List<int> cardNums)
		{
			
			foreach(int i in cardNums)
			{
				int count = 0;
				for(int j = 0; j < 5; j++)
				{
					if(i == cardNums[j])
					{
						count++;
					}
					if(count == 3){
						return true;
					}
				}
			}
			return false;
		}

		//checks if there are two pairs
		private bool two_pair(List<int> cardNums)
		{
			int count = 0;
			for(int i = 0; i < 4; i++){
				if(cardNums[i] == cardNums[i+1]){
					count++;
					i++;
				}
			}

			if(count == 2){
				return true;
			}
			return false;

		}

		//checks if a jack or better card exists
		private bool jacks_or_better(List<int> cardNums)
		{
			for(int i = 0; i < 5; i++){
				if(cardNums[i] >= 11){
					return true;
				}
			}
			return false;

		}

		//subtracts balance from bet
		public void balanceBet()
		{
			currentBalance -= bet;
		}




	}
}