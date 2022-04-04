using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSerializeClassesCollection : MonoBehaviour
{

    public static GameSerializeClassesCollection instance;


    [Serializable]
    public class ObserveTableValues
    {
        public string token;
        public string ticket;
        public string port;
        public bool isHuman;
        public string table_type;
        public string user_image;
        public bool reserve;
        // public string game_type;
    }

    public ObserveTableValues observeTable;


    [Serializable]
    public class PlayerData
    {
        public string token;
        public string ticket;
        public string port;
        public bool isHuman;
        // public bool danmu;
        public string table_type;
        public string game_type;
        public int seatId;
        public int initial_chips;
        public bool auto_rebuy;
        public int auto_rebuy_percentage;
    }

    public PlayerData playerData;


    [Serializable]
    public class Player
    {
        public string playerName;
        public string tournament_id;
        public bool isOnline;
        public int seatId;
        public int initialChips;
        public int clientId;
        public float counter;
        public string tableNumber;
        public string user_image;
    }


    [Serializable]
    public class Players
    {
        public Player player;
        public string tableNumber;
        public int tableStatus;
        public string tournament_id;

    }
    public Players newLocalPlayer;

    [Serializable]
    public class AllPlayers
    {
        public string tableNumber;
        public Player[] players;
        public Basicdata basicData;
        public string currentSocketDisplayName;
        public bool checkActionPlayer;
        public NextChance currentActionPlayerDetails;
        public int tableStatus;
        public bool break_time;

        public string tournament_id;

    }

    public AllPlayers allPlayers;


    [Serializable]
    public class TableInfo
    {
        public string tableNumber;
        public string tournamentId;
        public int status;
        public string roundName;
        public string[] board;
        public int roundCount;
        public int raiseCount;
        public int betCount;
        public int totalBet;
        public int initChips;
        public int currentPlayer;
        public string currentPlayerName;
        public int maxReloadCount;
        public int[] sidePots;
        public string smallBlindUp;
        public string bigBlindUp;
        public int currentLevel;
        public int blindsUpTimestamp;
        [SerializeField]
        public SmallAndBigBlindInfo smallBlind;
        [SerializeField]
        public SmallAndBigBlindInfo bigBlind;
        [SerializeField]
        public SmallAndBigBlindInfo dealer;
        public int commandTimeout;
        public int timer;
        public int[] realSidePots;
        //public int currentPlayer;
    }


    [Serializable]
    public class SmallAndBigBlindInfo
    {
        public string playerName;
        public int seatId;
        public int amount;
        public bool isStraddle;
    }


    [Serializable]
    public class PlayerOnRoundStart
    {
        public string playerName;
        public int chips;
        public bool folded;
        public bool allIn;
        public string[] cards;
        public bool isSurvive;
        public int reloadCount;
        public int seatId;
        public string message;
        public int roundBet;
        public int bet;
        public bool isOnline;
        public bool isHuman;
        public int minBet;
        public int reRaise;
        public string[] action_buttons;

    }


    [Serializable]
    public class RoundStartInfo
    {
        public PlayerOnRoundStart player;
        public TableInfo table;
        public string tournament_id;
    }


    [Serializable]
    public class Basicdata
    {
        public PlayerOnRoundStart[] players;
        public TableInfo table;
    }


    [Serializable]
    public class GamePrepare
    {
        public string tableNumber;
        public int countDown;
    }

   
    [Serializable]
    public class GameStart
    {
        public string msg;
        public string tableNumber;
        public int error_code;
    }


    [SerializeField]
    public class NextDealInfo
    {
        [SerializeField]
        public GameSerializeClassesCollection.PlayerOnRoundStart[] players;
        public GameSerializeClassesCollection.TableInfo table;
        public string tournament_id;
    }


    [Serializable]
    public class Game
    {
        public string[] board;
        public string roundName;
        public int roundCount;
        public int raiseCount;
        public int betCount;
    }
    [Serializable]

    public class NextChance
    {
        [SerializeField]
        public Game game;

        [SerializeField]
        public GameSerializeClassesCollection.PlayerOnRoundStart player;

        public string tournament_id;
        public string tableNumber;
    }


    [Serializable]
    public class Action
    {
        public string action;
        public string playerName;
        public int chips;
        public int seatId;
        public int amount;
    }
    [Serializable]
    public class ActionPerformed
    {
        public Action action;
        public TableInfo table;
        public string tournament_id;
    }

    [Serializable]
    public class ActionClass
    {
        public string action;
        public string tournament_id;
        public string tableNumber;
        public string token; 
        // public int amount;
    }


    [Serializable]
    public class ActionClass2
    {
        public string action;
        public int amount;
        public string tournament_id;
        public string tableNumber;
        public string token;
    }
    [SerializeField]
    public ActionClass2 actionClass2;

 
    [Serializable]
    public class PlayersOnHandComplete
    {
        public string playerName;
        public int chips;
        public bool folded;
        public bool allIn;
        public string[] cards;
        public bool isSurvive;
        public int reloadCount;
        public int seatId;
        public string message;
        public int roundBet;
        public int bet;
        public Hand hand;
        public int winMoney;
        public bool isOnline;
        public bool isHuman;
        public bool newJoinee;

        public bool MP, SP1, SP2, SP3, SP4, SP5, SP6;
    }


    [Serializable]
    public class Hand
    {
        public string[] cards;
        public float rank;
        public string message;
        public string[] final_cards;
    }


    [Serializable]
    public class WinningHand
    {
        public PlayersOnHandComplete[] players;
        public TableInfo table;
        public string tournament_id;
        public int roundInterval;
        public int winning_timeout;
    }

    [SerializeField]
    public WinningHand winningHand;

    [Serializable]
    public class ChipBalance
    {
        public string playerName;
        public int chipBalance;
        public int minBuyIn;
        public int maxBuyIn;
    }
    [Header("For Buy-In")]
    public ChipBalance chipBalance;
    [Header("For Top-Up")]
    public ChipBalance topUpChipBalance;


    [Serializable]
    public class SendTableId
    {
        public string table_id;
        public string playerName;
    }

    public SendTableId tableID;


    [Serializable]
    public class TableCreated
    {
        public bool isCreatorBoard;
    }

    public TableCreated tableCreated;


    [Serializable]
    public class StartTable
    {
        public string token;
        public string ticket;
        public string table_type;
    }

    public StartTable startTheTable;

    [Serializable]
    public class TopUp
    {
        public string token;
        public string tableNumber;
        public int buyIn;
        public int chips;
        public bool isZeroChips;

    }

    public TopUp topUp;

    [Serializable]
    public class OnLowChipBalance
    {
        public string playerName;
        public bool errorStatus;
        public string message;
    }
    public OnLowChipBalance onLowChipBalance;
    public OnLowChipBalance seatOccupied; 


    [Serializable]
    public class OnPlayerLeft
    {
        public string playerName;
        public string clientId;
        public int seatId;
        public int tournment_entries_count;
        public int rank;
        public string tableNumber;
        public string tournament_id;
        public string user_image;
        public bool isGuest;
        public int prize;
    }
    public OnPlayerLeft onCurrentPlayerLeft;





    [Serializable]
    public class OnNewRoundPlayers
    {
        public string playerName;
        public int chips;
        public bool folded;
        public bool allIn;
        public bool isSurvive;
        public int reloadCount;
        public int seatId;
        public bool isActive;
        public string clientId;
        public int roundBet;
        public int bet;
        public bool isOnline;
        public bool newJoinee;
        public bool postBlind;
        public bool wait; 
    }

    [Serializable]
    public class OnNewRound
    {
        public OnNewRoundPlayers[] players;
        public TableInfo table;
        //public int tableNumber;
    }

    public OnNewRound onNewRound;



    [Serializable]
    public class PostBlind
    {
        public bool is_post_blind;
        public string token;
    }

    public PostBlind postBlindData;


    // Disband Table Listener and Emitter data
    [Serializable]
    public class DisbandTable
    {
        public string token;
        public string ticket;
        public string table_type;
        public bool is_disband;
        public bool is_disband_repeat;
    }

    public DisbandTable disbandTable;

    [Serializable]
    public class DisbandTableMessage
    {
        public bool isDisband;
        public string message;
        public string ticket;
        public bool errorStatus;
    }

    public DisbandTableMessage disbandTableMessage;


    [Serializable]
    public class ChipBalanceAndBuyInAuthCheck
    {
        public string token;
        public string ticket;
        public string port;
        public bool isHuman;
        public string table_type;
        public string game_type;
        
    }

    [Header("For Chip Balance And Buy In Auth Check")]
    public ChipBalanceAndBuyInAuthCheck chipBalanceBuyInAuth;


    [Serializable]
    public class BuyInAuthAction
    {

        public string username;
        public string ticket_id;
        public string token;
        public bool is_approved;
        //public bool is_approved_all;
    }

    [Header("For Buy In Auth Action")]
    public BuyInAuthAction buyInAuthAction;


    [Serializable]
    public class BuyInAuthActionAcceptAll
    {

        //public string username;
        public string ticket_id;
        public string token;
       // public bool is_approved;
        public bool is_approved_all;
    }

    [Header("For Buy In Auth Action All")]
    public BuyInAuthActionAcceptAll buyInAuthActionAcceptAll;


    [Serializable]
    public class BuyInAuthRequest
    {

        public string playerName;
        public string clientId;
        public string icountryFlag;
        public string isAuthorised;
    }

    [Header("For Buy In Auth Requesr")]
    public BuyInAuthRequest buyInAuthRequest;


    [Serializable]
    public class BuyInAuthPending
    {

        public string message;
      
    }

    [Header("For Buy In Auth Requesr")]
    public BuyInAuthPending buyInAuthPending;


    [Serializable]
    public class Straddle
    {
        public string straddle_steps;
        public bool is_straddle;
    }

    [Header("For Straddle")]
    public Straddle straddle;

    [Serializable]
    public class Ticket
    {
        public string ticket;
    }
    public Ticket ticket;

    [Serializable]
    public class Ticket2
    {
        public string tournament_id;
    }
    public Ticket2 tournament_id;

    [Serializable]
    public class PlayerStats
    {
        public bool error;
        public string totalGame;
        public int id;
        public float vpip;
        public float prf;
        public float threeBet;
        public float cbet;
        public float vpip_tournament;
        public float pfr_tournament;
        public float threeBet_tournament;
        public float cbet_tournament;
        public string totalTour;
        public string message;
        public string country;
        public string city;
        public string user_image;
        public string statusCode;
    }
    //public class PlayerStats
    //{
    //    public bool error;
    //    public int total_games;
    //    public int id;
    //    public float vpip;
    //    public float pfr;
    //    public float bet;
    //    public float c_bet;
    //    public string message;
    //    public string country;
    //    public string city;
    //    public string user_image;
    //    public string statusCode;
    //}
    public PlayerStats playerStats;

    [Serializable]
    public class ClubId
    {
        //public string club_id;
        public string username;
    }
    public ClubId clubId;

    [Serializable]
    public class ResumeInternet
    {
        public string ticket;
        public string tournament_id; 
        public string playerName;
        public string token;
    }
    public ResumeInternet resumeInternet;

    [Serializable]
    public class LocalPlayerConfirmation
    {
        public string ticket;
        public string playerName;
        public int seatId;
    }
    public LocalPlayerConfirmation localPlayerConfirmation; 
    //.................................................................SOCIAL POKER CLASSES..................................//
    [Serializable]
    public class EnterInSocialGame
    {
        public string stake_type;
        public string token;
        public float buyin;
        public float small_blind;
        public float big_blind; 
        public float fees; 
        public float fee_cap; 
        public string user_image; 
        public bool video;
        public bool user_table_match_preference;
        public double min_buy_in;
        public double max_buy_in;
    }
    public EnterInSocialGame enterInSocialGame;

    [Serializable]
    public class Throwables
    {
        public string source;
        public string destination;
        public string tournament_id;
        public string animation;
        public string ticket;
        public string token;
        public int amount;
    }
    public Throwables throwables;

    [Serializable]
    public class ThrowablesListener
    {
        public string destinationPlayerName;
        public string sourcePlayerName;
        public string animation;
        public string tournament_id;
    }
    public ThrowablesListener throwablesListener;

    [Serializable]
    public class UserChipBalance
    {
        public bool errorStatus;
        public string playerName;
        public int balance;
        public int buyIn;
        public int min_buy_in;
        public int max_buy_in;
        public int stepper;
        public string message;
    }
    public UserChipBalance userChipBalance;

    [Serializable]
    public class Chat
    {
        public string token;
        public string tableNumber;
        public string message;
    }
    public Chat chat;

    [Serializable]
    public class ChatListener
    {
        public string playerName;
        public string message;
    }
    public ChatListener chatListener;

    [Serializable]
    public class ChatHistory
    {
        public string tableNumber;
        public string token;
    }
    public ChatHistory chatHistory;

    [Serializable]
    public class ChatHistoryListener
    {
        public ChatMessage[] chat_message;
    }
    public ChatHistoryListener chatHistoryListener;

    [Serializable]
    public class ChatMessage
    {
        public string playerName;
        public string message;
    }
    [Serializable]
    public class TourneyChat
    {
        public string token;
        public string tournament_id;
        public string message;
    }
    public TourneyChat tourneyChat;

    [Serializable]
    public class TourneyChatListener
    {
        public string playerName;
        public string message;
    }
    public TourneyChatListener tourneyChatListener;

    [Serializable]
    public class TourneyChatHistory
    {
        public string tournament_id;
    }
    public TourneyChatHistory tourneyChatHistory;

    [Serializable]
    public class TourneyChatHistoryListener
    {
        public TourneyChatMessage[] chat_message;
        public string tournament_id;
    }
    public TourneyChatHistoryListener tourneyChatHistoryListener;

    [Serializable]
    public class TourneyChatMessage
    {
        public string playerName;
        public string message;
    }

    [Serializable]
    public class OnEnterInSocialGame
    {
        public bool error;
        public bool tokenMissing;
        public bool paramMissing;
    }
    public OnEnterInSocialGame onEnterInSocialGame;

    [Serializable]
    public class OnResumePlayerLeft
    {
        public string playerName;
        public string user_image;
        public int tournment_entries_count;
        public int rank;
        public int tournament_status;
        public string tableNumber;
    }
    public OnResumePlayerLeft onResumePlayerLeft;

    [Serializable]
    public class OnError
    {
        public string eventname;
        public string token;
        public string message;
    }
    public OnError onError;

    [Serializable]
    public class InternetSpeed
    {
        public double speed; 
        public string token;
        public string tableNumber;
    }
    public InternetSpeed internetSpeed;

    [Serializable]
    public class MultipleLogin
    {
        public string username;
        public string token;
    }
    public MultipleLogin multipleLogin;

    #region Tournament Serializable Classes
    [Serializable]
    public class Tournament
    {
        public int entries;
        public int remaining_player;
        public string start_time;
        public string tournament_description;
        public string table_name;
        public string new_prize;
        public float bounty;
        public int tournament_status;    
        public int level;    
        public int less_prize;    
        public int table_size;    
        public int blinds_up;    
        public int late_registration;    
        public int next_game_break;    
        public int buyIn;    
        public int fee;    
        public string new_starting_chips;    
        public int rebuy;    
        public int min_player_number;    
        public int max_player_number;    
        public string new_avg_satck;    
        public string tournament_id;    
        public string blind_structure;    
        public string payout_structure;
        public float addon;
        public string game_type;
        public int addOnCount;
        public int reBuyCount;
        public int position;
        public Current_Level current_level;
        public Next_Level next_level;
        public Smallest_Stack smallest_stack;
        public Largest_Stack largest_stack;
        public string ticket;

    }
    public Tournament tournament;

    [Serializable]
    public class Current_Level
    {
        public int sb;
        public int bb;
    }

    [Serializable]
    public class Next_Level
    {
        public int sb;
        public int bb;
    }

    [Serializable]
    public class Smallest_Stack
    {
        public int chips;
    }

    [Serializable]
    public class Largest_Stack
    {
        public int chips;
    }

    [Serializable]
    public class TimerData
    {
        public bool error;
    }
    public TimerData timerData;

    [Serializable]
    public class MTTJoinClass
    {
        public string tournament_id;
        public string token;
        public bool isRegistered;
        public bool inside_game_detail;
        public string user_image;
    }
    public MTTJoinClass objMTTJoinClass;

    [Serializable]
    public class Timer
    {
        public string tournament_id;
    }
    public Timer timer;

    [Serializable]
    public class UserRegisterTournament
    {
        public string token;
        public string tournament_id;
        public bool isRegistred;
    }
    public UserRegisterTournament userRegisterTournament;

    [Serializable]
    public class CountDown
    {
        public string tournament_id;
    }
    public CountDown countDown;

    [Serializable]
    public class MttTableListingData
    {
        public MttTableListing[] data;
    }
    public MttTableListingData mttTableListingData;

    [Serializable]
    public class MttTableListing
    {
        public string ticket;
        public int current_players;
        public int max_chips;
        public int min_chips;
    }
    public MttTableListing mttTableListing;

    [Serializable]
    public class AddOn
    {
        public bool errorStatus;
        public int chips;
        public int fees;
        public int buyIn;
    }
    public AddOn addOn;

    [Serializable]
    public class AddOnEmitter
    {
        public string tournament_id;
    }
    public AddOnEmitter addOnEmitter;

    [Serializable]
    public class RebuyChips
    {
        public bool errorStatus;
        public int chips;
        public int fees;
        public int buyIn;
    }
    public RebuyChips rebuyChips;

    [Serializable]
    public class RebuyChipsEmitter
    {
        public string tournament_id;
    }
    public RebuyChipsEmitter rebuyChipsEmitter;

    [Serializable]
    public class TableListing
    {
        public string token;
        public string tournament_id;
    }
    public TableListing tableListing;

    [Serializable]
    public class RankingListing
    {
        public string token;
        public string tournament_id;
        public string user_image;
    }
    public RankingListing rankingListing;

    [Serializable]
    public class TimerCountDown
    {
        public string hours;
        public string minutes;
        public string seconds;
        public string tournament_id;
    }
    public TimerCountDown timerCountDown;

    [Serializable]
    public class MttRankingListingData
    {
        public MttRankingListing[] data;
        public string tournament_id;
    }
    public MttRankingListingData mttRankingListingData;

    [Serializable]
    public class MttRankingListing
    {
        public string username;
        public string new_chips;
        public int chips;
        public int addOn;
        public int rebuy;
        public int k_o;
        public string userimage;
        //public BountyList[] bounty;
    }
    public MttRankingListing mttRankingListing;

    [Serializable]
    public class BountyList
    {
        public string player;
        public float bounty;
    }

    [Serializable]
    public class RewardListing
    {
        public string payout_structure;
        public string tournament_id;
    }
    public RewardListing rewardListing;

    [Serializable]
    public class MttRewardListingData
    {
        public MttRewardListing[] data;
        public string tournament_id;
    }
    public MttRewardListingData mttRewardListingData;

    [Serializable]
    public class MttRewardListing
    {
        public int rank;
        public string payout;
    }
    public MttRewardListing mttRewardListing;

    [Serializable]
    public class MttWinnerListing
    {
        public string tournament_chips;
        public string no_addon_times;
        public string no_rebuy_times;
        public string username;
        public string user_image;
        public string chips_value;
        public int client_id;
        public float chips;
        public float player_bounty;
        public int knock_out;
        public BountyList[] bounty;
    }

    [Serializable]
    public class MttMainTournament
    {
        public int tournament_id; 
        public int min_buy_in; 
        public int min_player; 
        
    }
    public MttWinnerListing mttWinnerListing;

    [Serializable]
    public class MttWinnerListingData
    {
        public MttWinnerListing[] data;
        public MttMainTournament main_tournament;
    }
    public MttWinnerListingData mttWinnerListingData;

    [Serializable]
    public class MttBreak
    {
        public int break_time;
        public int add_break_time;
        public string tournament_id;
    }
    public MttBreak mttBreak;

    [Serializable]
    public class PlayerExist
    {
        public int tournament_status;
        public bool isRegistered;
        public string tournament_id;
        public string playerName;
    }
    public PlayerExist playerExist;

    [Serializable]
    public class PlayerExistListener
    {
        public string playerName;
        public bool exist;
        public bool alreadyRegistered;
    }
    public PlayerExistListener playerExistListener;

    [Serializable]
    public class LateRegistrationEmitter
    {
        public string token;
        public string tournament_id;
    }
    public LateRegistrationEmitter lateRegistrationEmitter;

    [Serializable]
    public class LateRegistrationListener
    {
        public string ticket;
        public int seat_id;
        public bool error;
    }
    public LateRegistrationListener lateRegistrationListener;

    [Serializable]
    public class BlindDetailListener
    {
        public BlindStructure[] blind_structure;
    }
    public BlindDetailListener blindDetailListener;

    [Serializable]
    public class BlindStructure
    {
        public int sb;
        public int bb;
    }

    [Serializable]
    public class BlindDetailEmitter
    {
        public string blind_structure;
        public string tournament_id;
        public string token;
    }
    public BlindDetailEmitter blindDetailEmitter;

    [Serializable]
    public class LateRegistrationEntryEmitter
    {
        public string token;
        public string tournament_id;
        public string ticket;
        public int seat_id;
    }
    public LateRegistrationEntryEmitter lateRegistrationEntryEmitter;

    [Serializable]
    public class TournamentOnwardsTimerEmitter
    {
        public string tournament_id;
    }
    public TournamentOnwardsTimerEmitter tournamentOnwardsTimerEmitter;

    [Serializable]
    public class TournamentOnwardsTimerListener
    {
        public string hours;
        public string minutes;
        public string seconds;
        public string tournament_id;
    }
    public TournamentOnwardsTimerListener tournamentOnwardsTimerListener;

    [Serializable]
    public class TournamentNextLevelTimerListener
    {
        public string minutes;
        public string seconds;
        public string tournament_id;
    }
    public TournamentNextLevelTimerListener nextLevelTimerListener;

    [Serializable]
    public class Rejoin
    {
        public int tournament_status;
        public bool isRegistered;
        public string tournament_id;
        public string user_image;
    }
    public Rejoin rejoin;

    [Serializable]
    public class BlindUpData
    {
        public int table_id;
    }
    public BlindUpData blindUpData;

    [Serializable]
    public class MttBlindsListingData
    {
        public MttBlindsListing[] blinds;
    }
    public MttBlindsListingData mttBlindsListingData;

    [Serializable]
    public class MttBlindsListing
    {
        public float sb;
        public float bb;
    }
    public MttBlindsListing mttBlindsListing;


    [Serializable]
    public class OnManualBuyIn
    {
        public string token;
        public string tableNumber;
        public string user_image;
        public int chips;
        public bool isZeroChips;
    }

    public OnManualBuyIn onManualBuyIn;

    [Serializable]
    public class TopUpListener
    {
        public string chips;
        public bool errorStatus;
    }

    public TopUpListener topUpListener;

    [Serializable]
    public class MttObserverEmitter
    {
        public string tournament_id;
        public string token;
        public string tableNumber;
    }
    public MttObserverEmitter mttObserverEmitter;

    [Serializable]
    public class MttObserverClientId
    {
        public int clientId;
        public bool entry;
        public string player_name;
    }
    public MttObserverClientId mttObserverClientId;
    public class PlayerPosition
    {
        public int position;

    }
    public PlayerPosition playerposition;


    [Serializable]
    public class SwitchTableEmitter
    {
        public string tableNumber;
        public string token;
        public int switch_player_chips;
    }
    public SwitchTableEmitter switchTableEmitter;

    [Serializable]
    public class SwitchTableListener
    {
        public string ticket;
    }
    public SwitchTableListener switchTableListener;


    [Serializable]
    public class UpdateTourStatusData
    {
        public string tournament_id;
        public int tournament_status;
    }
    public UpdateTourStatusData updateTourStatusData;

    //..................................................................//
    #endregion



    private void Awake()
    {
        instance = this;
    }

    [Serializable]
    public class AddingFriendParameters
    {

        public string recipient;
        public string recipient_name;
        public string token;
        public string tableNumber;
        public string sender_name;
        public string sender_id;

    }


    public AddingFriendParameters addfrindRequest;

    [Serializable]
    public class AddingFriendParametersTournament
    {

        public string recipient;
        public string recipient_name;
        public string token;
        public string tournament_id;
        public string sender_name;
        public string sender_id;

    }


    public AddingFriendParametersTournament addfrindRequestTour;

    [Serializable]
    public class InviteParameterClass
    {
        public bool isVideo;
        public string username;
        public string tableNumber;
        public bool pushNotification;
        public string senderUserName;
        public string token;
    }

    public InviteParameterClass inviteInstance;

    [Serializable]
    public class StandUpPlayerEmitter
    {
        public string token;
        public string tableNumber;
        public bool resume;
    }

    public StandUpPlayerEmitter standUpPlayer;

    [Serializable]
    public class StandUpPlayerReceiver
    {
        public string playerName;
        public int clientId;
        public bool isGuest;
        public int seatId;
        public string tableNumber;
        public string message;
    }

    public StandUpPlayerReceiver standUpPlayerReceiver;

    [Serializable]
    public class SitInPlayerEmitter
    {
        public string token;
        public string tableNumber;
        public string user_image; 
        public int seatId;
    }

    public SitInPlayerEmitter sitInPlayerEmitter;

    [Serializable]
    public class JoinFriendParameterClass
    {
        public string table_id;
        public string ticket;
        public string token;
    }

    public JoinFriendParameterClass joinfriendInstance;

    [Serializable]
    public class WaitListRequestClass
    {
        public string ticket;
        public string tableNumber;
        public string token;
        public string reciever_name;
    }

    public WaitListRequestClass waitInstance;

    [Serializable]
    public class ReserveSeatRequestClass
    {
        public string ticket;
        public int seatId;
        public string token;
    }
    public ReserveSeatRequestClass reserveInstance;

    [Serializable]
    public class _st_InviteParameterClass
    {
        public bool isVideo;
        public string username;
        public string tableNumber;
        public bool pushNotification;
        public string senderUserName;
        public string token;
    }

    public _st_InviteParameterClass _st_inviteInstance;
    [Serializable]
    public class ReserveSeatResponseClass
    {
        public bool success;
        public string message;
    }
    public ReserveSeatResponseClass reserveResponseInsance;


    public class CheckForReserveSeatInviteClass
    {
        public bool success;
        public string message;
        public string username;
    }
    public CheckForReserveSeatInviteClass chkreserveInstance;

    public class WaitingListResponse
    {
        public bool errorStatus;
        public string username;
        public string message;
    }

    public WaitingListResponse waitListResInstance;

    [Serializable]
    public class MttRevokeEmitter
    {
        public string tournament_id;
        public string token;
        public string tableNumber;
        public string playerName;
        public int seat_id;
    }
    public MttRevokeEmitter mttRevokeEmitter;

    [Serializable]
    public class LocalPlayerExitHandler 
    {
        public string token; 
    }
    public LocalPlayerExitHandler localPlayerExitHandler;


    [Serializable]
    public class FinalTableMessageData
    {
        public string message;

    }
    public FinalTableMessageData finalTableMessageData;


    [Serializable]
    public class NotifyGameBreak
    {
        public string message;
        public string tournament_id;
        public int break_time;

    }
    public NotifyGameBreak notifyGameBreak;

    [Serializable]
    public class NotifyAddOnBreak
    {
        public string message;
        public string tournament_id;
        public int add_break_time;

    }
    public NotifyAddOnBreak notifyAddOnBreak;

    [Serializable]
    public class CheckPlayerExistEmitter
    {
        public string ticket;
        public string check_username;
        public string token;

    }
    public CheckPlayerExistEmitter checkPlayerExistEmitter;

    [Serializable]
    public class CheckPlayerExistListner
    {
        public bool success;
        public CheckPlayerExistListnerResult result;
        public string message;
        public string clientId;
        public string username;
    }
    [Serializable]
    public class CheckPlayerExistListnerResult
    {
        public bool is_online;
        public string table_id;
    }
    public CheckPlayerExistListner checkPlayerExistListner;


}
