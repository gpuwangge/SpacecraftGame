using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSpace
{
    public class ActionManager
    {
        
        /*Each action slots indicates a human action type. */
        /*Specifically, 0 is left mouse, 1 is right mouse, 2~5 are quik keyboard commands*/
        ActionSlot[] actionSlots;

        public ActionManager(int maxActionSlot)
        {
            actionSlots = new ActionSlot[maxActionSlot];

            for (int i = 0; i < maxActionSlot; i++)
                actionSlots[i] = new ActionSlot(maxActionSlot);
        }

        public void SetAction(int actionSlotIndex, ActionType weaponIndex)
        {
            for (int i = 0; i < actionSlots[actionSlotIndex].GetActionNum(); i++)
            {
                if (actionSlots[actionSlotIndex].GetAction(i) == ActionType.NONE)
                {
                    actionSlots[actionSlotIndex].SetAction(i, weaponIndex);
                    break;
                }
            }
        }

        public ActionType[] GetAction(int actionSlotIndex)
        {
            ActionType[] r = new ActionType[GetActionNum()];
            for (int i = 0; i < GetActionNum(); i++)
            {
                r[i] = ActionType.NONE;
            }
            for (int i = 0; i < actionSlots[actionSlotIndex].GetActionNum(); i++)
            {
                ActionType x = actionSlots[actionSlotIndex].GetAction(i);
                if(x == ActionType.NONE)
                    return r;
                r[i] = x;
            }
            return r;
        }

        public int GetActionNum()
        {
            return actionSlots.Count();
        }
        

    }

    public class ActionSlot
    {
        private ActionType[] Actions;
        public ActionSlot(int num)
        {
            Actions = new ActionType[num];
            for (int i = 0; i < num; i++)
                Actions[i] = ActionType.NONE;
        }

        public void SetAction(int actionSlot, ActionType weaponIndex)
        {
            Actions[actionSlot] = weaponIndex;
        }

        public ActionType GetAction(int actionSlot)
        {
            return Actions[actionSlot];
        }

        public int GetActionNum()
        {
            return Actions.Count();
        }
    }
}
