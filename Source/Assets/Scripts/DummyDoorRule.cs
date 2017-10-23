using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Change this architecture to an abstract class.
// Is the that scriptableobject inheritance required?
public class DummyDoorRule : ScriptableObject, IDoorOpeningRule
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
        int pHash = 0;
        foreach (Player p in selection) { pHash += p.Code; }
        pHash %= 9;
        pHash = pHash == 0 ? 9 : pHash;

        return pHash == doorCode && (_compositeRule != null ? VerifyCode(selection, doorCode) : true);

    }
}
