using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] private string predicate;
        [SerializeField] private string[] parameters;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluatorsList)
        {
            foreach (IPredicateEvaluator evaluator in evaluatorsList)
            {
                if (evaluator.Evaluate(predicate, parameters) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
