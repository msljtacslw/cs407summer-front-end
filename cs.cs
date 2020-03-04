/*
children sort by sabling index, act don't need piroity field, it's automaticly piroitilized after being sorted with height
List of services providers: 
Game
Combat needs effect
Effect
*/

public class SubScript
{
    //list of acts, maps, and charactors
}

public class Script
{
    List<SubScript> subscripts;

}

public class Area
{

}

public class Charactor
{
    public string leaderName;
    string areaName;
    public String AreaName
    {
        get
        {
            if (leaderName)
            {
                return leader.AreaName;
            }
            else
            {
                return areaName;
            }
        }
        set
        {
            areaName = value;
        }
    }
}



public class GameService
{

    SpecialEffect specialEffect;

    void Start()
    {
        specialEffect = GetComponent<SpecialEffect>();
        background = instansiate(backgroundPrefab).GetComponent<Background>();
    }
    public void CharactorUnfollow(Charactor follower)
    {
        if (leaderName)
        {
            leaderName = null;
        }
    }

    //function useful
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

    public void loadMap(Map map, Action<Area> onPlayerEnterArea, Action<Area> onPlayerExitArea) //when execute enter area act, if the area is on another map, load map first, than change player instance position, call OnPlayerEnterArea() in Game controller
    {
        foreach (Area area in map)
        {
            instansiate(areaInstancePrefab).GetComponent<AreaInstance>().init(area);

        }
    }
}

//this is a contract defines methods can be used in the class the controller injected in !!!!! Includes the actElement, actElement.play(controller)
public interface IGame
{

}

public class GameController : IGameController
{
    Script script; // the state of the whole app
    //GameService gameService; //because storySerive is also composed with normal game services, there is no need to have storyServices
    Background backgroundPrefab;
    SpecialEffect specialEffect; //visual effects in game, can be used in both gameServices and combat

    //Put all services on the controller object. Grab any Service if needed
    void start()
    {
        specialEffect = GetComponent<SpecialEffect>();
        background = instansiate(backgroundPrefab).GetComponent<Background>();
        charactorListCanvas = instansiate(charactorListCanvasPrefab).GetComponent<charactorListCanvas>();
        transactionPanel = instansiate(TransactionPanelPrefab).GetComponent<TransactionPanel>();
    }

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

    //?? not pure !!!
    //dialogue will dismiss default dialogue but enterArea will not dismiss entering area. Where to handle this logic?
    //
    public void SearchTriggeredActAndPlay(TriggerType triggerType, string interactionObjectName) //it may not only be the interaction with object, it can also be time, for example one day later.
    {
        foreach (Act act in Script.ActiveActs)
        {
            if (act.trigger.Hit(triggerType, interactionObjectName))
            {
                act.play(gameSevice);
            }
        }
    }
}

public class MapInstance
{

}

[System.Serializable]
public class PlayerEnterAreaEvent : UnityEvent<Area>
{
}

public class AreaInstance
{
    IGameController gameController; //use delegate instead?
    public PlayerEnterAreaEvent playerEnterAreaEvent;
    string name;
    public void OnTriggerEnter(Coilder coilder)
    {
        if (coilder == "playerCharactor")
        {
            playerEnterAreaEvent.Invoke(area);//single responsebility, botton shouldn't care about the its listener function's implementation
            //HandleEnterArea() generate charactor list in this area. Should it be directly in this class or in an injected controller?
            //should the check be called in the controller's EnterArea()?
            //HandlePlayerEnterArea has side effects such as search acts triggered, display charactor in the area
        }
    }
    public void OnTriggerExit(Coilder coilder)
    {
        if (coilder == "playerCharactor")
        {
            playerLeaveAreaEvent.Invoke(area);
        }
    }
}

public class SelectingRect
{
    public setRectTransform(Vector2 topLeftCorner, Vector2 buttomRightCorner)
    {
        rectTransform = (RectTransform)transform;
        rectTransform.pos = (topLeftCorner + buttomRightCorner) / 2;
        rectTransform.width = Math.abs(topLeftCorner - buttomRightCorner).x;
        rectTransform.height = Math.abs(topLeftCorner - buttomRightCorner).y;
    }
}

public class ChildrenSelectedableAndMovablePanel
{
    public GameObject selectingRectPrefab;
    List<GameObject> selectedElements;
    Vector2 initialPointerPostion;

    //add handlers to dragablePanel
    public void HandlePointerDown()
    {

    }
    public void HandleBeginDrug()
    {

    }
    public void HandleDrug()
    {

    }
    public void HandlePointerUp()
    {
        //update selection list, add draggablePanel with delegates below to them
    }
    public void HandlePointerDownOnSelectedElements()
    {

    }
    public void HandleBeginDrugOnSelectedElements()
    {

    }
    public void HandleDrugOnSelectedElements()
    {

    }
    public void HandlePointerUpOnSelectedElements()
    {

    }
}

