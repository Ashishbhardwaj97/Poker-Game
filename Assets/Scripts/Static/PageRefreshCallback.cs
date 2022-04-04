using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageRefreshCallback : MonoBehaviour
{
    [SerializeField] private UIRefreshControl m_UIRefreshControl;

    private void Start()
    {
        // Register callback        
        m_UIRefreshControl.OnRefresh.AddListener(RefreshItems);
    }

    private void RefreshItems()
    {
        m_UIRefreshControl.EndRefreshing();
    }

    // Register the callback you want to call to OnRefresh when refresh starts.
    public void OnRefreshGameDetailEntries()
    {
        Debug.Log("OnRefreshGameDetailEntries .......");
        SocialTournamentScript.instance.ClickOnTournamentEntries();
    }

    public void OnRefreshGameDetailsPage()
    {
        //Debug.Log("OnRefreshGameDetailsPage .......");
        //TournamentManagerScript.instance.MttEntyEmitter();
    }

    public void OnRefreshGameDetailRanking()
    {
        Debug.Log("OnRefreshGameDetailRanking .......");
        if (GameSerializeClassesCollection.instance.tournament.tournament_status != 3)
        {
            SocialTournamentScript.instance.ClickOnRanking();
        }
    }


    public void OnRefreshLeaderboard()
    {
        Debug.Log("OnRefreshLeaderboard .......");
        LeadershipScript.instance.LeaderBoardRequest();
    }

    public void OnRefreshRegisteredTournament()
    {
        Debug.Log("OnRefreshRegisteredTournament .......");
        SocialTournamentScript.instance.RegisteredTournamentRequest();
    }

    public void OnRefreshUpcomingTournament()
    {
        Debug.Log("OnRefreshUpcomingTournament .......");
        SocialTournamentScript.instance.TournamentRequest();
    }
}
