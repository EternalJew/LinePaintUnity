using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Linepaint
{
    public enum Swipe
    {
        None, Top, Bottom, Left, TopLeft, BottomLeft, Right, TopRight, BottomRight
    }
    public class SwipeController 
    {   
        private Vector2 startPosition, endPosition;
        private LevelManager _levelManager;

        public void SetLevelManager(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
        public void OnUpdate()
        {
            if(Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                endPosition = startPosition;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                endPosition = Input.mousePosition;
                
                if(Vector2.Distance(endPosition, startPosition ) > 0.1f)
                {
                    SwipeDirection();
                }
            }
            
        }
        private Swipe SwipeDirection()
        {
            Swipe direction = Swipe.None;
            Vector2 currentSwipe = endPosition - startPosition;

            float angle = Mathf.Atan2(currentSwipe.y, currentSwipe.x) * (180 / Mathf.PI);

            if(angle > 67.5f && angle < 112.5f)
            {
                direction = Swipe.Top;
            }
            else if(angle < -67.5f && angle > -112.5f)
            {
                direction = Swipe.Bottom;
            }
            else if(angle < -157.5f || angle > 157.5f)
            {
                direction = Swipe.Left;
            }
            else if(angle > -22.5f && angle < 22.5f)
            {
                direction = Swipe.Right;
            }
            else if(angle > 22.5f && angle < 67.5f)
            {
                direction = Swipe.TopRight;
            }
            else if(angle > 112.5f && angle < 157.5f)
            {
                direction = Swipe.TopLeft;
            }
            else if(angle < -22.5f && angle > -67.5f)
            {
                direction = Swipe.BottomRight;
            }
            else if(angle < -112.5f && angle > -157.5f)
            {
                direction = Swipe.BottomLeft;
            }

            if(direction != Swipe.None)
            {
                _levelManager.MoveBrush(direction);
                direction = Swipe.None;
            }

            return direction;
        }
    }
}
