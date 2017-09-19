using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GameSpace
{
    public class WeaponSlot
    {
        public List<Slot> slots;

        public WeaponSlot()
        {
            slots = new List<Slot>();
        }

        public void Initializa(int slotNumber)
        {
            for (int i = 0; i < slotNumber; i++)
            {
                slots.Add(new Slot());
            }
        }

        public void SetSlot(int slotIndex, WeaponType weaponType, Vector3 slotPosition, float slotAngle)
        {
            if (slotIndex > slots.Count) 
                return;
            slots[slotIndex].weaponType = weaponType;
            slots[slotIndex].slotPosition = slotPosition;
            slots[slotIndex].slotAngle = slotAngle;
        }
    }

    public class Slot
    {
        public Weapon weapon;
        public WeaponType weaponType;
        public Vector3 slotPosition;
        public float slotAngle;

        public Slot()
        {
            weapon = null;
            weaponType = WeaponType.NONE;
            slotPosition = Vector3.Zero;
            slotAngle = 0;
        }
    }
}
