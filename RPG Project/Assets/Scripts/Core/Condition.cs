using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        private Disjunction[] and;
        
        public bool Check(IEnumerable<IPredicateEvaluator> evaluatorsList)
        {
            foreach (Disjunction disjunction in and)
            {
                if (!disjunction.Check(evaluatorsList)) return false;
            }
            return true;
        }
        
        [System.Serializable]
        private class Disjunction
        {
            [SerializeField] private Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluatorsList)
            {
                foreach (Predicate predicate in or)
                {
                    if (predicate.Check(evaluatorsList)) return true;
                }
                return false;
            }
        }
        
        [System.Serializable]
        private class Predicate
        {
            [SerializeField] private string predicate;
            [SerializeField] private string[] parameters;
            [SerializeField] private bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluatorsList)
            {
                foreach (IPredicateEvaluator evaluator in evaluatorsList)
                {
                    if (evaluator.Evaluate(predicate, parameters) == negate)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
