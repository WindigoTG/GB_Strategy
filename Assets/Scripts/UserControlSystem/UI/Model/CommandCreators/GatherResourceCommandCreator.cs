using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherResourceCommandCreator : CommandCreatorBase<IGatherResourceCommand>
{
    protected override void ClassSpecificCommandCreation(Action<IGatherResourceCommand> creationCallback)
    {
        throw new NotImplementedException();
    }
}
