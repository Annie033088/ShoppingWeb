using Pashamao.Models;

namespace Pashamao.Utility
{
    public class ValidStateTransition
    {
        public bool IsValidStateTransition(OrderStateEnum originalState, OrderStateEnum selectedState)
        {
            //判斷訂單狀態是否符合轉換規則
            bool inRulesFlag = false;
            StateTransitionRules stateTransitionRules = new StateTransitionRules();

            if (stateTransitionRules.TransitionRules.ContainsKey(originalState))
            {
                inRulesFlag = stateTransitionRules.TransitionRules[originalState].Contains(selectedState);
            }

            return inRulesFlag;
        }
    }
}