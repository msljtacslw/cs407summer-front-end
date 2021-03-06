public UnityEvent<CharactorInstance> OnDamage;


public class CombatInstance{
    public void Combat(CharactorInstance a, CharactorInstance b){
        foreach(CharactorInstance charactorInstance in charactorInstances){
            foreach(PassiveSpell passiveSpell in charactorInstance){
                switch(passiveSpell.triggerType){
                    case PassiveSpellTriggerType.OnCombat:
                        break;
                }
            }
        }
    }

    public void checkSpellCondition(CharactorInstance source, CharactorInstance target){
        switch (this.condition)
        case probability
        case target has passiveSpell
        case source has passiveSpell
        case target passiveSpell lasts for n turns
        
    }

    public void nextTurn(){

    }

    public void win(){

    }

    public void loss(){

    }

    public void executeSpellEffect(){

    }
}

enum PassiveSpellTriggerType{
    OnBeDamaged,
    OnDamaging,
    OnBeAttacked,
    OnAttacking,
    TurnStart,
}

[System.Serializable]
public class PassiveSpellTrigger{
    public PassiveSpellTriggerType TriggerType;
    public string payload;
    public bool triggerHit(){ //move to game controller for SSOT?

    }
}


enum SpellConditionType{
    targetHasPassiveSpell,
    hasPassiveSpell,    
    probabilty,
}

[System.Serializable]
public class SpellCondition{
    public SpellConditionType type;
    public string payload;
    public bool ConditionIsValide(){

    }
}
[System.Serializable]
public class SpellEfftect{

}

[System.Serializable]
public class Spell{
    List<PassiveSpellTrigger> spellTrigger;
    List<SpellEfftect> spellEfftects;
    
}


public class PassiveSpellInstance{
    public PassiveSpellInstance(PassiveSpell ps){
        this.ps = ps;
    }
}

//passive spell sub/unsub to events? Or loop through all passive on charactor instance?
