public interface ICombatInstance
{
    void DealDamage();
    void Combat();
}

public class CombatInstance
{
    public SpecialEffect specialEffect;
    public void Combat(CharactorInstance a, CharactorInstance b)
    {
        foreach (PassiveSpell passiveSpell in a)
        {
            //characterInstance here is the reference to the passive spell owner! 
            if (passiveSpell.Hit(PassiveSpellTriggerType.OnCombat, a, b))
            {
                passiveSpell.play(this, a, b, specialEffect);
            }
        }
    }

    public void nextTurn()
    {
        //increase turns last for passive spells
        //
        foreach (CharactorInstance charactorInstance in charactorInstances)
        {
            foreach (PassiveSpell passiveSpell in charactorInstance)
            {
                //characterInstance here is the reference to the passive spell owner!
                if (passiveSpell.Hit(PassiveSpellTriggerType.OnTurnStart))
                {
                    passiveSpell.play(this, charactorInstance);
                }
            }
        }
    }

    public void win()
    {

    }

    public void loss()
    {

    }

}

public class combatInfo{
    public CombatInstance combatInstance;
    public CharactorInstance source;
    public CharactorInstance target;
}




enum PassiveSpellTriggerType
{
    OnWounded,
    OnCombat,
    OnTurnStart,
}

[System.Serializable]
public class PassiveSpellTrigger
{
    public PassiveSpellTriggerType triggerType;
}

/*
SpellConditionType
{
    Probability,
    TargetHasPassiveSpell,
    SourceHasPassiveSpell,
    SourceHasPassiveSpellLastForNTurns,
    TargetHasPassiveSpellLastForNTurns,
} 
instansiate a selection panel for selection, same for act element
*/

[System.Serializable]
public abstract class SpellCondition
{
    abstract public bool IsValide(CharactorInstance source, CharactorInstance target) //also applys to stroy system conditions
    {
        
    }
}

public class TargetHasPassiveSpell : SpellCondition
{
    public string passiveSpellName;
    public bool IsValide(CharactorInstance source=null, CharactorInstance target=null){
        //check if target has the passiveSpell in its list
        
    }
}

public class SourceHasPassiveSpell : SpellCondition
{
    public string passiveSpellName;
}









//spell effects
[System.Serializable]
public abstract class SpellEfftect
{
    public abstract void Play(CombatInstance combatInstance, CharactorInstance source = null, CharactorInstance target = null, SpecialEffect effect)
    {
        effect.flash();
        combatInstance.DealDamage();
        combatInstance.AddPassiveSpell(source, spell);
        combatInstance.RemovePassiveSpell(target);
    }
}

public class Flash : SpellEfftect
{
    public void Play(CombatInstance combatInstance, CharactorInstance source = null, CharactorInstance target = null, SpecialEffect effect)
    {
        combatInstance.effect.flash(source);
    }
}

public class DealDamageToTarget : SpellEfftect
{
    int amount;
    public void Play(CombatInstance combatInstance, CharactorInstance source = null, CharactorInstance target = null, SpecialEffect effect)
    {
        combatInstance.DealDamage(amount);
    }
}

public class AddPassiveSpellToSource : SpellEfftect{
    string passiveSpellName;
    public void Play(CombatInstance combatInstance, CharactorInstance source = null, CharactorInstance target = null, SpecialEffect effect)
    {
        //???Not pure !!!
        combatInstance.AddPassiveSpell(source, passiveSpellName); // combatInstance will preload all passiveSpells into SpellDictionary before combat start
        //OR pure functions
        PassiveSpell ps = combatInstance.SpellDictionary.get(passiveSpellName);
        source.AddPassiveSpell(ps);
        //its not convienent to bind visual effects to a passive spell, so only add visual effect when the passive is suppose to last forever
        //All passive should be acquired through active spell. Therefore no need to worry about init passive spells
    }
}

public class RemovePassiveSpellFromSource : SpellEfftect{
    string passiveSpellName;
    public void Play(CombatInstance combatInstance, CharactorInstance source = null, CharactorInstance target = null)
    {
        PassiveSpell ps = combatInstance.SpellDictionary.get(passiveSpellName);
        source.RemovePassiveSpell(passiveSpellName);   
    }
}

public class TeamGainDefence : SpellEfftect{
    int amount;
    
    public void Play(CombatInstance combatInstance, CharactorInstance source = null, CharactorInstance target = null)
    {
        foreach(CharactorInstance charactorInstance in combatInstance.charactorInstances){
            if(charactorInstance.team == source.team){
                charactorInstance.defence += amount;
            }
        }
    }
}












//Spell class
[System.Serializable]
public class PassiveSpell
{
    public string name;
    PassiveSpellTrigger spellTrigger;
    List<SpellCondition> spellConditions;
    List<SpellEfftect> spellEfftects;
    int turns;

    public Hit(PassiveSpellTriggerType triggerType, CharactorInstance source, CharactorInstance target){
        if(spellTrigger.triggerType!=triggerType){
            return false;
        }
        foreach(SpellCondition spellCondition in spellConditions){
            if(!spellCondition.IsValide(source, target)){
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class Spell
{
    List<SpellCondition> spellConditions;
    List<SpellEfftect> spellEfftects;
    public Hit(CharactorInstance source, CharactorInstance target){
        foreach(SpellCondition spellCondition in spellConditions){
            if(!spellCondition.IsValide(source, target)){
                return false;
            }
        }
        return true;
    }
}

//passive spell sub/unsub to events? Or loop through all passive on charactor instance?