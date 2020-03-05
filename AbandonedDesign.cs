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