public class SubScriptEditorPanel
{
    public SubScript SubScript
    {
        get
        {

        }
    }

    public void init(SubScript subScript)
    {
        //generate all actPanels and other elements...
    }
}

public class SubScriptPanel
{
    SubScriptEditorPanel subScriptEditorPanel;
    IScriptPanel scriptPanel;
    GameObject subScriptEditorPanelPrefab;

    public Button mainButton;
    public SubScript SubScript
    {
        get
        {
            return scriptEditorPanel.SubScript;
        }
    }

    public init(SubScript subScript, IScriptPanel scriptPanel)
    {
        subScriptEditorPanel = instansiate(subScriptEditorPanelPrefab);
        subScriptEditorPanel.init(subScript);
        this.scriptPanel = scriptPanel;
    }

    //remember to delete resoucese created
    void OnDestory()
    {
        Destroy(subScriptEditorPanel);
    }

}

public class ScriptPanel
{
    public Script Script
    {
        get
        {

        }
        set
        {
            //don't use, use init() instead!!!
        }
    }


}

interface IScriptPanel
{
    void SelectSubScript(SubScriptPanel subScriptPanel);
}

public class ScriptPanel : IScriptPanel
{
    public GameObject deletableButtonPrefab;
    public GameObject subScriptPanelPrefab;
    public ScrollView subScriptButtonScrollView;
    List<SubScriptPanel> subScriptPanels;
    int activeSubScriptPanelIndex;
    public Script Script
    {
        get
        {
            //combine Subscripts in SubscriptPanels
        }
        set
        {
            //don't use, use init() instead!!!
        }
    }
    List<SubScriptPanel> subscriptPanels;
    int currentActiveSubscriptPanelIndex;

    public init(Script script)
    {
        //do setter staff. Don't use setter because setter is suppose to be called at any time. However, in the storyboard system it will only be called once after instansiation.
        subScriptPanels = GetComponentInChildren<SubScriptPanel>().toList();
        activeSubScriptPanelIndex = 0;
    }

    public activeSubScriptPanel(SubScriptPanel subScriptPanel)
    {
        subScriptPanel.GameObject.SetActive(true);
    }

    public addSubScript()
    {
        //
        SubScriptPanel subScriptPanel = instansiate(SubScriptPanelPrefab).GetComponent<SubScriptPanel>();
        SubScriptPanel.init(new SubScript(), subScriptButtonScrollView.content.transform);
        subScriptPanels.Add(subScriptPanel);
    }

    public void SelectSubScript(SubScriptPanel subScriptPanelToSelect)
    {
        foreach (SubScriptPanel subScriptPanel in subScriptPanels)
        {
            if (subScriptPanel == subScriptPanelToSelect)
            {
                subScriptPanel.SetActive(true);
            }
            else
            {
                subScriptPanel.SetActive(false);
            }
        }
    }

    public removeSubScript(int index)
    {
        subScriptPanelToRemove = subscriptPanels(index);
        subscriptPanels.removeAt(index);
        Destory(subScriptPanelToRemove);
    }
}

//??? can be the scriptPanel just like actpanel has AddCondition() and AddActElements()
public class StoryboardCotroller
{

}

public class DiagluesPanel
{
    public GameObject diagluesEditorPanelPrefab;
    public GameObject diagluesEditorPanel;
    public Diaglues Diaglues
    {
        get
        {
            return diagluesEditorPanel.Diaglues;
        }
    }

    void OnDestory()
    {
        Destory(diagluesEditorPanel);
    }
}

public class DiagluesEditorPanel
{
    public DiagluePanel diagluePanelPrefab;
    public Diaglues Diaglues
    {
        get
        {
            List<Diaglue> DiagluePanels = GetComponentsInChildren<DiagluePanel>().Diaglue;
        }
    }

    public init(Diaglues diaglues)
    {
        foreach (Diaglue diaglue in diaglues)
        {
            DiagluePanel diagluePanel = instantiate(diagluePanelPrefab);
            diagluePanel.init(diaglue);
        }
    }
}

public class DiagluePanel
{
    public InputField speakerNameInput;
    public InputField contentInput;
    public InputField optionalProtraitSrcInput;

    public Diaglue Diaglue
    {
        get
        {
            return new Diaglue(speakerNameInput.text, contentInput.text, optionalProtraitSrcInput.text);
        }
    }

    public init(Diaglue diaglue)
    {
        speakerNameInput.text = diaglue.speakerName;
        contentInput.text = diaglue.content;
        optionalProtraitSrcInput = diaglue.optionalProtraitSrc;
    }
}

public class Grooming
{
    start()
    {
        //add grooming image to the gameobject with siblingLast and a size a little larger than the rectTransform
    }
}