using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

public class GunComponent : ItemComponent
{
    public Gun Gun { get; private set; }
    
    public BaseItem Item => (BaseItem)Gun;

    public void Initialize(BaseItem item)
    {
        Gun = (Gun)item;
    }
}