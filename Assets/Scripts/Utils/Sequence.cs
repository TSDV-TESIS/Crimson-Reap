using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public class Sequence
    {
        private readonly List<IEnumerator> _preActions = new();
        private IEnumerator _action = null;
        private readonly List<IEnumerator> _postActions = new();

        public IEnumerator Execute()
        {
            foreach (var preAction in _preActions)
            {
                yield return preAction;
            }

            yield return _action;
        
            foreach (var postAction in _postActions)
            {
                yield return postAction;
            }
        }

        public void AddPreAction(IEnumerator action)
        {
            _preActions.Add(action);
        }

        public void SetAction(IEnumerator action)
        {
            _action = action;
        }

        public void AddPostAction(IEnumerator action)
        {
            _postActions.Add(action);
        }
        
        public void RemovePreAction(IEnumerator action)
        {
            _preActions.Remove(action);
        }

        public void RemovePostAction(IEnumerator action)
        {
            _postActions.Remove(action);
        }
    }
}