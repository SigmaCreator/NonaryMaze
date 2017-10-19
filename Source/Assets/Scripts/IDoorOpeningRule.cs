using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoorOpeningRule{

    IDoorOpeningRule CompositeRule { get; set; }

    bool VerifyCode(List<Player> selection, int doorCode);

}
