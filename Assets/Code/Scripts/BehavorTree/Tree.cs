// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 12/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected abstract Node SetupTree();

        protected void Start()
        {
            _root = SetupTree();
        }

        protected void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }
    }
}