using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Change this architecture to an abstract class.
// Is the that scriptableobject inheritance required?
public class ThreeOrFiveRule : ScriptableObject, IDoorOpeningRule
{
    IDoorOpeningRule _compositeRule;
    public IDoorOpeningRule CompositeRule
    {
        get { return _compositeRule; }

        set {  _compositeRule = value; }
    }


    // Quantity rule
    public bool VerifyCode(List<Player> selection, int doorCode)    
    {
        return (3 <= selection.Count && selection.Count <= 5) &&
               (_compositeRule != null ? VerifyCode(selection, doorCode) : true);
    }
}
