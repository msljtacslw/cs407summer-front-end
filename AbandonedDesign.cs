public class GameService
{

    SpecialEffect specialEffect;

    void Start()
    {
        specialEffect = GetComponent<SpecialEffect>();
        /*background = instansiate(backgroundPrefab).GetComponent<Background>();
        charactorListCanvas = instansiate(charactorListCanvasPrefab).GetComponent<charactorListCanvas>();
        transactionPanel = instansiate(TransactionPanelPrefab).GetComponent<TransactionPanel>();
        mapInstance = instansiate(mapInstancePrefab).GetComponent<MapInstance>();*/

        //all use service class for consistency. Let the service class decide whether to do instansiate&destroy or enable&disable
        backgroundService = GetComponent<BackgroundService>();
        charactorListService = GetComponent<CharactorListService>();
        transactionService = GetComponent<TransactionService>();
        mapService = GetComponent<MapService>();
        combatService = GetComponent<CombatService>();
        dialogueService = GetComponent<DialogueService>();
    }

    //should be implemented in Charactor class
    public void CharactorUnfollow(Charactor follower)
    {
        if (leaderName)
        {
            leaderName = null;
        }
    }

    //should be implemented in Charactor class
    public void CharactorFollow(Charactor leader, Charactor follower)
    {

    }

    //public void getCharactorsInArea();// should be in the Area class

    public void startCombat();
    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }
    public void showDiague()
    {
        startCorotine(DiagueCorotine);
    }

    public void startCombat()
    {
        GameObject combat = instantiate(combatPrefab);
        Combat combatScript = combat.getCoponent<combat>();
        combatScript.init();
    }
    public void showImage(); 

    public void showBackground(); //different from showImage, need deleteBackground to delete while showImage will delete the image after a click

    public void deleteBackground();


}


public class GameController{
    
    public void OnPlayerEnterArea(Area area)
    {
        //player.enterArea(); //call player charactorInstance's method. Seems unecessary because game only needs npcs' location
        //display all charactors in the area on scrollview
        charactorList.AddCharactors(area.GetCharactors());

        //instansiate(charactorListCanvasPrefab).GetComponent<CharactorListCanvas>().init(area.GetCharactorsInArea()); //isn't instansiating prefab already some kind of denpendency injection?


        //search and play any act triggered
        Act actTriggered = script.GetTriggeredAct(TriggerType.EnterArea, area.name);
        if (actTriggered) { actTriggered.play(); };
    }

    public void OnPlayerLeaveArea()
    {
        //leave area should remove all charactor in the area. But don't destory or disable it to prevent the bug caused by overlay area coliders.
    }

    public void OnPlayerInspectInventory()
    {

    }

    public void OnPlayerInspectCharactor(Charactor charactor)
    {

    }

    public void OnPlayerTalkTo(Charactor charactor)
    {
        //search and play any act triggered
        Act actTriggered = script.GetTriggeredAct(TriggerType.TalkTo, charactor.name);
        if (actTriggered) 
        { 
            actTriggered.play(); 
        } else {
            gameService.showDiague();
        }
    }

    public void OnPlayerMakeTransactionWith(Charactor charactor){
        transactionPanel.CreateTransaction(charactor);
    }

    public void OnGameStart()
    {

    }

    public void OnPlayerTransaction()
    {
        GameObject transactionPanel = instantiate(transactionPanelPrefab);
        transactionPanel.init(player, dealer);
    }
}