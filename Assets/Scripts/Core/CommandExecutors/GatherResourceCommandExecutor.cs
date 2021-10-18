using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GatherResourceCommandExecutor : CommandExecutorBase<IGatherResourceCommand>
{
    public override Task ExecuteSpecificCommand(IGatherResourceCommand command)
    {
        throw new System.NotImplementedException();
    }
}
