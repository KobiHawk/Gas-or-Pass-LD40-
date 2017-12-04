using UnityEngine;
using System.Collections;

public class BlueCarController : CarController {


    private void boost()
    {
        if (hasGas)
        {
            //gasRemaining += gasPickupAward;

            hasGas = false;
            emergencyGas.updateImage("red");
        }
    }
}
