using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Pistol
{
    void Start()
    {
        ammoCurrent = 5;
        ammoMax = 10;
        ammoBackPack = 30;
        cooldown = 0.2f;
        auto = true;
    }

}

