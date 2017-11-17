using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Change this architecture to an abstract class.
// Is the that scriptableobject inheritance required?
public class DigitalRootRule : ScriptableObject, IDoorOpeningRule
{
    IDoorOpeningRule _compositeRule;
    public IDoorOpeningRule CompositeRule
    {
        get { return _compositeRule; }

        set {  _compositeRule = value; }
    }


    // Digital Root rule
    public bool VerifyCode(List<Player> selection, int doorCode)
    {
        int pHash = DigitalRoot(selection);

        return pHash == doorCode && 
               (_compositeRule != null ? _compositeRule.VerifyCode(selection, doorCode) : true);

    }

    private int DigitalRoot(List<Player> selection)
    {
        int pHash = 0;
        foreach (Player p in selection) { pHash += p.Code; }
        pHash %= 9;
        pHash = pHash == 0 ? 9 : pHash;

        return pHash;
    }
}
